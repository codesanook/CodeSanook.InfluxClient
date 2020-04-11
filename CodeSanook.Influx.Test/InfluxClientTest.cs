using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace CodeSanook.Influx.Test
{
    public class InfluxClientTest
    {
        private readonly InfluxClient client;

        public InfluxClientTest()
        {
            var executedPath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            var executedDirectory = Path.GetDirectoryName(executedPath);
            var directoryInfo = new DirectoryInfo(executedDirectory);
            var rootPath = directoryInfo.Parent.Parent.Parent.FullName;
            client = new InfluxClient(Path.Combine(rootPath, "config.yml"));
        }

        [Fact]
        public void Query_ValidQuery_GetResult()
        {
            var result = client.Query("select * from bookingMetrics where time > now() - 12h");
        }
    }
}
