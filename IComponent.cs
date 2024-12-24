using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunflowerECS
{
    public interface IComponent : IDisposable
    {
        Entity? Entity { get; set; }

        Type GetRegisteredType();
    }
}
