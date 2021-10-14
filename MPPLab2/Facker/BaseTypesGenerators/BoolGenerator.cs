using System;
using System.Collections.Generic;
using System.Text;
using Facker;
namespace Facker.BaseTypesGenerators
{
    class BoolGenerator : IGenerator {

        public object Create(Type type)
        {
            if(type != typeof(bool))
            {
                throw new Exception();
            }

            Random rand_generator = new Random();
            int rand_value = rand_generator.Next();
            return ((rand_value % 2) == 0);

        }

        public bool IsTypeValid(Type type)
        {
            return (type == typeof(bool));
        }

    }
}
