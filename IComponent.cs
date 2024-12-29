using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunflowerECS
{
    public interface IComponent
    {
        Entity? Entity { get; set; }

        string MyType { get; }

        Type GetRegisteredType();
    }
}
