using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using AssemblyBrowserLib.Extensions;
using static System.Reflection.BindingFlags;

namespace AssemblyBrowserLib
{
    public static class AssemblyBrowser
    {
        private static readonly List<Node> Extensions = new();

        public static List<INode> GetAssemblyInfo(string filePath)
        {
            var assembly = Assembly.LoadFrom(filePath);
            var assemblyInfo = new Dictionary<string, INode>();

            foreach (var type in assembly.GetTypes())
            {
                if (type.Namespace != null)
                {
                    if (!assemblyInfo.ContainsKey(type.Namespace))
                    {
                        assemblyInfo.Add(type.Namespace, new Node("[namespace]", name: type.Namespace));
                    }

                    var namespaceNode = assemblyInfo[type.Namespace];
                    var typeNode = type.GetNode();
                    Extensions.AddRange(type.GetExtensionMethodNodes());
                    namespaceNode.AddNode(typeNode);

                }
                else
                {
                    Console.WriteLine("Namespace of type: " + type + " is null");
                }
            }

            var result = assemblyInfo.Values.ToList();
            InsertExtensionMethods(result);
            Extensions.Clear();
            return result;
        }

        private static void InsertExtensionMethods(List<INode> nodes)
        {
            foreach (var extensionMethod in Extensions)
            {
                var extendedType = extensionMethod.Nodes[0].FullType;
                foreach (var namespaceNode in nodes)
                {
                    foreach (var typeNode in namespaceNode.Nodes)
                    {
                        if (typeNode.FullType == extendedType)
                        {
                            typeNode.AddNode(extensionMethod);
                        }
                    }
                }
            }
        }



        public static void AddRange(this Node node, IEnumerable<INode> nodes)
        {
            if (nodes != null)
            {
                node.Nodes.AddRange(nodes);
            }
        }
    }
}