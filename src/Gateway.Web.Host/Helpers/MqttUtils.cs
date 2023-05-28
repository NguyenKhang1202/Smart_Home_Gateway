using MQTTnet.Client;
using MQTTnet;
using MQTTnet.Server;

namespace Gateway.Web.Host.Helpers
{
    public class MqttUtils
    {
        private readonly IConfiguration _configuration;
        private readonly static string SubscribeChannel = "projects/smart_home/data";
        public MqttUtils(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public async void SubscribeAndHandleMessage()
        {
            MqttFactory mqttFactory = new();
            IMqttClient mqttClient = mqttFactory.CreateMqttClient();

            MqttClientOptions options = new MqttClientOptionsBuilder()
                                .WithClientId(_configuration["MqttSettings:ClientId"])
                                .WithTcpServer(_configuration["MqttSettings:Host"], int.Parse(_configuration["MqttSettings:Port"]))
                                .WithCredentials(_configuration["MqttSettings:Username"], _configuration["MqttSettings:Password"])
                                .WithCleanSession()
                                .Build();

            await mqttClient.ConnectAsync(options);
            if (mqttClient.IsConnected)
            {
                // subscribe
                await mqttClient.SubscribeAsync(SubscribeChannel);

                // receive message
                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    Console.WriteLine("Received application message.");
                    String str = ConvertByteToString(e.ApplicationMessage.PayloadSegment);
                    
                    Console.WriteLine(str);
                    return Task.CompletedTask;
                };
            }
            else
            {
                Console.WriteLine("Connect MQTT fail");
            }
        }

        private static string ConvertByteToString(System.ArraySegment<byte> bytes)
        {
            return System.Text.Encoding.Default.GetString(bytes);
        }
    }
}
