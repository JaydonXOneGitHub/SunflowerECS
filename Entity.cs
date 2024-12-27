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

        internal readonly Dictionary<Type, IComponent> components;
        
        private Scene scene;
        
        internal Entity(Scene scene)
        {
            components = new Dictionary<Type, IComponent>();
            this.scene = scene;
        }
        
        public T AddComponent<T>() where T : class, IComponent, new()
        {
            T component = new T();
            AddComponent<T>(component);
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
        
        public T? GetComponent<T>() where T : class, IComponent, new()
        {
            T component = new();
            return components[component.GetRegisteredType()] as T;
        }
        
        public bool HasComponent<T>() where T : class, IComponent, new()
        {
            return GetComponent<T>() != null;
        }
        
        public void Dispose()
        {
            var tempComponents = new Dictionary<Type, IComponent>(components);
        
            foreach (var typeAndComponent in tempComponents)
            {
                RemoveComponent(typeAndComponent.Value);
            }
        
            components.Clear();
            tempComponents.Clear();
        }
    }
}
