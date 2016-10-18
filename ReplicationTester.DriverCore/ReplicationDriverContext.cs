using Couchbase.Lite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReplicationTester.DriverCore
{
    public sealed class ReplicationDriverContext
    {
        #region Builder

        public sealed class Builder
        {
            private Replication _replication;
            private Database _database;
            private string _attachmentStorePath;
            private string _dbDirectory;

            public Builder WithDatabase(Database database)
            {
                this._database = database;

                var dbType = this._database.GetType();

                // Find out where the actual database file resides.
                // Unfortunately, this information is not available via the public API..

                BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

                this._attachmentStorePath = (string)dbType.GetProperty("AttachmentStorePath", bindingFlags).GetValue(this._database);
                this._dbDirectory = (string)dbType.GetProperty("DbDirectory", bindingFlags).GetValue(this._database);

                return this;
            }

            public Builder WithReplication(Replication replication)
            {
                this._replication = replication;
                return this;
            }

            public ReplicationDriverContext Build()
            {
                if (this._database == null)
                    throw new InvalidOperationException("A database instance must configured");

                if (this._replication == null)
                    throw new InvalidOperationException("A replication instance must configured");

                return new ReplicationDriverContext
                {
                    Database = this._database,
                    DbDirectory = this._dbDirectory,
                    AttachmentStorePath = this._attachmentStorePath,

                    ReplicationStopwatch = new Stopwatch(),
                    Replication = this._replication
                };
            }
        }

        #endregion

        private ReplicationDriverContext() { }

        /// <summary>
        /// 
        /// </summary>

        public Stopwatch ReplicationStopwatch { get; private set; }

        /// <summary>
        /// 
        /// </summary>

        public Replication Replication { get; private set; }

        /// <summary>
        /// 
        /// </summary>

        public Database Database { get; private set; }

        public string DbDirectory { get; private set; }
        public string AttachmentStorePath { get; private set; }

        /// <summary>
        /// 
        /// </summary>

        public bool Aborted { get; private set; }

        /// <summary>
        /// 
        /// </summary>

        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 
        /// </summary>

        public IEnumerable<string> ErrorDetails { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="errorDetails"></param>

        public void Abort(string errorMessage, params string[] errorDetails)
        {
            this.Aborted = true;
            this.ErrorMessage = errorMessage ?? "";
            this.ErrorDetails = errorDetails ?? new string[0];

            this.ReplicationStopwatch.Stop();
            this.Replication.Stop();
        }
    }
}
