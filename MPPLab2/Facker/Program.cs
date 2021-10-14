using System;
using Facker;

namespace Facker
{
    class Program
    {
        private class TestClass1
        {
            private int a;
            private ulong b;
            private bool[] t;
            
            public TestClass1(int a, ulong b, bool t)
            {
                this.a = a;
                this.b = b;
                this.t = new bool[10];
            }
        }
        static void Main(string[] args)
        {
            var facker = new CustomFacker("D:\\C++\\MPPLabs\\MPPLabs\\MPPLab2\\Facker\\bin\\Debug\\netcoreapp3.1\\");
            TestClass1 test = facker.Create<TestClass1>();
            String str = facker.Create<String>();
            TestClass1[][] arr = facker.Create<TestClass1[][]>();
        }
    }
}
