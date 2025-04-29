using SunflowerECS.Systems;
using System.Text.Json.Serialization;

namespace SunflowerECS
{
    public delegate void EntityEvent(Entity entity);
    public delegate void ComponentEvent(IComponent component);
    
    public sealed partial class Scene : IDisposable
    {
        public Scene()
        {
            _entities = [];
            _systems = [];
        }
    
        public bool AddSystem(ISystem system)
        {
            if (!_systems.ContainsKey(system.GetType()))
            {
                _systems[system.GetType()] = system;

                OnEntityAdded += system.OnEntityAdded;
                OnComponentAdded += system.OnComponentAdded;
                OnEntityRemoved += system.OnEntityRemoved;
                OnComponentRemoved += system.OnComponentRemoved;

                return true;
            }
            return false;
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

        public T GetDataAs<T>()
        {
            if (Data is T tData)
            {
                return tData;
            }

            throw new InvalidCastException($"Cannot cast {nameof(Data)} to {typeof(T)}");
        }
    
        public Entity Create(string name = "Entity")
        {
            if (nextID + 1 > MAX_ENTITY_COUNT)
            {
                throw new Exception("Max entity limit reached!");
            }

            Entity entity = new Entity(this)
            {
                ID = nextID,
                Name = name
            };
            nextID++;
            _entities[entity.ID] = entity;
            return entity;
        }
    
        public void AddEntity(Entity? entity)
        {
            if (entity == null)
            {
                return;
            }

            if (entity.ID == INVALID_ID)
            {
                throw new EntityException("Entity has become invalid!");
            }

            if (_entities.ContainsKey(entity.ID)) { return; }

            entitiesToAdd.Add(entity);
        }
    
        public Entity? GetByID(uint id)
        {
            if (_entities.TryGetValue(id, out var entity))
            {
                return entity;
            }
            return null;
        }

        public Entity? GetEntityByName(string name)
        {
            foreach (var entity in _entities.Values)
            {
                if (entity.Name.Equals(name))
                {
                    return entity;
                }
            }

            return null;
        }
    
        public bool RemoveEntity(Entity? entity)
        {
            if (entity == null)
            {
                return false;
            }

            bool removed = _entities.Remove(entity.ID);
    
            if (removed)
            {
                entitiesToRemove.Add(entity);
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

        public void PrepareForNextIteration()
        {
            AddQueuedEntities();
            RemoveQueuedEntities();
        }
    
        public void UpdateGeneral()
        {
            Parallel.ForEach(_systems.Values, system =>
            {
                if (system is IUpdateSystem updateSystem)
                {
                    updateSystem.Update();
                }
            });
        }
    
        public void DrawGeneral()
        {
            Parallel.ForEach(_systems.Values, system =>
            {
                if (system is IDrawSystem drawSystem)
                {
                    drawSystem.Draw();
                }
            });
        }
    
        public T? GetSystem<T>() where T : class, ISystem
        {
            if (_systems.TryGetValue(typeof(T), out ISystem? system))
            {
                return system as T;
            }
            return null;
        }
    
        public void Dispose()
        {
            foreach (var system in _systems.Values)
            {
                system.Dispose();
                OnEntityAdded -= system.OnEntityAdded;
                OnEntityRemoved -= system.OnEntityRemoved;
                OnComponentAdded -= system.OnComponentAdded;
                OnComponentRemoved -= system.OnComponentRemoved;
            }
            _systems.Clear();

            foreach (var entity in _entities.Values)
            {
                entity.Dispose();
            }
            _entities.Clear();
        }

        private void AddQueuedEntities()
        {
            if (entitiesToAdd.Count == 0)
            {
                // If there are no queued entities, do nothing
                return;
            }

            foreach (var entity in entitiesToAdd)
            {
                if (entity.scene != null)
                {
                    continue;
                }

                _entities[entity.ID] = entity;

                entity.scene = this;

                OnEntityAdded?.Invoke(entity);

                foreach (var component in entity.components.Values)
                {
                    OnComponentAdded?.Invoke(component);
                }
            }
            entitiesToAdd.Clear();
        }

        public int EntityCount() => _entities.Values.Count;

        private void RemoveQueuedEntities()
        {
            if (entitiesToRemove.Count == 0)
            {
                // If there are no queued entities, do nothing
                return;
            }

            foreach (var entity in entitiesToRemove)
            {
                if (entity.scene != this)
                {
                    continue;
                }

                OnEntityRemoved?.Invoke(entity);

                entity.scene = null;

                foreach (var component in entity.components.Values)
                {
                    OnComponentRemoved?.Invoke(component);
                }
            }
            entitiesToRemove.Clear();
        }
    }
}

