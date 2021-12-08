﻿using System;
using System.Collections.Generic;
using System.Linq;
using TestsGenerator.IO;

namespace TestsGenerator.UsageExample
{
    class Program
    {
        static void Main(string[] args)
        {
            TestsGeneratorConfig config = new TestsGeneratorConfig
            {
                ReadPaths = new List<string> { "../../SimpleTestFile.cs", "../../ExtendedTestFile.cs" },
                Writer = new AsyncFileWriter() { Directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) },
                ReadThreadCount = 2,
                WriteThreadCount = 2,
                processThreadCount = 2
            };
            new TestsGenerator(config).Generate().Wait();
            Console.WriteLine("Generation completed");
            Console.ReadKey();

        }
    }
}
