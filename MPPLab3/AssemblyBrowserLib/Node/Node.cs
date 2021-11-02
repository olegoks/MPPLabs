using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AssemblyBrowserLib;
namespace AssemblyBrowserLib
{
    public class Node : INode
    {
        public string Optional { get; set; }
        public string NodeType { get; set; }
        public string AccessModifier { get; set; }
        public string TypeModifier { get; set; }
        public string ClassType { get; set; }
        public string Type { get; set; }
        public string FullType { get; set; }
        public string ReturnType { get; set; }
        public string Name { get; set; }
        public List<INode> Nodes { get; } = new();

        public Node
        (
            string nodeType,
            string optional = "",
            string accessModifier = "",
            string typeModifier = "",
            string classType = "",
            string type = "",
            string fullType = "",
            string returnType = "",
            string name = "",
            IEnumerable<INode> nodes = null
        )
        {
            this.Optional = optional;
            this.NodeType = nodeType;
            this.AccessModifier = accessModifier;
            this.TypeModifier = typeModifier;
            this.ClassType = classType;
            this.Type = type;
            this.FullType = fullType;
            this.ReturnType = returnType;
            this.Name = name;
            this.AddRange(nodes);
        }

        public void AddNode(INode node)
        {
            if (node != null)
            {
                Nodes.Add(node);
            }
        }
    }
}