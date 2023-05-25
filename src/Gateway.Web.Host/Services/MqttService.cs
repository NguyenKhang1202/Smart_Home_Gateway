using Gateway.Core.Settings;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;

namespace Gateway.Web.Host.Services
{
    public interface IMqttService
    {
        Task<IMqttClient> GetMqttClient();
        void PublishMqtt(string topic, string payload);
    }
    public class MqttService : IMqttService
    {
        private readonly MqttSettings _settings;
        public MqttService(IOptions<MqttSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<IMqttClient> GetMqttClient()
        {
            MqttFactory mqttFactory = new();
            IMqttClient mqttClient = mqttFactory.CreateMqttClient();

            MqttClientOptions options = new MqttClientOptionsBuilder()
                                .WithClientId(_settings.ClientId)
                                .WithTcpServer(_settings.Host, _settings.Port)
                                .WithCredentials(_settings.Username, _settings.Password)
                                .WithCleanSession()
                                .Build();

            await mqttClient.ConnectAsync(options);
            return await Task.FromResult(mqttClient);
        }

        public async void PublishMqtt(string topic, string payload)
        {
            IMqttClient _mqttClient = GetMqttClient().Result;
            MqttApplicationMessage message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithRetainFlag()
            .Build();
            await _mqttClient.PublishAsync(message);
        }
    }
}
