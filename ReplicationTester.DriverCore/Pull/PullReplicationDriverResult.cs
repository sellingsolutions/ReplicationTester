using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplicationTester.DriverCore.Pull
{
    public sealed class PullReplicationDriverResult : ReplicationDriverResult
    {
        /// <summary>
        /// 
        /// </summary>

        public IPullReplicationTestStrategy TestStrategy { get; set; }

        public static PullReplicationDriverResult Create(ReplicationDriverContext context, IPullReplicationTestStrategy strategy)
        {
            return new PullReplicationDriverResult
            {
                TestStrategy = strategy,
                ElapsedMilliseconds = context.ReplicationStopwatch.ElapsedMilliseconds,

                // Error info
                Error = context.Aborted,
                ErrorMessage = context.ErrorMessage,
                ErrorDetails = context.ErrorDetails,

                // Database file
                DbDirectory = context.DbDirectory,
                AttachmentStorePath = context.AttachmentStorePath,

                Username = context.Replication.Username
            };
        }
    }
}
