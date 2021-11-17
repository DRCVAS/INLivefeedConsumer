using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using INLiveFeecd.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace INLivefeedConsumer
{
    public class Consumer
    {
       
        public static void StartConsumer(string groupid, string topic,string BootstrapServers)
        {
            var conf = new ConsumerConfig
            {
                GroupId = groupid,

                BootstrapServers = BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Latest
               
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf)
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    Console.WriteLine($"Assigned partitions: [{string.Join(", ", partitions)}]");
                    return partitions.Select(tp => new TopicPartitionOffset(tp, Offset.Unset));
                })
                .SetPartitionsRevokedHandler((c, partitions) =>
                {
                    Console.WriteLine($"Revoking assignment: [{string.Join(", ", partitions)}]");
                })
                .Build())
            {
                c.Subscribe(topic);
                try
                {
                    while (true)
                    {
                        try
                        {
                            var URL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["URL"];
                            var SqlConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["SqlConnection"];
                         
                            var cr = c.Consume();
                            clsFunction clsFunc = new clsFunction();
                            if (URL != "") {
                                clsFunc.CallAPI(cr.Message.Value, URL);
                            }
                            if (SqlConnection != "")
                            {
                                clsFunc.CallSQlSP(cr.Message.Value, SqlConnection);
                            }

                            Console.WriteLine($"{groupid} ::: Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                            //deserialize xml and do whatever is needed here with cr.Message.Value//
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    c.Close();
                }
            }
        }
    }
}
