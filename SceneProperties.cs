using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SunflowerECS
{
    public sealed partial class Scene
    {
        internal readonly Dictionary<uint, Entity> _entities;

        private readonly HashSet<Entity> entitiesToAdd = [];
        private readonly HashSet<Entity> entitiesToRemove = [];

        private readonly Dictionary<Type, ISystem> _systems;

        private EntityEvent? OnEntityAdded;
        private EntityEvent? OnEntityRemoved;

        [JsonIgnore]
        public object? Data { get; set; } = null;

        internal ComponentEvent? OnComponentAdded;
        internal ComponentEvent? OnComponentRemoved;

        private uint nextID = 0;

        public const uint MAX_ENTITY_COUNT = 0xFFFFFFFB;
        public const uint INVALID_ID = 0xFFFFFFFF;
    }
}
