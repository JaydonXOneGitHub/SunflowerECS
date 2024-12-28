using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SunflowerECS
{
    public abstract class BehaviourComponent : IComponent
    {
        public Entity? Entity { get; set; }

        public virtual Type GetRegisteredType() => typeof(BehaviourComponent);

        [JsonPropertyName("type")]
        public virtual string MyType => nameof(BehaviourComponent);

        public virtual void Dispose() { }

        public virtual void Update() { }

        public virtual void Draw() { }

        public virtual void OnAdded() { }

        public virtual void OnRemoved() { }
    }
}
