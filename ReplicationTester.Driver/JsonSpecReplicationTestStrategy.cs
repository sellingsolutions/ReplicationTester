using Couchbase.Lite;
using Couchbase.Lite.Auth;
using ReplicationTester.DriverCore;
using ReplicationTester.DriverCore.Pull;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplicationTester.Driver
{
    internal sealed class JsonSpecReplicationTestStrategy : IPullReplicationTestStrategy
    {
        #region Builder

        public sealed class Builder
        {
            private string _name;

            private string _username;
            private string _password;

            private bool _continuous;
            private IEnumerable<string> _localDocIDs;

            public Builder WithName(string name)
            {
                this._name = name;
                return this;
            }

            public Builder UseCredentials(string username, string password)
            {
                this._username = username;
                this._password = password;

                return this;
            }

            public Builder ContinousReplication(bool continuous)
            {
                this._continuous = continuous;
                return this;
            }

            public Builder EnsureLocalDocuments(IEnumerable<string> docIDs)
            {
                this._localDocIDs = docIDs;
                return this;
            }

            public JsonSpecReplicationTestStrategy Build()
            {
                if (String.IsNullOrWhiteSpace(this._username))
                    throw new InvalidOperationException("Missing username");

                if (String.IsNullOrWhiteSpace(this._password))
                    throw new InvalidOperationException("Missing password");

                if (String.IsNullOrWhiteSpace(this._name))
                    throw new InvalidOperationException("Missing driver test name");

                return new JsonSpecReplicationTestStrategy
                {
                    _credentials = new Tuple<string, string>(this._username, this._password),
                    _localDocIDs = this._localDocIDs,
                    _continuous = this._continuous,
                    _name = this._name,
                };
            }
        }

        #endregion

        #region Fields

        private string _name;
        private bool _continuous;
        private IEnumerable<string> _localDocIDs;
        private Tuple<string, string> _credentials;

        #endregion

        private JsonSpecReplicationTestStrategy() { }

        public string Name
        {
            get { return this._name; }
        }

        public IAuthenticator CreateAuthenticator(ReplicationDriverContext context)
        {
            return AuthenticatorFactory.CreateBasicAuthenticator(
                username: this._credentials.Item1, 
                password: this._credentials.Item2
            );
        }

        public void DatabaseChanged(ReplicationDriverContext context, DatabaseChangeEventArgs eventArgs)
        {

        }

        public void ReplicationChanged(ReplicationDriverContext context, ReplicationChangeEventArgs eventArgs)
        {

        }

        public void ReplicationStarting(ReplicationDriverContext context)
        {
            context.Replication.Continuous = this._continuous;
        }
    }
}
