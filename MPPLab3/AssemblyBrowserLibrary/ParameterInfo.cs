using System.Reflection;

namespace AssemblyBrowserLibrary.Extensions
{
    public static class ParameterInfoExtensions
    {
        public static Node GetNode(this ParameterInfo parameterInfo)
        {
            var typeModifier = parameterInfo.GetTypeModifier();
            var parameterType = parameterInfo.ParameterType.ToGenericTypeString();
            var fullType = parameterInfo.ParameterType.FullName;
            var name = parameterInfo.Name;
            return new Node("[param]", typeModifier: typeModifier, type: parameterType, fullType: fullType, name: name);
        }


        public static string GetTypeModifier(this ParameterInfo parameterInfo)
        {
            if (parameterInfo.IsRetval)
                return "ret";
            if (parameterInfo.IsIn)
                return "in";
            if (parameterInfo.IsOut)
                return "virtual";

            return "";
        }
    }
}