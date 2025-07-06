using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunflowerECS
{
    public sealed class ComponentCollecton<T> : BehaviourComponent where T : IComponent, new()
    {
        public override Entity? Entity
        {
            get => base.Entity;
            set
            {
                base.Entity = value;

                PrepareComponents(value);
            }
        }

        private void PrepareComponents(Entity? value)
        {
            foreach (var bc in behaviourComponents)
            {
                bc.OnRemoved();
            }

            foreach (var component in _components)
            {
                var ce = component.Entity;

                if (ce != null && ce.HasComponent(component.GetType()))
                {
                    var comparer = ce.GetComponent(component.GetType());

                    if (ReferenceEquals(comparer, component))
                    {
                        ce.RemoveComponent(component);
                    }
                }

                component.Entity = value;
            }

            foreach (var bc in behaviourComponents)
            {
                bc.OnAdded();
            }
        }

        private readonly List<T> _components;
        private readonly List<BehaviourComponent> behaviourComponents;

        public ComponentCollecton()
        {
            _components = [];
            behaviourComponents = [];
        }

        public T AddComponent()
        {
            return AddComponent(new());
        }

        public T AddComponent(T component)
        {
            if (component.Entity != null)
            {
                return component;
            }

            if (_components.Contains(component))
            {
                return component;
            }

            _components.Add(component);

            if (component is BehaviourComponent bc)
            {
                behaviourComponents.Add(bc);
            }

            return component;
        }

        public List<T> GetComponents() => _components;


        public override void Update()
        {
            foreach (var behaviourComponent in behaviourComponents)
            {
                behaviourComponent.Update();
            }
        }

        public override void Draw()
        {
            foreach (var behaviourComponent in behaviourComponents)
            {
                behaviourComponent.Draw();
            }
        }


        public override void Dispose()
        {
            foreach (var component in _components)
            {
                if (component is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            _components.Clear();
        }

        public T First() => _components.First();

        public T First(Func<T, bool> predicate) => _components.First(predicate);

        public bool TryGetFirst(out T component)
        {
            component = _components.First();

            return component != null;
        }

        public bool TryGetFirst(Func<T, bool> predicate, out T component)
        {
            component = _components.First(predicate);

            return component != null;
        }
    }
}
