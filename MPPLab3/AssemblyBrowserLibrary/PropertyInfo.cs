using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AssemblyBrowserLibrary.Extensions
{
    public static class PropertyInfoExtension
    {
        public static Node GetNode(this PropertyInfo propertyInfo)
        {
            var accessModifier = propertyInfo.GetGetMethod(true).GetAccessModifier();
            var propertyType = propertyInfo.PropertyType.ToGenericTypeString();
            var fullType = propertyInfo.PropertyType.FullName;
            var name = propertyInfo.Name;
            var accessors = propertyInfo.GetAccessorsNodes();
            return new Node("[property]", accessModifier: accessModifier, type: propertyType, fullType: fullType, name: name, nodes: accessors);
        }

        private static IEnumerable<Node> GetAccessorsNodes(this PropertyInfo propertyInfo)
        {
            return (from accessor in propertyInfo.GetAccessors(true)
                    let accessModifier = accessor.GetAccessModifier()
                    let name = accessor.Name
                    select new Node("[accessor]", accessModifier: accessModifier, name: name)).ToList();
        }
    }
}
