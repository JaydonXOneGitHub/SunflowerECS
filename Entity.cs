using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SunflowerECS
{
    public sealed class Entity
    {
        public uint ID { get; internal set; }

        public string Name;

        internal readonly Dictionary<Type, IComponent> components;
        
        internal Scene? scene;

        public Scene? Scene => scene;

        [JsonIgnore]
        public int ComponentCount => components.Count;

        public bool Enabled
        { 
            get
            {
                if (scene == null)
                {
                    return false;
                }

                return scene._entities.ContainsKey(ID);
            }
            set
            {
                if (value)
                {
                    scene?.RemoveEntity(this);
                }
                else
                {
                    scene?.AddEntity(this);
                }
            }

        }

        internal Entity(Scene scene, string name = "")
        {
            components = new Dictionary<Type, IComponent>();
            this.scene = scene;
            Name = name;
        }
        
        public T AddComponent<T>() where T : class, IComponent, new()
        {
            T component = new T();
            AddComponent(component);
            return component;
        }

        internal void AddComponent(Type componentType, IComponent component)
        {
            if (!IsValid())
            {
                throw new EntityException("Entity was disposed!");
            }

            if (component.Entity != null)
            {
                return;
            }

            if (!components.ContainsKey(componentType))
            {
                components[componentType] = component;
                component.Entity = this;

                if (component is BehaviourComponent behaviourComponent)
                {
                    behaviourComponent.OnAdded();
                }

                scene?.OnComponentAdded?.Invoke(component);
            }
        }
        
        public void AddComponent<T>(T component) where T : class, IComponent
        {
            if (!IsValid())
            {
                throw new EntityException("Entity was disposed!");
            }

            if (component.Entity != null)
            {
                return;
            }
        
            if (!components.ContainsKey(typeof(T)))
            {
                components[typeof(T)] = component;
                component.Entity = this;
        
                if (component is BehaviourComponent behaviourComponent)
                {
                    behaviourComponent.OnAdded();
                }
        
                scene?.OnComponentAdded?.Invoke(component);
            }
        }
        


        public void RemoveComponent<T>(T component) where T : class, IComponent
        {
            if (!IsValid())
            {
                throw new EntityException("Entity was disposed!");
            }

            if (component.Entity == null)
            {
                return;
            }

            bool removed = components.Remove(typeof(T));

            if (removed)
            {
                component.Entity = null;
        
                if (component is BehaviourComponent behaviourComponent)
                {
                    behaviourComponent.OnRemoved();
                }

                scene?.OnComponentRemoved?.Invoke(component);
            }
        }
        


        public T? GetComponent<T>() where T : class, IComponent
        {
            if (!IsValid())
            {
                throw new EntityException("Entity was disposed!");
            }

            if (!HasComponent<T>())
            {
                throw new InvalidOperationException(
                    $"Entity does not have component of type: {TypeRegistry.RegisteredComponentTypes[typeof(T)]}"
                );
            }

            return components[typeof(T)] as T;
        }
        


        public bool HasComponent<T>() where T : class, IComponent
        {
            if (!IsValid())
            {
                throw new EntityException("Entity was disposed!");
            }
            return components.ContainsKey(typeof(T));
        }
        


        public void Dispose()
        {
            if (!IsValid())
            {
                throw new EntityException("Entity was already disposed!");
            }

            var tempComponents = new Dictionary<Type, IComponent>(components);
        
            foreach (var typeAndComponent in tempComponents)
            {
                RemoveComponent(typeAndComponent.Value);

                if (typeAndComponent.Value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        
            components.Clear();
            tempComponents.Clear();

            ID = Scene.INVALID_ID;
        }



        public bool IsValid() => ID != Scene.INVALID_ID;
    }
}
