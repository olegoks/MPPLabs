using System;
using System.Collections.Generic;
using System.Text;

namespace Facker.BaseTypesGenerators
{
    class DoubleGenerator : IGenerator {
        public object Create(Type type)
        {
            if (type != typeof(Double))
            {
                throw new Exception();
            }

            Random rand_generator = new Random();
            int rand_value = rand_generator.Next();
            return (double)rand_value;
        }
        public bool IsTypeValid(Type type)
        {
            return (type == typeof(double));
        }

        public Type GetGeneratedType()
        {
            return typeof(double);
        }

    }
}
