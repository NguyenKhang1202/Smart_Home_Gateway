using MQTTnet.Client;
using MQTTnet;
using MQTTnet.Server;
using Newtonsoft.Json;
using Gateway.Web.Host.Protos.Devices;
using Gateway.Core.Settings;

namespace Gateway.Web.Host.Helpers
{
    public class MqttUtils
    {
        private readonly IConfiguration _configuration;
        private readonly static string SubscribeChannel = "projects/smart_home/data";
        private readonly DeviceGrpc.DeviceGrpcClient _deviceGrpcClient;
        public MqttUtils(IConfiguration configuration,
            DeviceGrpc.DeviceGrpcClient deviceGrpcClient) 
        {
            _configuration = configuration;
            _deviceGrpcClient = deviceGrpcClient;
        }

        public async void SubscribeAndHandleMessage()
        {
            MqttFactory mqttFactory = new();
            MqttDataReceive? data = new();
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
                    string payload = ConvertByteToString(e.ApplicationMessage.PayloadSegment);
                    data = JsonConvert.DeserializeObject<MqttDataReceive>(payload);

                    Console.WriteLine(payload);
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
