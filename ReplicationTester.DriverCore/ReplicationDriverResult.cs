using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplicationTester.DriverCore
{
    public abstract class ReplicationDriverResult
    {
        /// <summary>
        /// The path to the sqlite database file that was written to when the driver ran
        /// </summary>

        public string DbDirectory { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string AttachmentStorePath { get; set; }

        /// <summary>
        /// Indicates for how long the replication ran
        /// </summary>

        public long ElapsedMilliseconds { get; set; }

        public string Username { get; set; }

        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<string> ErrorDetails { get; set; }
    }
}
