using AutoMapper;
using Gateway.Core.Dtos;
using Gateway.Core.Dtos.Homes;
using Gateway.Web.Host.Helpers;
using Gateway.Web.Host.Protos.Homes;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Web.Host.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HomesController : ControllerBase
    {
        private readonly HomeGrpc.HomeGrpcClient _homeGrpcClient;
        private readonly IMapper _mapper;
        public HomesController(
            HomeGrpc.HomeGrpcClient homeGrpcClient,
            IMapper mapper
            ) 
        {
            _homeGrpcClient = homeGrpcClient;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ResponseDto> GetAllHomes([FromQuery] GetAllHomesInputDto input)
        {
            try
            {
                GetAllHomesResponse response = await _homeGrpcClient.GetAllHomesAsync(
                    _mapper.Map<GetAllHomesRequest>(input));
                return new ResponseDto()
                {
                    Data = response.Items,
                    Success = true,
                    Message = "Get homes success"
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
        public async Task<ResponseDto> GetHomeById(string id)
        {
            try
            {
                GetHomeByIdResponse response = await _homeGrpcClient.GetHomeByIdAsync(
                    new GetHomeByIdRequest()
                    {
                        Id = id,
                    });
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Get home success"
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
        public async Task<ResponseDto> CreateHome([FromBody] CreateHomeInputDto input)
        {
            try
            {
                CreateHomeResponse response = await _homeGrpcClient.CreateHomeAsync(
                    _mapper.Map<CreateHomeRequest>(input));
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Create home success"
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
        public async Task<ResponseDto> UpdateHome(string id, [FromBody] UpdateHomeInputDto input)
        {
            try
            {
                UpdateHomeResponse response = await _homeGrpcClient.UpdateHomeAsync(
                    _mapper.Map<UpdateHomeRequest>(input));
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Update home success"
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
        public async Task<ResponseDto> DeleteHome(string id)
        {
            try
            {
                DeleteHomeResponse response = await _homeGrpcClient.DeleteHomeAsync(
                    new DeleteHomeRequest()
                    {
                        Id = id
                    });
                return new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Delete home success"
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
