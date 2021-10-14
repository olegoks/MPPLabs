using System;
using System.Collections.Generic;
using System.Text;

namespace Facker
{
    public interface IGenerator {
        public class Exception : System.Exception { }
        public object Create(Type type);
        public bool IsTypeValid(Type type);

    }
}
