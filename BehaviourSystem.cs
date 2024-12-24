using SunflowerECS.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunflowerECS
{
    public sealed class BehaviourSystem : IUpdateSystem, IDrawSystem
    {
        private readonly HashSet<BehaviourComponent?> behaviourComponents = [];

        public void OnEntityAdded(Entity entity)
        {
            //behaviourComponents.Add(entity.GetComponent<BehaviourComponent>());
        }

        public void OnEntityRemoved(Entity entity)
        {
            //behaviourComponents.Remove(entity.GetComponent<BehaviourComponent>());
        }

        public void OnComponentAdded(IComponent component)
        {
            if (component is BehaviourComponent componentBehaviour)
            {
                behaviourComponents.Add(componentBehaviour);
            }
        }

        public void OnComponentRemoved(IComponent component)
        {
            if (component is BehaviourComponent componentBehaviour)
            {
                behaviourComponents.Remove(componentBehaviour);
            }
        }

        public void Draw()
        {
            foreach (var component in behaviourComponents)
            {
                component!.Draw();
            }
        }

        public void Update()
        {
            foreach (var component in behaviourComponents)
            {
                component!.Update();
            }
        }
    }
}
