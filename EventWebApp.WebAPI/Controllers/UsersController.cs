using EventWebApp.Application.DTOs;
using EventWebApp.Application.UseCases.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventWebApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly RegisterUserUseCase registerUserUseCase;
        private readonly GetUserByIdUseCase getUserByIdUseCase;
        private readonly GetUsersByEventUseCase getUserByEventUseCase;
        private readonly RegisterUserToEventUseCase registerUserToEventUseCase;
        private readonly CancelUserFromEventUseCase cancelUserFromEventUseCase;

        public UsersController (RegisterUserUseCase registerUserUseCase, 
            GetUserByIdUseCase getUserByIdUseCase,
            GetUsersByEventUseCase getUserByEventUseCase,
            RegisterUserToEventUseCase registerUserToEventUseCase,
            CancelUserFromEventUseCase cancelUserFromEventUseCase)
        {
            this.registerUserUseCase = registerUserUseCase;
            this.getUserByIdUseCase = getUserByIdUseCase;
            this.getUserByEventUseCase = getUserByEventUseCase;
            this.registerUserToEventUseCase = registerUserToEventUseCase;
            this.cancelUserFromEventUseCase = cancelUserFromEventUseCase;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await getUserByIdUseCase.ExecuteAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest userRegistrationRequest)
        {
            var result = await registerUserUseCase.ExecuteAsync(userRegistrationRequest);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("by-event/{eventId}")]
        public async Task<IActionResult> GetUsersByEvent (Guid eventId)
        {
            var result = await getUserByEventUseCase.ExecuteAsync(eventId);
            return Ok(result);
        }

        [Authorize(Policy = "UserOnly")]
        [HttpPost("register-to-event")]
        public async Task<IActionResult> RegisterToEvent([FromBody] RegisterUserToEventRequest registerUserToEventRequest)
        {
            await registerUserToEventUseCase.ExecuteAsync(registerUserToEventRequest.UserId, registerUserToEventRequest.EventId);
            return Ok();
        }

        [Authorize(Policy = "UserOnly")]
        [HttpPost("cancel-from-event")]
        public async Task<IActionResult> CancelFromEvent([FromBody] RegisterUserToEventRequest registerUserToEventRequest)
        {
            await cancelUserFromEventUseCase.ExecuteAsync(registerUserToEventRequest.UserId, registerUserToEventRequest.EventId);
            return Ok();
        }
    }
}
