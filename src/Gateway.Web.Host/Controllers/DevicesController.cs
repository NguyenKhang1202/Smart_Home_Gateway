using AutoMapper;
using Gateway.Core.Dtos;
using Gateway.Core.Dtos.Devices;
using Gateway.Web.Host.Helpers;
using Gateway.Web.Host.Protos.Devices;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Web.Host.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly DeviceGrpc.DeviceGrpcClient _deviceGrpcClient;
        private readonly IMapper _mapper;
        public DevicesController(
            DeviceGrpc.DeviceGrpcClient deviceGrpcClient,
            IMapper mapper
            )
        {
            _deviceGrpcClient = deviceGrpcClient;
            _mapper = mapper;
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

        [HttpPost("")]
        public async Task<ResponseDto> CreateDevice([FromBody] CreateDeviceInputDto input)
        {
            try
            {
                CreateDeviceResponse response = await _deviceGrpcClient.CreateDeviceAsync(
                    _mapper.Map<CreateDeviceRequest>(input));
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

        [HttpDelete("{id}")]
        public async Task<ResponseDto> DeleteDevice(string id)
        {
            try
            {
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
    }
}
