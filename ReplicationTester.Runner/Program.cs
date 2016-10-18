using ReplicationTester.DriverCore.Pull;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplicationTester.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new PullReplicationDriverOptions(new Uri("http://localhost:4985")); // Walrus
            var testStrategy = new DefaultPullReplicationTestStrategy(username: "test", password: "solarwind11");

            int amountOfDrivers = 1;

            Console.WriteLine(String.Format("Starting {0} pull replication drivers..", amountOfDrivers));

            var metricsTask = Task.WhenAll(
                Enumerable.Range(0, amountOfDrivers)
                    .Select(_ => new PullReplicationDriver(options))
                    .Select(driver =>
                        Task.Run(() =>
                            driver.Start(testStrategy)
                        )
                    )
                    .ToArray()
            );

            double avg = metricsTask.Result.Sum(metric => metric.ElapsedMilliseconds) / metricsTask.Result.Count();

            foreach(var metric in metricsTask.Result)
            {
                Console.WriteLine(metric.ErrorMessage);
            }

            Console.WriteLine(String.Format("Pull replication average: {0}", avg));
        }
    }
}
