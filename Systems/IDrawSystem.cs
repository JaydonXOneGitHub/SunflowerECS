using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunflowerECS.Systems
{
    public interface IDrawSystem : ISystem
    {
        void Draw();
    }
}
