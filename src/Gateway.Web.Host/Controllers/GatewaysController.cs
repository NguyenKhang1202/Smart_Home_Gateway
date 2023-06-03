using AutoMapper;
using Gateway.Core.Dtos;
using Gateway.Core.Dtos.Gateways;
using Gateway.Web.Host.Helpers;
using Gateway.Web.Host.Protos.Devices;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Web.Host.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GatewaysController : ControllerBase
    {
        private readonly DeviceGrpc.DeviceGrpcClient _deviceGrpcClient;
        private readonly IAppSession _appSession;
        private readonly IMapper _mapper;
        public GatewaysController(
            DeviceGrpc.DeviceGrpcClient deviceGrpcClient,
            IAppSession appSession,
            IMapper mapper
            )
        {
            _deviceGrpcClient = deviceGrpcClient;
            _appSession = appSession;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ResponseDto> GetAllGateways([FromQuery] GetAllGatewaysInputDto input)
        {
            try
            {
                GetAllGatewaysResponse response = await _deviceGrpcClient.GetAllGatewaysAsync(
                    _mapper.Map<GetAllGatewaysRequest>(input));
                return new ResponseDto()
                {
                    TotalCount = response.TotalCount,
                    Data = response.Items,
                    Success = true,
                    Message = "Get gateways success"
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
        public async Task<ResponseDto> GetGatewayById(string id)
        {
            try
            {
                GetGatewayByIdResponse response = await _deviceGrpcClient.GetGatewayByIdAsync(
                    new GetGatewayByIdRequest()
                    {
                        Id = id,
                    });
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Get gateway success"
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
        public async Task<ResponseDto> GetGatewayByCode(string deviceCode)
        {
            try
            {
                GetGatewayByCodeResponse response = await _deviceGrpcClient.GetGatewayByCodeAsync(
                    new GetGatewayByCodeRequest()
                    {
                        DeviceCode = deviceCode,
                    });
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Get gateway success"
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
        public async Task<ResponseDto> CreateGateway([FromBody] CreateGatewayInputDto input)
        {
            try
            {
                CreateGatewayResponse response = await _deviceGrpcClient.CreateGatewayAsync(
                    _mapper.Map<CreateGatewayRequest>(input));
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Create gateway success"
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
        public async Task<ResponseDto> UpdateGateway(string id, [FromBody] UpdateGatewayInputDto input)
        {
            try
            {
                UpdateGatewayResponse response = await _deviceGrpcClient.UpdateGatewayAsync(
                    _mapper.Map<UpdateGatewayRequest>(input));  
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Update gateway success"
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
        public async Task<ResponseDto> DeleteGateway(string id)
        {
            try
            {
                DeleteGatewayResponse response = await _deviceGrpcClient.DeleteGatewayAsync(
                    new DeleteGatewayRequest()
                    {
                        Id = id
                    });
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Delete gateway success"
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
