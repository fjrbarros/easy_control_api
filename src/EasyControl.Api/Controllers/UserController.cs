using System.Security.Authentication;
using EasyControl.Api.Contract.User;
using EasyControl.Api.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyControl.Api.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("auth")]
        [AllowAnonymous]
        public async Task<IActionResult> Authentication(UserLoginRequestContract userContract)
        {
            try
            {
                var user = await _userService.Authentication(userContract);

                return Ok(user);
            }
            catch (AuthenticationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(UserCreateRequestContract userContract)
        {
            try
            {
                var user = await _userService.Create(userContract);

                return Created("", user);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAll();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid userId)
        {
            try
            {
                var user = await _userService.GetById(userId);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{userId}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid userId, UserCreateRequestContract userContract)
        {
            try
            {
                var user = await _userService.Update(userId, userContract);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid userId)
        {
            try
            {
                await _userService.Delete(userId);

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
