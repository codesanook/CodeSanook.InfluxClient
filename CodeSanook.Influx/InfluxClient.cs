using EdgeJs;
using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CodeSanook.Influx
{
    public class InfluxClient
    {
        private Option option;

        public InfluxClient(string configPath)
        {
            Console.WriteLine($"loading config from {configPath}");
            this.option = GetOption(configPath);
            Console.WriteLine($"option host {option.Host}");
        }

        private Option GetOption(string configPath)
        {
            using (var streamReader = new StreamReader(configPath))
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(new CamelCaseNamingConvention())
                    .Build();
                return deserializer.Deserialize<Option>(streamReader.ReadToEnd());
            }
        }

        public dynamic Query(string query)
        {
            var func = Edge.Func(@"
                var influx = require('influx');

                return function (options, callback) {
                    var client = influx({
                        host    :   options.host,
                        port    :   options.port, 
                        protocol:   options.protocol,
                        username:   options.username,
                        password:   options.password,
                        database:   options.database
                    });

                    client.query(options.query, function (error, response){
                        callback(error, response);
                    });
                };
            ");

            var options = new
            {
                option.Host,
                option.Port,
                option.Protocol,
                option.Username,
                option.Password,
                option.Database,
                query
            };

            return func(options).Result;
            //dynamic data = response[0];
            //var name = data.name;
            ////Console.WriteLine(JsonConvert.SerializeObject(, Formatting.Indented));
            //foreach (dynamic row in data.points)
            //{
            //    foreach (dynamic column in row)
            //    {
            //        Console.Write($"{column}");
            //    }
            //    Console.WriteLine();
            //}
        }
    }
}
