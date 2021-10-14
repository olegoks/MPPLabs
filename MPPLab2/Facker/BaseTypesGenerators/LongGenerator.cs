using System;
using System.Collections.Generic;
using System.Text;

namespace Facker.BaseTypesGenerators
{
    class LongGenerator : IGenerator
    {
        public object Create(Type type)
        {
            if (type != typeof(long))
            {
                throw new Exception();
            }

            Random rand_generator = new Random();
            int rand_value = rand_generator.Next();
            return (long)rand_value;
        }
        public bool IsTypeValid(Type type)
        {
            return (type == typeof(long));
        }

    }
}
