using NDesk.Options;
using Newtonsoft.Json;
using ReplicationTester.DriverCore.Pull;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplicationTester.Driver
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmdArgs = new CommandLineArgs();
            var optionSet = new OptionSet
                {
                    { "output=", "", path => cmdArgs.DriverOutputFilePath = path },
                    { "remote=", "", uri => cmdArgs.RemoteUri = new Uri(uri) },
                    { "spec=", "", path => cmdArgs.JsonDriverSpecPath = path },
                    { "name=", "", name => cmdArgs.DriverTestName = name },
                    { "username=", "", username => cmdArgs.Username = username },
                    { "password=", "", password => cmdArgs.Password = password }
                }
                .Parse(args);

            var testStrategy = new JsonSpecReplicationTestStrategy.Builder()
                .UseCredentials(cmdArgs.Username, cmdArgs.Password)
                .WithName(cmdArgs.DriverTestName)
                .ContinousReplication(true)
                .Build();

            var options = new PullReplicationDriverOptions(remoteUri: cmdArgs.RemoteUri);
            var driver = new PullReplicationDriver(options);

            var result = driver.Start(testStrategy);

            File.WriteAllText(
                cmdArgs.DriverOutputFilePath,
                JsonConvert.SerializeObject(
                    new
                    {
                        elapsedMs = result.ElapsedMilliseconds,

                        error = result.Error,
                        errorMessage = result.ErrorMessage,
                        errorDetails = result.ErrorDetails,

                        name = result.TestStrategy.Name,

                        dbDirectory = result.DbDirectory,
                        attachmentStorePath = result.AttachmentStorePath
                    },
                    Formatting.Indented
                )
            );
        }
    }
}
