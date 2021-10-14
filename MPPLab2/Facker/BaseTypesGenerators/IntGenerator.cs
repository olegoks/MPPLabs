using System;
using System.Collections.Generic;
using System.Text;

namespace Facker.BaseTypesGenerators
{
    class IntGenerator : IGenerator
    {
        public object Create(Type type)
        {
            if (type != typeof(int))
            {
                throw new Exception();
            }

            Random rand_generator = new Random();
            int rand_value = rand_generator.Next();
            return (int)rand_value;
        }
        public bool IsTypeValid(Type type)
        {
            return (type == typeof(int));
        }
        public Type GetGeneratedType()
        {
            return typeof(int);
        }
    }
}
