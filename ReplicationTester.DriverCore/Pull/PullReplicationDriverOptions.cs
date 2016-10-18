using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplicationTester.DriverCore.Pull
{
    public sealed class PullReplicationDriverOptions
    {
        public PullReplicationDriverOptions(Uri remoteUri)
        {
            if (remoteUri == null)
                throw new ArgumentNullException("remoteUri");

            this.RemoteReplicationUri = remoteUri;
        }

        /// <summary>
        /// The remote replication endpoint that should be used when starting the pull replication
        /// </summary>

        public Uri RemoteReplicationUri { get; private set; }
    }
}
