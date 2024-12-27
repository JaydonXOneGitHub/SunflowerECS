using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunflowerECS
{
    public interface ISystem : IDisposable
    {
        void OnEntityAdded(Entity entity);
        void OnEntityRemoved(Entity entity);

        void OnComponentAdded(IComponent component);
        void OnComponentRemoved(IComponent component);
    }
}
