using System;
using System.Collections.Generic;
using System.Text;

namespace Facker.BaseTypesGenerators
{
    class ShortGenerator : IGenerator
    {
        public object Create(Type type)
        {
            if (type != typeof(short))
            {
                throw new Exception();
            }

            Random rand_generator = new Random();
            int rand_value = rand_generator.Next();
            return (short)rand_value;
        }
        public bool IsTypeValid(Type type)
        {
            return (type == typeof(short));
        }
        public Type GetGeneratedType()
        {
            return typeof(short);
        }
    }
}
