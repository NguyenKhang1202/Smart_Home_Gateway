using AutoMapper;
using Gateway.Core.Dtos;
using Gateway.Core.Dtos.Devices;
using Gateway.Core.Entities;
using Gateway.Core.Settings;
using Gateway.Web.Host.Helpers;
using Gateway.Web.Host.Protos.Devices;
using Gateway.Web.Host.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Gateway.Application.Shared.Enums.DeviceEnum;

namespace Gateway.Web.Host.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly DeviceGrpc.DeviceGrpcClient _deviceGrpcClient;
        private readonly IAppSession _appSession;
        private readonly IMqttService _mqttService;
        private readonly IMapper _mapper;
        public DevicesController(
            DeviceGrpc.DeviceGrpcClient deviceGrpcClient,
            IAppSession appSession,
            IMqttService mqttService,
            IMapper mapper
            )
        {
            _deviceGrpcClient = deviceGrpcClient;
            _appSession = appSession;
            _mapper = mapper;
            _mqttService = mqttService;
        }

        [HttpGet("")]
        public async Task<ResponseDto> GetAllDevices([FromQuery] GetAllDevicesInputDto input)
        {
            try
            {
                GetAllDevicesResponse response = await _deviceGrpcClient.GetAllDevicesAsync(
                    _mapper.Map<GetAllDevicesRequest>(input));
                return new ResponseDto()
                {
                    TotalCount = response.TotalCount,
                    Data = response.Items,
                    Success = true,
                    Message = "Get devices success"
                };
            }
            catch (RpcException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ResponseDto> GetDeviceById(string id)
        {
            try
            {
                GetDeviceByIdResponse response = await _deviceGrpcClient.GetDeviceByIdAsync(
                    new GetDeviceByIdRequest()
                    {
                        Id = id,
                    });
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Get device success"
                };
            }
            catch (RpcException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("code/{deviceCode}")]
        public async Task<ResponseDto> GetDeviceByCode(string deviceCode)
        {
            try
            {
                GetDeviceByCodeResponse response = await _deviceGrpcClient.GetDeviceByCodeAsync(
                    new GetDeviceByCodeRequest()
                    {
                        DeviceCode = deviceCode,
                    });
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Get device success"
                };
            }
            catch (RpcException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("")]
        public async Task<ResponseDto> CreateDevice([FromBody] CreateDeviceInputDto input)
        {
            try
            {
                GetDeviceByCodeResponse res = await _deviceGrpcClient.GetDeviceByCodeAsync(new GetDeviceByCodeRequest()
                {
                    DeviceCode = input.GatewayCode,
                });
                if (input.Type == (int)DEVICE_TYPE.GATEWAY)
                {
                    string topicPublish = TOPIC_GATEWAY_PUBLISH_PREFIX + $"/{input.MacAddress}";
                    RegisterGatewayResponse registerGatewayResponse = new()
                    {
                        data = new Core.Dtos.Devices.DataInside()
                        {
                            code = 0,
                            deviceCode = input.DeviceCode,
                            mac = input.MacAddress,
                        },
                        deviceCode = input.DeviceCode,
                        time = HelpersClass.GetCurrentTimeByLong(),
                        mac = input.MacAddress,
                        to = DEVICE_PUBLISH_GATEWAY,
                        from = DEVICE_PUBLISH_CLOUD,
                        type = TYPE_REGISTER_RESP,
                    };
                    // Gửi thông tin DEVICE_CODE tới gateway
                    _mqttService.PublishMqtt(topicPublish, JsonConvert.SerializeObject(registerGatewayResponse));
                }
                else
                {
                    AddDeviceDusun addDeviceDusun = new()
                    {
                        data = new AddDeviceDataInside()
                        {
                            arguments = new()
                            {
                                attribute = ATTRIBUTE_ADD_DEVICE,
                                ep = 1,
                                value = new()
                                {
                                    mac = input.MacAddress,
                                },
                                mac = input.MacAddress,
                            },
                            command = COMMAND_ADD_DEVICE,
                            id = $"{input.DeviceCode}_{input.Name}",
                        },
                        from = DEVICE_PUBLISH_CLOUD,
                        mac = res.Data.MacAddress,
                        messageId = HelpersClass.GetCurrentTimeByLong(),
                        time = HelpersClass.GetCurrentTimeByLong(),
                        to = DEVICE_PUBLISH_NXP,
                        type = TYPE_CMD,
                    };
                    // Gửi thông tin DEVICE_CODE tới gateway
                    string topicPublish = TOPIC_GATEWAY_PUBLISH_PREFIX + $"/{res.Data.MacAddress}";
                    _mqttService.PublishMqtt(topicPublish, JsonConvert.SerializeObject(addDeviceDusun));
                }
                CreateDeviceRequest request = new()
                {
                    Name = input.Name,
                    TenantId = input.TenantId,
                    DeviceCode = input.DeviceCode,
                    Properties = input.Properties,
                    HomeDeviceId = input.HomeDeviceId,
                    GatewayCode = input.GatewayCode,
                    RoomId = input.RoomId,
                    Producer = input.Producer,
                    MacAddress = input.MacAddress,
                    ImagesUrl = { input.ImagesUrl },
                    Type = input.Type,
                    UserId = _appSession.GetUserId(),
                    DataDusuns = { GetListEndPointDevice(input.Type) },
                };
                CreateDeviceResponse response = await _deviceGrpcClient.CreateDeviceAsync(request);
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Create device success"
                };
            }
            catch (RpcException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ResponseDto> UpdateDevice(string id, [FromBody] UpdateDeviceInputDto input)
        {
            try
            {
                UpdateDeviceResponse response = await _deviceGrpcClient.UpdateDeviceAsync(
                    _mapper.Map<UpdateDeviceRequest>(input));
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Update device success"
                };
            }
            catch (RpcException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("control/{id}")]
        public async Task<ResponseDto> ControlDevice(string id, [FromBody] ControlDeviceInputDto input)
        {
            try
            {
                GetDeviceByIdResponse responseGetDevice = await _deviceGrpcClient.GetDeviceByIdAsync(
                   new GetDeviceByIdRequest()
                   {
                       Id = id,
                   });
                GetDeviceByCodeResponse responseGateway = await _deviceGrpcClient.GetDeviceByCodeAsync(
                    new GetDeviceByCodeRequest()
                    {
                        DeviceCode = responseGetDevice.Data.GatewayCode,
                    });
                ControlDeviceResponse controlResponse = await _deviceGrpcClient.ControlDeviceAsync(
                    _mapper.Map<ControlDeviceRequest>(input));

                if (controlResponse.Data == true && responseGateway.Data.Type == (int)DEVICE_TYPE.GATEWAY_DUSUN)
                {
                    ControlDeviceDto controlDeviceDto = new()
                    {
                        to = DEVICE_PUBLISH_GREENPOWER,
                        from = DEVICE_PUBLISH_CLOUD,
                        type = TYPE_CMD,
                        time = HelpersClass.GetCurrentTimeByLong(),
                        deviceCode = responseGateway.Data.DeviceCode,
                        mac = responseGateway.Data.MacAddress,
                        data = new()
                        {
                            id = responseGetDevice.Data.Id,
                            command = COMMAND_CONTROL_DEVICE,
                            arguments = new()
                            {
                                mac = responseGetDevice.Data.MacAddress,
                                ep = input.ControlDusun.EndPoint.ToString(),
                                attribute = ATTRIBUTE_CONTROL_DEVICE,
                                value = new()
                                {
                                    value = input.ControlDusun.Value,
                                }
                            }
                        }
                    };
                    string topicPublish = TOPIC_GATEWAY_PUBLISH_PREFIX + $"/{responseGateway.Data.MacAddress}";
                    _mqttService.PublishMqtt(topicPublish, JsonConvert.SerializeObject(controlDeviceDto));
                }

                if (controlResponse.Data == true && responseGateway.Data.Type != (int)DEVICE_TYPE.GATEWAY_DUSUN)
                {
                    GetDeviceByIdResponse deviceResponse = await _deviceGrpcClient.GetDeviceByIdAsync(new GetDeviceByIdRequest()
                    {
                        Id = input.Id,
                    });
                    Payload payload = new()
                    {
                        GatewayCode = deviceResponse.Data.GatewayCode,
                        DeviceCode = deviceResponse.Data.DeviceCode,
                        Control = _mapper.Map<ControlDevice>(deviceResponse.Data.Control),
                    };
                    _mqttService.PublishMqtt(TOPIC_CONTROL, JsonConvert.SerializeObject(payload));
                }
                return new ResponseDto()
                {
                    Data = controlResponse.Data,
                    Success = true,
                    Message = "Control device success"
                };
            }
            catch (RpcException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ResponseDto> DeleteDevice(string id)
        {
            try
            {
                GetDeviceByIdResponse responseGetDevice = await _deviceGrpcClient.GetDeviceByIdAsync(
                    new GetDeviceByIdRequest()
                    {
                        Id = id,
                    });
                GetDeviceByCodeResponse responseGateway = await _deviceGrpcClient.GetDeviceByCodeAsync(
                    new GetDeviceByCodeRequest()
                    {
                        DeviceCode = responseGetDevice.Data.GatewayCode,
                    });
                string topicPublish = TOPIC_GATEWAY_PUBLISH_PREFIX + $"/{responseGateway.Data.MacAddress}";
                DeleteDeviceDusun deleteDeviceDusun = new()
                {
                    data = new DeleteDeviceDataInside()
                    {
                        arguments = new()
                        {
                            attribute = ATTRIBUTE_DELETE_DEVICE,
                            ep = 1,
                            value = new()
                            {
                                mac = responseGetDevice.Data.MacAddress,
                            },
                            mac = responseGetDevice.Data.MacAddress,
                        },
                        command = COMMAND_DELETE_DEVICE,
                        id = $"{responseGetDevice.Data.DeviceCode}_{responseGetDevice.Data.Name}",
                    },
                    from = DEVICE_PUBLISH_CLOUD,
                    mac = responseGateway.Data.MacAddress,
                    messageId = (long)(((DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds) / 1000),
                    time = (long)(((DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds) / 1000),
                    to = DEVICE_PUBLISH_NXP,
                    type = TYPE_CMD,
                };

                _mqttService.PublishMqtt(topicPublish, JsonConvert.SerializeObject(deleteDeviceDusun));
                DeleteDeviceResponse response = await _deviceGrpcClient.DeleteDeviceAsync(
                    new DeleteDeviceRequest()
                    {
                        Id = id
                    });
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Delete device success"
                };
            }
            catch (RpcException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<PDataDusun> GetListEndPointDevice(int type)
        {
            List<PDataDusun> dataDusuns = new();
            switch (type)
            {
                case (int)DEVICE_TYPE.DOOR:
                    dataDusuns.Add(new PDataDusun()
                    {
                        EndPoint = 1,
                        Value = 1,
                    });
                    break;
                case (int)DEVICE_TYPE.SWITCH:
                    dataDusuns.Add(new PDataDusun()
                    {
                        Value = 0,
                        EndPoint = 1,
                    });
                    dataDusuns.Add(new PDataDusun()
                    {
                        Value = 0,
                        EndPoint = 2,
                    });
                    dataDusuns.Add(new PDataDusun()
                    {
                        Value = 0,
                        EndPoint = 3,
                    });
                    dataDusuns.Add(new PDataDusun()
                    {
                        Value = 0,
                        EndPoint = 242,
                    });
                    break;
                default:
                    break;
            }
            return dataDusuns;
        }
    }
}
