using System;
using System.Reflection;
using System.Collections.Generic;
using Facker.BaseTypesGenerators;
using System.IO;
namespace Facker
{
    public class CustomFacker
    {
        private Dictionary<Type, IGenerator> baseTypesGenerators;
        private ArrayGenerator arrayGenerator;
        private String pluginsPath;
        public class Exception : System.Exception
        {

        }
        public CustomFacker(String pluginsPath)
        {
            baseTypesGenerators = new Dictionary<Type, IGenerator>();
            baseTypesGenerators.Add(typeof(byte), new ByteGenerator());
            baseTypesGenerators.Add(typeof(double), new DoubleGenerator());
            baseTypesGenerators.Add(typeof(int), new IntGenerator());
            baseTypesGenerators.Add(typeof(long), new LongGenerator());
            baseTypesGenerators.Add(typeof(short), new ShortGenerator());
            baseTypesGenerators.Add(typeof(uint), new UIntGenerator());
            baseTypesGenerators.Add(typeof(ulong), new ULongGenerator());
            baseTypesGenerators.Add(typeof(ushort), new UShortGenerator());
            baseTypesGenerators.Add(typeof(DateTime), new DateTimeGenerator());
            arrayGenerator = new ArrayGenerator();
            this.pluginsPath = pluginsPath;

            List<Assembly> assemblies = new List<Assembly>();

            try
            {
                foreach (string file in Directory.GetFiles(pluginsPath, "*.dll"))
                {
                    try
                    {
                        assemblies.Add(Assembly.LoadFile(file));
                    }
                    catch (BadImageFormatException)
                    { }
                    catch (FileLoadException)
                    { }
                }
            }
            catch (DirectoryNotFoundException)
            { }

            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    foreach (Type typeInterface in type.GetInterfaces())
                    {
                        if (typeInterface.Equals(typeof(IGenerator)))
                        {
                            IGenerator pluginGenerator = (IGenerator)Activator.CreateInstance(type);
                            baseTypesGenerators.Add(pluginGenerator.GetGeneratedType(), pluginGenerator);
                        }
                    }
                }
            }
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

            } else if (type == typeof(System.Char))
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                created = chars[(int)((uint)GetBaseTypeGenerator(typeof(uint)).Create(typeof(uint))) % chars.Length];
            }
            
            return created;
        }
        public T Create<T>()
        {

            return (T)Create(typeof(T));

        }

    }
}
