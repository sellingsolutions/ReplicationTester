using Couchbase.Lite;
using Couchbase.Lite.Auth;
using ReplicationTester.DriverCore;
using ReplicationTester.DriverCore.Pull;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplicationTester.Runner
{
    internal sealed class DefaultPullReplicationTestStrategy : IPullReplicationTestStrategy
    {
        private readonly string _username;
        private readonly string _password;

        public DefaultPullReplicationTestStrategy(string username, string password)
        {
            this._username = username;
            this._password = password;
        }

        public IAuthenticator CreateAuthenticator(ReplicationDriverContext context)
        {
            return AuthenticatorFactory.CreateBasicAuthenticator(this._username, this._password);
        }

        public void ReplicationStarting(ReplicationDriverContext context)
        {
            context.Replication.Continuous = true;
            context.Replication.ReplicationOptions = new ReplicationOptions
            {
                UseWebSocket = false
            };
        }

        public void ReplicationChanged(ReplicationDriverContext context, ReplicationChangeEventArgs eventArgs)
        {
            //if(eventArgs.LastError != null)
            //    context.Abort(eventArgs.LastError.Message, eventArgs.LastError.ToString());


        }

        public void DatabaseChanged(ReplicationDriverContext context, DatabaseChangeEventArgs eventArgs)
        {

        }

        public string Name
        {
            get { return "proof_of_concept"; }
        }
    }
}
