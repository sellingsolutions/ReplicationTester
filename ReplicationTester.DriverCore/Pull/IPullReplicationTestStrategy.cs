using Couchbase.Lite;
using Couchbase.Lite.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplicationTester.DriverCore.Pull
{
    public interface IPullReplicationTestStrategy
    {
        /// <summary>
        /// 
        /// </summary>

        string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>

        IAuthenticator CreateAuthenticator(ReplicationDriverContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="eventArgs"></param>

        void DatabaseChanged(ReplicationDriverContext context, DatabaseChangeEventArgs eventArgs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="eventArgs"></param>

        void ReplicationChanged(ReplicationDriverContext context, ReplicationChangeEventArgs eventArgs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>

        void ReplicationStarting(ReplicationDriverContext context);
    }
}
