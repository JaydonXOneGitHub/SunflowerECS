using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SunflowerECS
{
    public abstract class BehaviourComponent : IComponent, IDisposable
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

        public virtual void OnOtherComponentAdded(IComponent component) { }
        
        public virtual void OnOtherComponentRemoved(IComponent component) { }
    }
}
