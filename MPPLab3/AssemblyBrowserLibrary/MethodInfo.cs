using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace AssemblyBrowserLibrary.Extensions
{
    public static class MethodInfoExtensions
    {
        public static Node GetNode(this MethodInfo methodInfo)
        {
            if (methodInfo.IsFinal && methodInfo.Name == "Finalize")
            {
                return new Node("[finalizer]", accessModifier: methodInfo.GetAccessModifier(), name: "Finalize");
            }
            if (methodInfo.IsConstructor)
            {
                var parametersNodes = methodInfo.GetParameterNodes();
                return new Node("[constuctor]", accessModifier: methodInfo.GetAccessModifier(), name: methodInfo.Name, nodes: parametersNodes);
            }
            if (!methodInfo.IsSpecialName)
            {
                var accessModifier = methodInfo.GetAccessModifier();
                var typeModifier = methodInfo.GetTypeModifier();
                var returnType = methodInfo.ReturnType.ToGenericTypeString();
                var generic = methodInfo.IsGenericMethod ? methodInfo.GetGenericArguments().ToGenericTypeString() : null;
                var name = methodInfo.Name + generic;
                var parameters = methodInfo.GetParameterNodes();
                return new Node("[method]", accessModifier: accessModifier, typeModifier: typeModifier, returnType: returnType, name: name, nodes: parameters);
            }
            return null;
        }

        public static IEnumerable<Node> GetParameterNodes(this MethodInfo methodInfo )
        {
            return (from parameter in methodInfo.GetParameters()
                    select parameter.GetNode()).ToList();
        }

        public static string GetAccessModifier(this MethodInfo methodInfo)
        {
            if (methodInfo.IsPublic)
                return "public";
            if (methodInfo.IsPrivate)
                return "private";
            if (methodInfo.IsFamily)
                return "protected";
            if (methodInfo.IsAssembly)
                return "internal";
            if (methodInfo.IsFamilyOrAssembly)
                return "protected internal";

            return "";
        }

        public static string GetTypeModifier(this MethodInfo methodInfo)
        {
            if (methodInfo.IsAbstract)
                return "abstract";
            if (methodInfo.IsStatic)
                return "static";
            if (methodInfo.IsVirtual)
                return "virtual";
            if (methodInfo.GetBaseDefinition() != methodInfo)
                return "override";

            return "";
        }
    }
}