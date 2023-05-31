using MQTTnet.Client;
using MQTTnet;
using MQTTnet.Server;
using Newtonsoft.Json;
using Gateway.Web.Host.Protos.Devices;
using Gateway.Core.Settings;
using Gateway.Web.Host.Protos.Notifications;
using static Gateway.Application.Shared.Enums.DeviceEnum;
using Gateway.Web.Host.Services;
using Gateway.Web.Host.Protos.Users;
using static Gateway.Web.Host.Protos.Users.UserGrpc;

namespace Gateway.Web.Host.Helpers
{
    public class MqttUtils
    {
        private readonly IConfiguration _configuration;
        private readonly static string SubscribeChannel = "projects/smart_home/data";
        private readonly DeviceGrpc.DeviceGrpcClient _deviceGrpcClient;
        private readonly NotificationGrpc.NotificationGrpcClient _notificationGrpcClient;
        private readonly UserGrpc.UserGrpcClient _userGrpcClient;
        private readonly IFirebaseService _firebaseService;
        public MqttUtils(IConfiguration configuration,
            UserGrpc.UserGrpcClient userGrpcClient,
            IFirebaseService firebaseService,
            DeviceGrpc.DeviceGrpcClient deviceGrpcClient,
            NotificationGrpc.NotificationGrpcClient notificationGrpcClient) 
        {
            _configuration = configuration;
            _firebaseService = firebaseService;
            _deviceGrpcClient = deviceGrpcClient;
            _userGrpcClient = userGrpcClient;
            _notificationGrpcClient = notificationGrpcClient;
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

                    HandleMqttData(data);
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
        private async Task<string> GetUserId(string deviceCode)
        {
            GetDeviceByCodeResponse response = await _deviceGrpcClient.GetDeviceByCodeAsync(new GetDeviceByCodeRequest()
            {
                DeviceCode = deviceCode
            });
            return response.Data.UserId;
        }
        private async Task<PUser> GetUserAsync(string UserId)
        {
            GetUserByIdResponse response = await _userGrpcClient.GetUserByIdAsync(new GetUserByIdRequest()
            {
                Id = UserId
            });
            return response.Data;
        }
        private async void HandleMqttData(MqttDataReceive data)
        {
            try
            {
                string userId = await GetUserId(data.Code);
                PUser user = await GetUserAsync(userId);
                List<int> listData = new();
                switch (data.Type)
                {
                    case (int)DEVICE_TYPE.FLAME:
                    case (int)DEVICE_TYPE.MQ2:
                        
                        CreateNotificationRequest request = new()
                        {
                            UserId = userId,
                            Content = "Nội dung chi tiết thông báo",
                            Title = "Phát hiện khói",
                            TenantId = "tenant smart home"
                        };
                        // thêm thông báo vào DB
                        await _notificationGrpcClient.CreateNotificationAsync(request);

                        // pushs thông báo tới firebase
                        await _firebaseService.SendNotification(user.FCMToken[0], "Phát hiện khói", "Nội dung chi tiết thông báo");

                        // update data của device
                        listData.Add((int)data.Value!);
                        await _deviceGrpcClient.UpdateDataDeviceAsync(new UpdateDataDeviceRequest()
                        {
                            DeviceCode = data.Code,
                            Data = { listData }
                        });
                        break;
                    case (int)DEVICE_TYPE.DHT:
                        listData.Add((int)data.Humidity!);
                        listData.Add((int)data.Temperature!);
                        await _deviceGrpcClient.UpdateDataDeviceAsync(new UpdateDataDeviceRequest()
                        {
                            DeviceCode = data.Code,
                            Data = { listData }
                        });
                        break;
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
