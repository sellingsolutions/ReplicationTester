using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplicationTester.Driver
{
    internal class CommandLineArgs
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DriverOutputFilePath { get; set; }
        public string JsonDriverSpecPath { get; set; }
        public string DriverTestName { get; set; }
        public Uri RemoteUri { get; set; }
    }
}
