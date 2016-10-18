using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplicationTester.Driver
{
    internal sealed class JsonReplicationSpec
    {
        public IEnumerable<JObject> LocalDocumentIDs { get; set; }
        public IEnumerable<string> RemoteDocumentIDs { get; set; }
        public bool Continuous { get; set; }
    }
}
