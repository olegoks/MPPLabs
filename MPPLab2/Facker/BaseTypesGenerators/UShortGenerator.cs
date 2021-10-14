using System;
using System.Collections.Generic;
using System.Text;

namespace Facker.BaseTypesGenerators
{
    class UShortGenerator : IGenerator
    {
        public object Create(Type type)
        {
            if (type != typeof(ushort))
            {
                throw new Exception();
            }
            Random rand_generator = new Random();
            int rand_value = rand_generator.Next();
            return (ushort)rand_value;
        }
        public bool IsTypeValid(Type type)
        {
            return (type == typeof(ushort));
        }
        public Type GetGeneratedType()
        {
            return typeof(ushort);
        }
    }
}
