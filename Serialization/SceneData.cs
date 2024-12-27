using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json.Serialization;

namespace SunflowerECS.Serialization
{
    public sealed class SceneData
    {
        public HashSet<EntityData> Entities { get; set; }

        public static SceneData ToSceneData(Scene scene)
        {
            SceneData sceneData = new SceneData()
            {
                Entities = new HashSet<EntityData>(),
            };

            foreach (var entityPair in scene._entities)
            {
                sceneData.Entities.Add(EntityData.ToEntityData(entityPair.Value));
            }

            return sceneData;
        }

        public static Scene FromSceneData(SceneData sceneData)
        {
            Scene scene = new Scene();

            scene.AddSystem(new BehaviourSystem());

            uint nextID = 0;

            foreach (var entityData in sceneData.Entities)
            {
                var entity = EntityData.FromEntityData(entityData, scene);
                entity.ID = nextID;
                scene.AddEntity(entity);
                nextID++;
            }

            return scene;
        }

        public static void SaveToJson(SceneData data, string jsonPath, JsonConverter<IComponent> converter)
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

        public static void SaveSceneToJson(Scene scene, string jsonPath, JsonConverter<IComponent> converter) => SaveToJson(ToSceneData(scene), jsonPath, converter);

        public static SceneData LoadFromJson(string jsonPath, JsonConverter<IComponent> converter)
        {
            using (var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(converter);
                return JsonSerializer.Deserialize<SceneData>(stream, options)!;
            }
        }

        public static Scene LoadSceneFromJson(string jsonPath, JsonConverter<IComponent> converter) => FromSceneData(LoadFromJson(jsonPath, converter));
    }
}
