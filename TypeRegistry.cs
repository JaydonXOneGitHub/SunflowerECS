using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunflowerECS
{
    public static class TypeRegistry
    {
        internal static readonly Dictionary<Type, Type> RegisteredComponentTypes = new()
        {
            {
                typeof(BehaviourComponent), typeof(BehaviourComponent)
            }
        };

        public static void Bind(Type typeOfComponent, Type registeredType)
            => RegisteredComponentTypes[typeOfComponent] = registeredType;

        public static Type Get(Type typeOfComponent) => RegisteredComponentTypes[typeOfComponent];

        public static bool Remove(Type typeOfComponent) => RegisteredComponentTypes.Remove(typeOfComponent);
    }
}
