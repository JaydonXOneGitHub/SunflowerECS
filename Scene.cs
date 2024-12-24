using SunflowerECS.Systems;

namespace SunflowerECS
{
    public delegate void EntityEvent(Entity entity);
    public delegate void ComponentEvent(IComponent component);

    public sealed class Scene
    {
        private readonly Dictionary<uint, Entity> _entities;

        private readonly Dictionary<Type, ISystem> _systems;

        private EntityEvent OnEntityAdded;
        private EntityEvent OnEntityRemoved;

        internal ComponentEvent OnComponentAdded;
        internal ComponentEvent OnComponentRemoved;

        public Scene()
        {
            _entities = [];
            _systems = [];
        }

        public bool AddSystem(ISystem system)
        {
            bool valid = _systems.TryAdd(system.GetType(), system);
            if (valid)
            {
                _systems[system.GetType()] = system;
                OnEntityAdded += system.OnEntityAdded;
                OnComponentAdded += system.OnComponentAdded;
                OnEntityRemoved += system.OnEntityRemoved;
                OnComponentRemoved += system.OnComponentRemoved;
            }
            return valid;
        }

        public bool RemoveSystem(ISystem system)
        {
            bool removed = _systems.Remove(system.GetType());

            if (removed)
            {
                OnEntityAdded -= system.OnEntityAdded;
                OnComponentAdded -= system.OnComponentAdded;
                OnEntityRemoved -= system.OnEntityRemoved;
                OnComponentRemoved -= system.OnComponentRemoved;
            }

            return removed;
        }

        public Entity Create()
        {
            Entity entity = new Entity(this);
            entity.ID = Roll();
            _entities[entity.ID] = entity;
            return entity;
        }

        public void AddEntity(Entity entity)
        {
            if (_entities.ContainsKey(entity.ID)) { return; }

            _entities[entity.ID] = entity;

            OnEntityAdded?.Invoke(entity);
        }

        public bool RemoveEntity(Entity entity)
        {
            bool removed = _entities.Remove(entity.ID);

            if (removed)
            {
                OnEntityRemoved?.Invoke(entity);
            }

            return removed;
        }

        public void UpdateBehaviour()
        {
            if (_systems.TryGetValue(typeof(BehaviourSystem), out ISystem? system))
            {
                BehaviourSystem behaviourSystem = (BehaviourSystem)system;

                behaviourSystem.Update();
            }
        }

        public void DrawBehaviour()
        {
            if (_systems.TryGetValue(typeof(BehaviourSystem), out ISystem? system))
            {
                BehaviourSystem behaviourSystem = (BehaviourSystem)system;

                behaviourSystem.Draw();
            }
        }

        public void UpdateGeneral()
        {
            foreach (var system in _systems.Values)
            {
                if (system is IUpdateSystem updateSystem)
                {
                    updateSystem.Update();
                }
            }
        }

        public void DrawGeneral()
        {
            foreach (var system in _systems.Values)
            {
                if (system is IDrawSystem drawSystem)
                {
                    drawSystem.Draw();
                }
            }
        }

        private uint Roll()
        {
            uint result = TrueRoll();

            while (_entities.ContainsKey(result))
            {
                result = TrueRoll();
            }

            return result;
        }

        private uint TrueRoll()
        {
            return (uint)(new Random().Next(0, 100000000));
        }
    }
}

