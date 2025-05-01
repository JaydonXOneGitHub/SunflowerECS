using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunflowerECS
{
    public interface IStateComponent : IComponent
    {
        void OnAdded();
        void OnRemoved();
    }
}
