using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunflowerECS
{
    public sealed class Entity
    {
        public uint ID { get; internal set; }

        public string Name;

        internal readonly Dictionary<Type, IComponent> components;
        
        private Scene scene;

        public bool Enabled
        { 
            get => scene._entities.ContainsKey(ID);
            set
            {
                if (value)
                {
                    scene.RemoveEntity(this);
                }
                else
                {
                    scene.AddEntity(this);
                }
            }

        }

        internal Entity(Scene scene)
        {
            components = new Dictionary<Type, IComponent>();
            this.scene = scene;
        }
        
        public T AddComponent<T>() where T : class, IComponent, new()
        {
            T component = new T();
            AddComponent(component);
            return component;
        }
        
        public void AddComponent<T>(T component) where T : class, IComponent
        {
            if (component.Entity != null)
            {
                return;
            }
        
            if (!components.ContainsKey(component.GetRegisteredType()))
            {
                components[component.GetRegisteredType()] = component;
                component.Entity = this;
        
                if (component is BehaviourComponent behaviourComponent)
                {
                    behaviourComponent.OnAdded();
                }
        
                scene.OnComponentAdded?.Invoke(component);
            }
        }
        
        public void RemoveComponent(IComponent component)
        {
            if (component.Entity == null)
            {
                return;
            }
        
            if (components.ContainsKey(component.GetRegisteredType()))
            {
                components.Remove(component.GetRegisteredType());
                component.Entity = null;
        
                if (component is BehaviourComponent behaviourComponent)
                {
                    behaviourComponent.OnRemoved();
                }
        
                scene.OnComponentRemoved?.Invoke(component);
            }
        }
        
        public T? GetComponent<T>() where T : class, IComponent
        {
            return components[TypeRegistry.RegisteredComponentTypes[typeof(T)]] as T;
        }
        
        public bool HasComponent<T>() where T : class, IComponent
        {
            return components.ContainsKey(
                TypeRegistry.RegisteredComponentTypes[typeof(T)]
            );
        }
        
        public void Dispose()
        {
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
        }
    }
}
