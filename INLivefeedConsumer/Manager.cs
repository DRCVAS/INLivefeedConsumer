using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace INLiveFeecd
{
    public static class Manager
    {
        static int tps = 0; static int l_tps = 0;
        static int tps_01 = 0; static int l_tps_01 = 0;

        public static int MAXTPS = 0;
        public static void Rtps()
        {
            while (true)
            {
                l_tps = tps;
                tps = 0;

                l_tps_01 = tps_01;
                tps_01 = 0;
                if (l_tps != 0 || l_tps_01 != 0)
                    Console.WriteLine("TPS: " + l_tps + "     :::     ERRORS: " + l_tps_01);
                if (l_tps > MAXTPS)
                    MAXTPS = l_tps;
                Thread.Sleep(1000);
            }
        }


        //private static IProducer<Null, string> KafkaProducer;
        //private static string bootstrapServers = "10.100.11.24:9092";

        //public static void InitKafka()
        //{
        //    Task.Run(() => Rtps());
        //    var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        //    KafkaProducer = new ProducerBuilder<Null, string>(config).Build();
        //    KafkaProducer.AddBrokers("10.100.11.24:9092");
        //    ProduceOne("test", "safesdf").Wait();
        //}

        //public static async Task CreateTopicAsync(string topicName, int partitions = 10, short replicationFactor = 1)
        //{
        //    using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build())
        //    {
        //        try
        //        {
        //            await adminClient.CreateTopicsAsync(new TopicSpecification[] {
        //                new TopicSpecification { Name = topicName, ReplicationFactor = replicationFactor, NumPartitions = partitions } });
        //        }
        //        catch (CreateTopicsException e)
        //        {
        //            Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
        //        }
        //    }
        //}

        //public static async Task<bool> ProduceOne(string topic, string message)
        //{
        //    try
        //    {
        //        var dr = await KafkaProducer.ProduceAsync(topic, new Message<Null, string> { Value = message });
        //        Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
        //        tps++;
        //        return true;
        //    }
        //    catch (ProduceException<Null, string> e)
        //    {
        //        Console.WriteLine($"Delivery failed: {e.Error.Reason}");
        //        tps_01++;
        //        return false;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        tps_01++;
        //        return false;
        //    }
        //}
    }
}
