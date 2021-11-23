using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AssemblyBrowserLibrary.Extensions
{
    public static class MethodBaseExtensions
    {
        public static string GetAccessModifier(this MethodBase constrInfo)
        {
            if (constrInfo.IsPublic)
                return "public";
            if (constrInfo.IsPrivate)
                return "private";
            if (constrInfo.IsFamily)
                return "protected";
            if (constrInfo.IsAssembly)
                return "internal";
            if (constrInfo.IsFamilyOrAssembly)
                return "protected internal";

            return "";
        }

        public static IEnumerable<Node> GetParameterNodes(this MethodBase methodInfo)
        {
            return (from parameter in methodInfo.GetParameters()
                    select parameter.GetNode()).ToList();
        }

        public static string GetTypeModifier(this MethodBase methodBase)
        {
            return methodBase.IsStatic ? "static" : "";
        }
    }
}