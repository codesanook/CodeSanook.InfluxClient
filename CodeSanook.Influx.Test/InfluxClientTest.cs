using Xunit;

namespace CodeSanook.Influx.Test
{
    public class InfluxClientTest
    {
        public InfluxClientTest()
        {
        }

        [Fact]
        public void Query_ValidQuery_GetResult()
        {
            var client = new InfluxClient(@"D:\projects\CodeSanook.InfluxClient\config.yaml");
            client.Query("");
        }
    }
}
