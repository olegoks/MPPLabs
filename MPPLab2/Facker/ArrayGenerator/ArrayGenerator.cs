using System;
using System.Collections.Generic;
using Facker.BaseTypesGenerators;
using System.Text;

namespace Facker
{
    class ArrayGenerator : IGenerator
    {
        private Dictionary<Type, IGenerator> baseTypesGenerators;

        public ArrayGenerator() 
        {
           
        }
        private bool IsTypeBasic(Type type)
        {
            return baseTypesGenerators.ContainsKey(type);
        }
        public object Create(Type type) {

            if (!IsTypeValid(type)){
                throw new Exception();
            }
            Type elementType = type.GetElementType();
            CustomFacker facker = new CustomFacker();
            Array result = Array.CreateInstance(elementType, (byte)facker.Create(typeof(byte)));
            for (int i = 0; i < result.Length; i++)
            {
                result.SetValue(facker.Create(elementType), i);
            }
            return result;
        }

        public bool IsTypeValid(Type type)
        {
            return (type.IsArray);
        }
    }
}
