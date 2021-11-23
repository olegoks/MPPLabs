using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLibrary {
    public class Node : INode {
        public string optional { get; set; }
        public string nodeType { get; set; }
        public string accessModifier { get; set; }
        public string typeModifier { get; set; }
        public string classType { get; set; }
        public string type { get; set; }
        public string fullType { get; set; }
        public string returnType { get; set; }
        public string name { get; set; }
        public List<INode> nodes { get; } = new List<INode>();

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
            this.optional = optional;
            this.nodeType = nodeType;
            this.accessModifier = accessModifier;
            this.typeModifier = typeModifier;
            this.classType = classType;
            this.type = type;
            this.fullType = fullType;
            this.returnType = returnType;
            this.name = name;
            this.AddRange(nodes);
        }
        public void AddNode(INode node)
        {
            this.nodes.Add(node);
        }

        public void AddRange(IEnumerable<INode> nodes)
        {
            if (nodes != null)
            {
                this.nodes.AddRange(nodes);
            }
        }
    }
}