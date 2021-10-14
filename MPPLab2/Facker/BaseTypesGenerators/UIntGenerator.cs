using System;
using System.Collections.Generic;
using System.Text;

namespace Facker.BaseTypesGenerators
{
    class UIntGenerator : IGenerator
    {
        public object Create(Type type)
        {
            if (type != typeof(uint))
            {
                throw new Exception();
            }

            Random rand_generator = new Random();
            int rand_value = rand_generator.Next();
            return (uint)rand_value;
        }
        public bool IsTypeValid(Type type)
        {
            return (type == typeof(uint));
        }
    }
}
