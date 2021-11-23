using System.Reflection;

namespace AssemblyBrowserLibrary.Extensions
{
    public static class FieldInfoExtensions
    {
        public static Node GetNode(this FieldInfo fieldInfo)
        {
            var accessModifier = fieldInfo.GetAccessModifier();
            var typeModifier = fieldInfo.GetTypeModifier();
            var fieldType = fieldInfo.FieldType.ToGenericTypeString();
            var fullType = fieldInfo.FieldType.FullName;
            var name = fieldInfo.Name;
            return new Node("[field]", accessModifier: accessModifier, typeModifier: typeModifier,
                type: fieldType, fullType: fullType, name: name);
        }

        private static string GetAccessModifier(this FieldInfo fieldInfo)
        {
            if (fieldInfo.IsPublic)
                return "public";
            if (fieldInfo.IsPrivate)
                return "private";
            if (fieldInfo.IsFamily)
                return "protected";
            if (fieldInfo.IsAssembly)
                return "internal";
            if (fieldInfo.IsFamilyOrAssembly)
                return "protected internal";

            return "";
        }

        private static string GetTypeModifier(this FieldInfo fieldInfo)
        {
            return fieldInfo.IsStatic ? "static" : "";
        }
    }
}