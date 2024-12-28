using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SunflowerECS.Serialization
{ 
    public sealed class EntityData
    {
        public HashSet<IComponent> Components { get; set; }

        public string Name { get; set; }

        public static EntityData ToEntityData(Entity entity)
        {
            return new EntityData()
            {
                Components = new(entity.components.Values),
                Name = entity.Name
            };
        }

        public static Entity FromEntityData(EntityData entityData, Scene scene)
        {
            Entity entity = new Entity(scene)
            {
                Name = entityData.Name,
            };

            foreach (var component in entityData.Components)
            {
                entity.components.Add(component.GetRegisteredType(), component);
            }

            return entity;
        }

        public static void SaveToJson(EntityData data, string jsonPath, JsonConverter<IComponent> converter)
        {
            using (var stream = new FileStream(jsonPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    var options = new JsonSerializerOptions();
                    options.Converters.Add(converter);
                    options.WriteIndented = true;
                    string json = JsonSerializer.Serialize(data, options);
                    writer.Write(json);
                }
            }
        }

        public static void SaveEntityToJson(Entity entity, string jsonPath, JsonConverter<IComponent> converter) => SaveToJson(ToEntityData(entity), jsonPath, converter);

        public static EntityData LoadFromJson(string jsonPath, JsonConverter<IComponent> converter)
        {
            using (var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(converter);
                return JsonSerializer.Deserialize<EntityData>(stream, options)!;
            }
        }

        public static Entity LoadEntityFromJson(
            string jsonPath, 
            Scene scene,
            JsonConverter<IComponent> converter) => FromEntityData(LoadFromJson(jsonPath, converter), scene);
    }
}
