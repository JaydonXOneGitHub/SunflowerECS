using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SunflowerECS
{
    public sealed class Entity : IDisposable
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
            components = [];

            this.scene = scene;

            Name = name;
        }

        public T AddComponent<T>() where T : class, IComponent, new()
        {
            T component = new();
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
            AddComponent(component.GetType(), component);
        }

        public void RemoveComponent(Type componentType, IComponent component)
        {
            if (!IsValid())
            {
                throw new EntityException("Entity was disposed!");
            }

            if (component.Entity == null)
            {
                return;
            }

            bool removed = components.Remove(componentType);

            if (removed)
            {
                component.Entity = null;

                if (component is IStateComponent stateComponent)
                {
                    stateComponent.OnRemoved();
                }

                scene?.OnComponentRemoved?.Invoke(component);
            }
        }

        public void RemoveComponent<T>(T component) where T : class, IComponent
        {
            RemoveComponent(typeof(T), component);
        }

        public void RemoveComponent(IComponent component)
        {
            RemoveComponent(component.GetType(), component);
        }


        public IComponent GetComponent(Type componentType)
        {
            if (!IsValid())
            {
                throw new EntityException("Entity was disposed!");
            }

            if (!HasComponent(componentType))
            {
                throw new InvalidOperationException(
                    $"Entity does not have component of type: {TypeRegistry.RegisteredComponentTypes[componentType]}"
                );
            }

            return components[componentType];
        }


        public T? GetComponent<T>() where T : class, IComponent
        {
            return GetComponent(typeof(T)) as T;
        }



        public bool HasComponent<T>() where T : class, IComponent
        {
            return HasComponent(typeof(T));
        }

        public bool HasComponent(Type componentType)
        {
            if (!IsValid())
            {
                throw new EntityException("Entity was disposed!");
            }
            return components.ContainsKey(componentType);
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

            scene?.RemoveEntity(this);

            ID = Scene.INVALID_ID;
        }



        public bool IsValid() => ID != Scene.INVALID_ID;

        public Dictionary<Type, IComponent> GetComponents() => components;

        public T? GetComponentLinear<T>(int iterationCount = 0) where T : class, IComponent
        {
            if (!IsValid())
            {
                throw new EntityException("Entity was disposed!");
            }

            foreach (var component in components.Values)
            {
                if (component is T tComponent)
                {
                    if (iterationCount > 0)
                    {
                        iterationCount--;
                        continue;
                    }

                    return tComponent;
                }
            }

            return null;
        }

        public bool TryGetComponentLinear<T>(out T? component, int iterationCount = 0) where T : class, IComponent
        {
            component = GetComponentLinear<T>(iterationCount);

            return component != null;
        }
    }
}
