using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace F4ConversationCloud.WebUI.Controllers
{
    [ApiController]
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {

            _authService = authService;

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserDetailsDTO userDetailsDTO)
        {
            try
            {
                var result = _authService.ValidateUser(userDetailsDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500);

            }
        }
    }
}
