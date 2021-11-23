using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using AssemblyBrowserLibrary.Extensions;

namespace AssemblyBrowserLibrary
{
    public static class AssemblyBrowser
    {

        private static readonly List<Node> extensions = new List<Node>();
        public static List<INode> GetAssemblyInfo(string dllPath)
        {

            //Get assembly.
            Assembly assembly = Assembly.LoadFrom(dllPath);
            //Creating dictionary for 
            var info = new Dictionary<string, INode>();

            foreach (var type in assembly.GetTypes()) {

                if (type.Namespace != null)
                {
                    if (!info.ContainsKey(type.Namespace))
                    {
                        info.Add(type.Namespace, new Node("[namespace]", name: type.Namespace));
                    }
                    INode namespaceNode = info[type.Namespace];
                    var typeNode = type.GetNode();
                    extensions.AddRange(type.GetExtensionMethodNodes());
                    namespaceNode.AddNode(typeNode);
                }
                else
                {
                    Console.WriteLine("Namespace of type: " + type + " is null");
                }
            }

            List<INode> result = info.Values.ToList();
            InsertExtensionMethods(result);
            extensions.Clear();
            return result;
        }

        private static void InsertExtensionMethods(List<INode> nodes)
        {
            foreach (var extensionMethod in extensions)
            {
                var extendedType = extensionMethod.nodes[0].fullType;
                foreach (var namespaceNode in nodes)
                {
                    foreach (var typeNode in namespaceNode.nodes)
                    {
                        if (typeNode.fullType == extendedType)
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
                node.nodes.AddRange(nodes);
            }
        }

    }
}
