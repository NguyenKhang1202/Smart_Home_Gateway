using AutoMapper;
using Gateway.Core.Dtos;
using Gateway.Core.Dtos.Homes;
using Gateway.Web.Host.Helpers;
using Gateway.Web.Host.Protos.Homes;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Web.Host.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HomesController : ControllerBase
    {
        private readonly HomeGrpc.HomeGrpcClient _homeGrpcClient;
        private readonly IAppSession _appSession;
        private readonly IMapper _mapper;
        private readonly ILogger<HomesController> _logger;
        public HomesController(
            HomeGrpc.HomeGrpcClient homeGrpcClient,
            ILogger<HomesController> logger,
            IAppSession appSession,
            IMapper mapper
            )
        {
            _homeGrpcClient = homeGrpcClient;
            _appSession = appSession;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllHomes([FromQuery] GetAllHomesInputDto input)
        {
            try
            {
                GetAllHomesRequest request = _mapper.Map<GetAllHomesRequest>(input);
                request.UserId = _appSession.GetUserId();
                GetAllHomesResponse response = await _homeGrpcClient.GetAllHomesAsync(request);
                return Ok(new ResponseDto()
                {
                    TotalCount = response.TotalCount,
                    Data = response.Items,
                    Success = true,
                    Message = "Get homes success"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Get homes failed"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHomeById(string id)
        {
            try
            {
                GetHomeByIdResponse response = await _homeGrpcClient.GetHomeByIdAsync(
                    new GetHomeByIdRequest()
                    {
                        Id = id,
                    });
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Get home success"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Get home failed"
                });
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateHome([FromBody] CreateHomeInputDto input)
        {
            try
            {
                CreateHomeRequest request = _mapper.Map<CreateHomeRequest>(input);
                request.UserId = _appSession.GetUserId();
                request.SmartHomeCode = $"{_appSession.GetCurrentUser().Username.ToUpper()}_{HelpersClass.GenerateOrderCode()}";
                CreateHomeResponse response = await _homeGrpcClient.CreateHomeAsync(request);
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Create home success"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Create home failed"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHome(string id, [FromBody] UpdateHomeInputDto input)
        {
            try
            {
                UpdateHomeResponse response = await _homeGrpcClient.UpdateHomeAsync(
                    _mapper.Map<UpdateHomeRequest>(input));
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Update home success"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Update home failed"
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHome(string id)
        {
            try
            {
                DeleteHomeResponse response = await _homeGrpcClient.DeleteHomeAsync(
                    new DeleteHomeRequest()
                    {
                        Id = id
                    });
                return Ok(new ResponseDto()
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Delete home success"
                });
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message);
                return BadRequest(new ResponseDto()
                {
                    Data = null,
                    Success = false,
                    Message = "Delete home failed"
                });
            }
        }
    }
}
