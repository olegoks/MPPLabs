using System;
using System.Collections.Generic;
using System.Text;
using Facker;
namespace Facker.BaseTypesGenerators
{
    class FloatGenerator : IGenerator
    {
        public object Create(Type type)
        {
            if (type != typeof(float))
            {
                throw new Exception();
            }

            Random rand_generator = new Random();
            int rand_value = rand_generator.Next();
            return (float)rand_value;
        }
        public bool IsTypeValid(Type type)
        {
            return (type == typeof(float));
        }
        public Type GetGeneratedType()
        {
            return typeof(float);
        }
    }
}
