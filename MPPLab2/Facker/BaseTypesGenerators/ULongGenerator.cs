using System;
using System.Collections.Generic;
using System.Text;

namespace Facker.BaseTypesGenerators
{
    class ULongGenerator : IGenerator
    {
        public object Create(Type type)
        {
            if (type != typeof(ulong))
            {
                throw new Exception();
            }

            Random rand_generator = new Random();
            int rand_value = rand_generator.Next();
            return (ulong)rand_value;
        }
        public bool IsTypeValid(Type type)
        {
            return (type == typeof(ulong));
        }
        public Type GetGeneratedType()
        {
            return typeof(ulong);
        }
    }
}
