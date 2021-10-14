using System;
using System.Reflection;
using System.Collections.Generic;
using Facker.BaseTypesGenerators;
namespace Facker
{
    public class CustomFacker
    {

        private Dictionary<Type, IGenerator> baseTypesGenerators;
        private ArrayGenerator arrayGenerator;
        public class Exception : System.Exception
        {

        }
        public CustomFacker()
        {
            baseTypesGenerators = new Dictionary<Type, IGenerator>();
            baseTypesGenerators.Add(typeof(bool), new BoolGenerator());
            baseTypesGenerators.Add(typeof(byte), new ByteGenerator());
            baseTypesGenerators.Add(typeof(double), new DoubleGenerator());
            baseTypesGenerators.Add(typeof(float), new FloatGenerator());
            baseTypesGenerators.Add(typeof(int), new IntGenerator());
            baseTypesGenerators.Add(typeof(long), new LongGenerator());
            baseTypesGenerators.Add(typeof(short), new ShortGenerator());
            baseTypesGenerators.Add(typeof(uint), new UIntGenerator());
            baseTypesGenerators.Add(typeof(ulong), new ULongGenerator());
            baseTypesGenerators.Add(typeof(ushort), new UShortGenerator());
            arrayGenerator = new ArrayGenerator();

        }
        private bool IsTypeBasic(Type type)
        {
            return baseTypesGenerators.ContainsKey(type);
        }
        private IGenerator GetBaseTypeGenerator(Type type)
        {
            return baseTypesGenerators[type];
        }
        private object CreateObjectByConstructor(Type type)
        {
            ConstructorInfo[] constructorsInfos = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructorsInfos.Length == 0)
            {
                throw new Exception();
            }
            ConstructorInfo constructor = constructorsInfos[0];
            var parametersValues = new List<object>();
            var parametersInfos = constructor.GetParameters();
            foreach (ParameterInfo parameterInfo in parametersInfos)
            {
                parametersValues.Add(Create(parameterInfo.ParameterType));
            }
            return constructor.Invoke(parametersValues.ToArray());
        }
        public object Create(Type type)
        {
            object created = null;

            if (IsTypeBasic(type))
            {
                IGenerator baseTypeGenerator = GetBaseTypeGenerator(type);
                created = baseTypeGenerator.Create(type);
            } else if (type.IsArray)
            {
                created = arrayGenerator.Create(type);
            }
            else if (type.IsClass && !type.IsGenericType)
            {
                created = CreateObjectByConstructor(type);
            } else if (type.IsClass && type.IsGenericType)
            {

            }
            
            return created;
        }
        public T Create<T>()
        {

            return (T)Create(typeof(T));

        }

    }
}
