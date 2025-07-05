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

                foreach (var component in _components)
                {
                    var ce = component.Entity;

                    if (ce != null && ce.HasComponent(component.GetType()))
                    {
                        ce.RemoveComponent(component);
                    }

                    component.Entity = value;
                }
            }
        }

        private readonly List<T> _components;

        public ComponentCollecton()
        {
            _components = [];
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

            return component;
        }

        public List<T> GetComponents() => _components;


        public override void Update()
        {
            foreach (var component in _components)
            {
                if (component is BehaviourComponent behaviourComponent)
                {
                    behaviourComponent.Update();
                }
            }
        }

        public override void Draw()
        {
            foreach (var component in _components)
            {
                if (component is BehaviourComponent behaviourComponent)
                {
                    behaviourComponent.Draw();
                }
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
