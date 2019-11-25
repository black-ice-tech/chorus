using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Chorus.DistributedLog.InMemory;
using Chorus.DistributedLog.TextFile;
using NUnit.Framework;

namespace Chorus.Specs.DistributedLog.TextFile
{
    [TestFixture]
    public class TextFileDistributedLogSpecs : BaseDistributedLogSpecs
    {
        private static readonly string TestFileDirectory = Directory.GetCurrentDirectory(); 
        [SetUp]
        public override void SetUp()
        {
            var options = new TextFileDistributedLogOptions {StreamDirectory = TestFileDirectory};

            _log = new TextFileDistributedLog(options);
        }

        [TearDown]
        public void TearDown()
        {
            var fileToDelete = Path.Combine(TestFileDirectory, $"{StreamName}.txt");

            if (File.Exists(fileToDelete))
            {
                File.Delete(fileToDelete);
            }
        }
    }
}
