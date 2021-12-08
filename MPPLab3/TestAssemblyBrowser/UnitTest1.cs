using Microsoft.VisualStudio.TestTools.UnitTesting;
using AssemblyBrowserLibrary;
using System.Collections.Generic;
using Moq;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private const string PathToDll = "D:\\C++\\MPPLabs\\MPPLab3\\AssemblyBrowserLibrary\\bin\\Debug\\AssemblyBrowserLibrary.dll";
        private readonly List<INode> _nodes = AssemblyBrowser.GetAssemblyInfo(PathToDll);

        [TestMethod]
        public void TestNamespaces()
        {
            Assert.AreEqual(2, _nodes.Count);
            Assert.AreEqual("AssemblyBrowserLibrary", _nodes[0].name);
            Assert.AreEqual("AssemblyBrowserLibrary.Extensions", _nodes[1].name);
        }

        [TestMethod]
        public void TestClassType()
        {
            var type = _nodes[0].nodes[0];
            Assert.AreEqual("[type]", type.nodeType);
            Assert.AreEqual("public", type.accessModifier);
            Assert.AreEqual("class", type.classType);
        }

        [TestMethod]
        public void TestConstructors()
        {
            var constructor = _nodes[0].nodes[2].nodes[2];
            Assert.AreEqual("[constructor]", constructor.nodeType);
        }

        [TestMethod]
        public void TestProperty()
        {
            var property = _nodes[0].nodes[2].nodes[4];
            Assert.AreEqual("[property]", property.nodeType);
        }

        [TestMethod]
        public void TestPropertyAccessors()
        {
            var property = _nodes[0].nodes[2].nodes[3];
            var accessors = property.nodes;
            Assert.AreEqual("[accessor]", accessors[0].nodeType);
            Assert.AreEqual("get_optional", accessors[0].name);
            Assert.AreEqual("[accessor]", accessors[1].nodeType);
            Assert.AreEqual("set_optional", accessors[1].name);
        }

        [TestMethod]
        public void TestAssemblyExtensionMethods()
        {
            var extensionMethod1 = _nodes[0].nodes[2].nodes[_nodes[0].nodes[2].nodes.Count - 1];
            Assert.AreEqual("[extension]", extensionMethod1.optional);
            Assert.AreEqual("AddRange", extensionMethod1.name);
        }
        [TestMethod]
        public void TestLoggerDirectoryByName() {

            Mock<Logger.ILogger> mock = new Mock<Logger.ILogger>();
            mock.Setup(l => l.GetDirectoryByLoggerName(It.IsAny<string>()))
                .Returns<string>(name => "C:\\" + name);

            string loggerName = "LoggerName";

            Logger.ILogger logger = mock.Object;

            string loggerDirectory = logger.GetDirectoryByLoggerName(loggerName);

            Assert.AreEqual("C:\\LoggerName", loggerDirectory);

        }

        [TestMethod]
        public void TestLoggerGetCurrentDirectory()
        {

            Mock<Logger.ILogger> mock = new Mock<Logger.ILogger>();
            mock.Setup(l => l.GetCurrentDirectory())
                .Returns("C:\\");

            string trueloggerDirectory = "C:\\";

            Logger.ILogger logger = mock.Object;

            string loggerDirectory = logger.GetCurrentDirectory();

            Assert.AreEqual(trueloggerDirectory, loggerDirectory);

        }
    }
}


