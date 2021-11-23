using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLibrary {
    public interface INode {
        string optional { get; set; }
        string nodeType { get; set; }
        string accessModifier { get; set; }
        string typeModifier { get; set; }
        string classType { get; set; }
        string type { get; set; }
        string fullType { get; set; }
        string returnType { get; set; }
        string name { get; set; }
        List<INode> nodes { get; }

        void AddNode(INode node);

        void AddRange(IEnumerable<INode> nodes);

    }
}
