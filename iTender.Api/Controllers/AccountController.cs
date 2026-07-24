using iTender.Application.Commands.Credentials;
using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Application.Queries.Credentials;
using iTender.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iTender.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly GetCredentialByCredentialsQueryHandler _handler;
        private readonly UpdateCredentialCommandHandler _updateHandler;
        private readonly IJwtTokenService _tokenService;
        private readonly IContactRepository _repository;

        public AccountController(GetCredentialByCredentialsQueryHandler handler, UpdateCredentialCommandHandler updateHandler, IContactRepository repository, IJwtTokenService tokenService)
        {
            _handler = handler;
            _updateHandler = updateHandler;
            _repository = repository;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request,CancellationToken ct)
        {
            var query = new GetCredentialByCredentialsQuery(
                   request.Email,
                   request.Password);

            var credential = await _handler.Handle(query, ct);

            if (credential == null)
            {
                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = "Invalid username or password."
                });
            }

            var entity = _repository.GetUserByEmailAddress(credential.Username, new string[] { "fullname", "nv_employerid" });

            if (entity == null || entity.Count == 0)
            {
                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = "User profile not found."
                });
            }

            if (credential.IsLocked == true)
            {
                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = "This account has been locked. Please contact an administrator for assistance."
                });
            }

            if (credential.ForcePasswordChange == true)
            {
                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = "Please reset your password."
                });
            }

            if (entity[0].ContactType == 100000001)
            {
                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = "This account is currently under case management."
                });
            }

            if (entity[0].AccountEnabled == false)
            {
                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = "This account has been deactivated. Please contact your employer."
                });
            }

            List<PermissionModel> userPermissions =
                await _repository.GetContactsPermissions(entity[0].Id);            

            var token = _tokenService.GenerateJwtToken(credential.Username, userPermissions);

            credential.IncorrectLoginCount = 0;
           
            var command = new UpdateCredentialCommand(credential.Id, credential.IncorrectLoginCount.Value, request.Email, request.Password);
            await _updateHandler.Handle(command, ct);

            var response = new LoginResponse
            {
                Success = true,
                Token = token,
                User = new AuthenticatedUser
                {
                    FullName = entity[0].FullName,
                    Email = credential.Username,
                    Employer = entity[0].EmployerId.Value.ToString(),
                    Role = entity[0].Role
                }
            };

            return Ok(response);
        }
    }
}
