using SmartTrinity.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using SmartTrinity.Core.Services;
using Asp.Versioning;

namespace  SmartTrinity.App.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUsersService _usersService;

        public UserController(ILogger<UserController> logger, IUsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }

        /// <summary>
        /// Authenticate user and return a token
        /// </summary>
        /// <response code="200">UserDTO</response>
        /// <response code="401">Error authenticating user</response>
        /// <response code="500">Server errorr</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponseDetails), 500)]
        public async Task<ActionResult> Authenticate([FromBody] UserLoginDTO userLoginDTO)
        {
            try
            {
                return Ok(await _usersService.Authenticate(userLoginDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
