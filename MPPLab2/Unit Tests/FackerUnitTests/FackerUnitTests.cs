using Microsoft.VisualStudio.TestTools.UnitTesting;
using Facker;
namespace FackerUnitTests
{
    [TestClass]
    public class TestBaseTypes
    {
        [TestMethod]
        public void TestBool()
        {
            var facker = new CustomFacker();
            Assert.IsTrue(facker.Create<bool>().GetType() == typeof(bool));
        }
    }
    [TestClass]
    public class TestArray
    {
        [TestMethod]
        public void TestArrayOfBase()
        {
            var facker = new CustomFacker();
            Assert.IsTrue(facker.Create<bool[]>().GetType().IsArray);
        }
    }
}
