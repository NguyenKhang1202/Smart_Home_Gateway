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
using Gateway.Web.Host.Protos.Homes;
using Gateway.Web.Host.Protos.Rooms;

namespace Gateway.Web.Host.Helpers
{
    public class MqttUtils
    {
        private readonly IConfiguration _configuration;
        private readonly DeviceGrpc.DeviceGrpcClient _deviceGrpcClient;
        private readonly NotificationGrpc.NotificationGrpcClient _notificationGrpcClient;
        private readonly UserGrpc.UserGrpcClient _userGrpcClient;
        private readonly HomeGrpc.HomeGrpcClient _homeGrpcClient;
        private readonly RoomGrpc.RoomGrpcClient _roomGrpcClient;
        private readonly IFirebaseService _firebaseService;
        public MqttUtils(IConfiguration configuration,
            UserGrpc.UserGrpcClient userGrpcClient,
            IFirebaseService firebaseService,
            DeviceGrpc.DeviceGrpcClient deviceGrpcClient,
            HomeGrpc.HomeGrpcClient homeGrpcClient,
            RoomGrpc.RoomGrpcClient roomGrpcClient,
            NotificationGrpc.NotificationGrpcClient notificationGrpcClient) 
        {
            _configuration = configuration;
            _firebaseService = firebaseService;
            _deviceGrpcClient = deviceGrpcClient;
            _homeGrpcClient = homeGrpcClient;
            _roomGrpcClient = roomGrpcClient;
            _userGrpcClient = userGrpcClient;
            _notificationGrpcClient = notificationGrpcClient;
        }

        public async void SubscribeAndHandleMessage()
        {
            MqttFactory mqttFactory = new();
            List<MqttDataReceive>? data = new();
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
                await mqttClient.SubscribeAsync(TOPIC_DATA);

                // receive message
                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    string payload = ConvertByteToString(e.ApplicationMessage.PayloadSegment);
                    data = JsonConvert.DeserializeObject<List<MqttDataReceive>>(payload);

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
        private async Task<NotificationInfo> GetNotificationInfo(string deviceCode)
        {
            GetDeviceByCodeResponse response = await _deviceGrpcClient.GetDeviceByCodeAsync(new GetDeviceByCodeRequest()
            {
                DeviceCode = deviceCode
            });
            GetHomeByIdResponse response1 = await _homeGrpcClient.GetHomeByIdAsync(new GetHomeByIdRequest()
            {
                Id = response.Data.HomeDeviceId,
            });
            GetRoomByIdResponse response2 = await _roomGrpcClient.GetRoomByIdAsync(new GetRoomByIdRequest()
            {
                Id = response.Data.RoomId,
            });

            return new NotificationInfo()
            {
                HomeName = response1.Data.Name,
                RoomName = response2.Data.Name,
                TenantId = response.Data.TenantId,
                UserId = response.Data.UserId,
            };
        }
        public class NotificationInfo
        {
            public string HomeName { get; set; }
            public string RoomName { get; set; }
            public string UserId { get; set; }
            public string TenantId { get; set; }
        }
        private async Task<PDevice> GetDeviceByCode(string deviceCode)
        {
            GetDeviceByCodeResponse response = await _deviceGrpcClient.GetDeviceByCodeAsync(new GetDeviceByCodeRequest()
            {
                DeviceCode = deviceCode
            });
            return response.Data;
        }
        private async Task<PUser> GetUserAsync(string UserId)
        {
            GetUserByIdResponse response = await _userGrpcClient.GetUserByIdAsync(new GetUserByIdRequest()
            {
                Id = UserId
            });
            return response.Data;
        }
        private async void HandleMqttData(List<MqttDataReceive> datas)
        {
            try
            {
                foreach(var data in datas)
                {
                    NotificationInfo notificationInfo = await GetNotificationInfo(data.DeviceCode);
                    PUser user = await GetUserAsync(notificationInfo.UserId);
                    List<int> listData = new();
                    switch (data.Type)
                    {
                        case (int)DEVICE_TYPE.FLAME:
                        case (int)DEVICE_TYPE.MQ2:
                            if(data.Value == 0)
                            {
                                CreateNotificationRequest request = new()
                                {
                                    UserId = notificationInfo.UserId,
                                    Content = "Phát hiện khói tại \"" + notificationInfo.RoomName
                                    + ", " + notificationInfo.HomeName + "\"",
                                    Title = "Cảnh báo",
                                    TenantId = notificationInfo.TenantId,
                                };
                                // thêm thông báo vào DB
                                await _notificationGrpcClient.CreateNotificationAsync(request);

                                // pushs thông báo tới firebase
                                await _firebaseService.SendNotification(user.FCMToken[0], request.Title, request.Content);

                                // update data của device
                                listData.Add((int)data.Value!);
                                await _deviceGrpcClient.UpdateDataDeviceAsync(new UpdateDataDeviceRequest()
                                {
                                    DeviceCode = data.DeviceCode,
                                    Data = { listData }
                                });
                            }
                            break;
                        case (int)DEVICE_TYPE.DHT:
                            listData.Add((int)data.Humidity!);
                            listData.Add((int)data.Temperature!);
                            await _deviceGrpcClient.UpdateDataDeviceAsync(new UpdateDataDeviceRequest()
                            {
                                DeviceCode = data.DeviceCode,
                                Data = { listData }
                            });
                            break;
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
