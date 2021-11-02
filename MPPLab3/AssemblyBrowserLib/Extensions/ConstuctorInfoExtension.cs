using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib.Extensions
{
    public static class ConstuctorInfoExtension
    {
        public static Node GetNode(this ConstructorInfo constructorInfo)
        {
            var accessModifier = constructorInfo.GetAccessModifier();
            var parameters = constructorInfo.GetParameterNodes();
            return new Node("[constructor]", accessModifier: accessModifier, name: "Constructor", nodes: parameters);
        }
    }
}
