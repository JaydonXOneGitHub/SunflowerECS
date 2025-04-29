using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunflowerECS
{
    public sealed class EntityException : Exception
    {
        public EntityException(string message) : base($"Entity error: {message}") { }
    }
}
