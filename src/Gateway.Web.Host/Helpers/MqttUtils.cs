using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Gateway.Core.Dtos.DataSensors;
using Gateway.Core.Dtos.Devices;
using Gateway.Core.Settings;
using Gateway.Web.Host.Protos.Devices;
using Gateway.Web.Host.Protos.Homes;
using Gateway.Web.Host.Protos.Notifications;
using Gateway.Web.Host.Protos.Rooms;
using Gateway.Web.Host.Protos.Users;
using Gateway.Web.Host.Services;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using Newtonsoft.Json;
using static Gateway.Application.Shared.Enums.DeviceEnum;

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
        private readonly IFirebaseClient _firebaseClient;
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
            _firebaseClient = new FireSharp.FirebaseClient(new FirebaseConfig
            {
                AuthSecret = "9J4DiNjy88ZJZA2WPqdFKng8HFXvvDCBFmVQ6ApZ",
                BasePath = "https://fire-alarm-system-c6a01-default-rtdb.firebaseio.com"
            });
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
            if (!mqttClient.IsConnected)
            {
                Console.WriteLine("Connect MQTT fail");
            }
            // subscribe
            await mqttClient.SubscribeAsync(TOPIC_DATA);
            await mqttClient.SubscribeAsync(TOPIC_GATEWAY_SUBSCRIBE);

            // receive message
            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                string payload = ConvertByteToString(e.ApplicationMessage.PayloadSegment);
                switch (e.ApplicationMessage.Topic.ToString())
                {
                    case var value when value == TOPIC_DATA:
                        var dataReceiveEsp32 = JsonConvert.DeserializeObject<List<MqttDataReceive>>(payload);
                        HandleMqttData(dataReceiveEsp32);
                        break;
                    case var value when value == TOPIC_GATEWAY_SUBSCRIBE:
                        var dataReceiveDusun = JsonConvert.DeserializeObject<MqttDataReceiveDusun>(payload);
                        HandleMqttDataDusun(dataReceiveDusun);
                        break;
                    default:
                        break;
                }
                Console.WriteLine(payload);
                return Task.CompletedTask;
            };
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
        private async Task<PDevice> GetDeviceByMacAddress(string deviceMacAddress)
        {
            GetDeviceByMacAddressResponse response = await _deviceGrpcClient.GetDeviceByMacAddressAsync(new GetDeviceByMacAddressRequest()
            {
                DeviceMacAddress = deviceMacAddress
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
                foreach (var data in datas)
                {
                    NotificationInfo notificationInfo = await GetNotificationInfo(data.DeviceCode);
                    PUser user = await GetUserAsync(notificationInfo.UserId);
                    List<int> listData = new();
                    switch (data.Type)
                    {
                        case (int)DEVICE_TYPE.FLAME:
                        case (int)DEVICE_TYPE.MQ2:
                            if (data.Value == 0)
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

                            // insert database firebase realtime
                            PushDataFirebase((int)data.Humidity, (int)data.Temperature);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async void HandleMqttDataDusun(MqttDataReceiveDusun data)
        {
            try
            {
                if (Equals(data.type, TYPE_REGISTER_REQ) && Equals(data.from, DEVICE_PUBLISH_GATEWAY))
                {
                    // register gateway
                    HandleRegisterGateway(data);
                }
                else if (Equals(data.type, TYPE_REPORT) && Equals(data.from, DEVICE_PUBLISH_GREENPOWER))
                {
                    // update state device
                    string gatewayMacAddress = data.mac;
                    string deviceMacAddress = data.data.mac;
                    int endPoint = (int)data.data.ep;
                    int value = (int)data.data.value.value;
                    UpdateStatusDeviceDusun(deviceMacAddress, value, endPoint);
                    PDevice pDevice = await GetDeviceByMacAddress(deviceMacAddress);
                    ControlDeviceResponse response = await _deviceGrpcClient.ControlDeviceAsync(new ControlDeviceRequest()
                    {
                        Id = pDevice.Id,
                        Control = new()
                        {
                            Direction = 0,
                            Intensity = 0,
                            Mode = 0,
                            Speed = 0,
                            Status = 0
                        },
                        ControlDusun = new()
                        {
                            EndPoint = endPoint,
                            Value = value
                        }
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // UPDATE to firebase
        private async void UpdateStatusDeviceDusun(string deviceMacAddress, int value, int endPoint)
        {
            try
            {
                StatusDeviceDusunDto statusDeviceDusunDto = new()
                {
                    value = value,
                };
                await _firebaseClient.UpdateAsync($"{PATH_STATUS_DEVICE_DUSUN}/{deviceMacAddress}/{endPoint}", statusDeviceDusunDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async void PushDataFirebase(int humidity, int temperature)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                DhtDataDto dhtDataDto = new()
                {
                    humidity = humidity,
                    temperature = temperature,
                    year = dateTime.Year,
                    month = dateTime.Month,
                    day = dateTime.Day,
                    hour = dateTime.Hour,
                    minute = dateTime.Minute
                };
                PushResponse response = await _firebaseClient.PushAsync($"{PATH_DHT_DATA}/", dhtDataDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void HandleRegisterGateway(MqttDataReceiveDusun data)
        {
            Console.WriteLine("Register gateway ...");
        }
    }
}
