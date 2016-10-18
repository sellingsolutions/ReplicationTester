using Couchbase.Lite;
using Couchbase.Lite.Auth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReplicationTester.DriverCore.Pull
{
    public sealed class PullReplicationDriver
    {
        #region Fields

        private readonly AutoResetEvent _pullReplicationWaitHandle = new AutoResetEvent(initialState: false);
        private readonly string _driverID = "pull_" + Guid.NewGuid().ToString().ToLower();
        private readonly Manager _manager = Manager.SharedInstance;
        private readonly PullReplicationDriverOptions _options;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>

        public PullReplicationDriver(PullReplicationDriverOptions options)
        {
            if (options == null)
                throw new ArgumentNullException("options");

            this._options = options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testStrategy"></param>
        /// <returns></returns>

        public PullReplicationDriverResult Start(IPullReplicationTestStrategy testStrategy)
        {
            if (testStrategy == null)
                throw new ArgumentNullException("testStrategy");

            ReplicationDriverContext context = null;
            try { context = InternalStart(testStrategy); }
            catch(Exception ex)
            {
                return new PullReplicationDriverResult
                {
                    ErrorMessage = ex.Message,
                    Error = true
                };
            }

            return PullReplicationDriverResult.Create(context, testStrategy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testStrategy"></param>
        /// <returns></returns>

        private ReplicationDriverContext InternalStart(IPullReplicationTestStrategy testStrategy)
        {
            // Construct a new database using the current driver id
            using (Database database = this._manager.GetDatabase(this._driverID))
            {
                // Assemble the driver context
                var pullReplication = database.CreatePullReplication(this._options.RemoteReplicationUri);
                var context = new ReplicationDriverContext.Builder()
                    .WithReplication(pullReplication)
                    .WithDatabase(database)
                    .Build();

                pullReplication.Authenticator = testStrategy.CreateAuthenticator(context);
                testStrategy.ReplicationStarting(context);

                database.Changed += (sender, e) =>
                {
                    testStrategy.DatabaseChanged(context, e);
                };

                pullReplication.Changed += (sender, e) =>
                {
                    testStrategy.ReplicationChanged(context, e);

                    switch (e.Status)
                    {
                        default:
                        case ReplicationStatus.Active:
                            // no-op
                            break;
                        case ReplicationStatus.Idle:
                        case ReplicationStatus.Offline:
                        case ReplicationStatus.Stopped:

                            this._pullReplicationWaitHandle.Set();

                            break;
                    }

                };

                context.ReplicationStopwatch.Start();
                pullReplication.Start();

                this._pullReplicationWaitHandle.WaitOne();

                pullReplication.Stop();
                context.ReplicationStopwatch.Stop();

                return context;
            }
        }
    }
}
