using Microsoft.VisualStudio.TestTools.UnitTesting;
using AssemblyBrowserLib;
using System.Collections.Generic;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private const string PathToDll = "C:\\Users\\ilyuh\\Desktop\\Main Projects For Study\\SPP3\\AssemblyBrowserLib\\bin\\Debug\\net5.0\\AssemblyBrowserLib.dll";
        private readonly List<INode> _nodes = AssemblyBrowser.GetAssemblyInfo(PathToDll);

        [TestMethod]
        public void TestNamespaces()
        {
            Assert.AreEqual(2, _nodes.Count);
            Assert.AreEqual("AssemblyBrowserLib", _nodes[0].Name);
            Assert.AreEqual("AssemblyBrowserLib.Extensions", _nodes[1].Name);
        }

        [TestMethod]
        public void TestClassType()
        {
            var type = _nodes[0].Nodes[0];
            Assert.AreEqual("[type]", type.NodeType);
            Assert.AreEqual("public", type.AccessModifier);
            Assert.AreEqual("class", type.ClassType);
        }

        [TestMethod]
        public void TestConstructors()
        {
            var constructor = _nodes[0].Nodes[2].Nodes[1];
            Assert.AreEqual("[constructor]", constructor.NodeType);
        }

        [TestMethod]
        public void TestProperty()
        {
            var property = _nodes[0].Nodes[2].Nodes[4];
            Assert.AreEqual("[property]", property.NodeType);
        }

        [TestMethod]
        public void TestPropertyAccessors()
        {
            var property = _nodes[0].Nodes[2].Nodes[2];
            var accessors = property.Nodes;
            Assert.AreEqual("[accessor]", accessors[0].NodeType);
            Assert.AreEqual("get_Optional", accessors[0].Name);
            Assert.AreEqual("[accessor]", accessors[1].NodeType);
            Assert.AreEqual("set_Optional", accessors[1].Name);
        }

        [TestMethod]
        public void TestAssemblyExtensionMethods()
        {
            var extensionMethod1 = _nodes[0].Nodes[2].Nodes[^1];
            Assert.AreEqual("[extension]", extensionMethod1.Optional);
            Assert.AreEqual("AddRange", extensionMethod1.Name);
        }
    }
}


