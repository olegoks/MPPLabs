using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using static System.Reflection.BindingFlags;

namespace AssemblyBrowserLib.Extensions
{
    public static class TypeExtensions
    {
        public static Node GetNode(this Type type)
        {
            var accessModifier = type.GetAccessModifier();
            var typeModifier = type.GetTypeModifier();
            var classType = type.GetClassType();
            var fullType = type.FullName;
            var name = type.ToGenericTypeString();

            Node typeNode = new Node("[type]", accessModifier: accessModifier, typeModifier: typeModifier,
                classType: classType, fullType: fullType, name: name);

            var typeMembers = type.GetMembers(NonPublic
                                             | Instance
                                             | Public
                                             | Static
                                             | DeclaredOnly);

            foreach (var member in typeMembers)
            {
                Node node = null;
                if (member.MemberType == MemberTypes.Method)
                {   
                    node = ((MethodInfo)member).GetNode();
                }
                else if (member.MemberType == MemberTypes.Property)
                {
                    node = ((PropertyInfo)member).GetNode();
                }
                else if (member.MemberType == MemberTypes.Field)
                {
                    node = ((FieldInfo)member).GetNode();
                }
                else if (member.MemberType == MemberTypes.Event)
                {
                    node = ((EventInfo)member).GetNode();
                }
                else if (member.MemberType == MemberTypes.Constructor)
                {
                    node = ((ConstructorInfo)member).GetNode();
                }
                else
                {
                    node = ((TypeInfo)member).GetNode();
                }
                if (node != null)
                {
                    typeNode.AddNode(node);
                }
            }

            return typeNode;
        }

        public static IEnumerable<Node> GetExtensionMethodNodes(this Type type)
        {
            return (from method in type.GetMethods(Instance | Static | Public | NonPublic | DeclaredOnly)
                    .Where(m => !m.IsSpecialName)
                    .Where(m => m.IsDefined(typeof(ExtensionAttribute), false))
                    let accessModifier = method.GetAccessModifier()
                    let typeModifier = method.GetTypeModifier()
                    let fullType = method.ReturnType.FullName
                    let returnType = method.ReturnType.ToGenericTypeString()
                    let name = method.Name
                    let parameters = method.GetParameterNodes()
                    select new Node("[method]", optional: "[extension]", accessModifier: accessModifier, typeModifier: typeModifier, fullType: fullType, returnType: returnType, name: name, nodes: parameters))
                .ToList();
        }

        public static string ToGenericTypeString(this Type type)
        {
            if (!type.IsGenericType)
                return type.Name;

            var genericTypeName = type.GetGenericTypeDefinition().Name;
            genericTypeName = genericTypeName[..genericTypeName.IndexOf('`')];
            var genericArgs = string.Join(", ",
                type.GetGenericArguments()
                    .Select(ToGenericTypeString).ToArray());
            return genericTypeName + "<" + genericArgs + ">";
        }

        public static string ToGenericTypeString(this Type[] types)
        {
            var listTypes = types.Select(type => type.ToGenericTypeString()).ToList();
            return "<" + string.Join(", ", listTypes) + ">";
        }

        public static string GetAccessModifier(this Type type)
        {
            if (type.IsNestedPublic || type.IsPublic)
                return "public";
            if (type.IsNestedPrivate)
                return "private";
            if (type.IsNestedFamily)
                return "protected";
            if (type.IsNestedAssembly)
                return "internal";
            if (type.IsNestedFamORAssem)
                return "protected internal";
            if (type.IsNestedFamANDAssem)
                return "private protected";
            if (type.IsNotPublic)
                return "private";

            return "";
        }

        public static string GetClassType(this Type type)
        {
            if (type.GetMethods().Any(m => m.Name == "<Clone>$"))
                return "record";
            if (type.IsClass)
                return "class";
            if (type.IsEnum)
                return "enum";
            if (type.IsInterface)
                return "interface";
            if (type.IsGenericType)
                return "generic";
            if (type.IsValueType && !type.IsPrimitive)
                return "structure";

            return "";
        }

        public static string GetTypeModifier(this Type type)
        {
            if (type.IsAbstract && type.IsSealed)
                return "static";
            if (type.IsAbstract)
                return "abstract";
            if (type.IsSealed)
                return "sealed";

            return "";
        }
    }
}