using FluentResults;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserService.Auth;
using UserService.Models.DTOs;
using UserService.Models.Responses;
using static UserService.Middlewares.AuthenticationMiddleware;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService? _authService;
        private readonly IValidator<RegisterDto>? _registerValidator;
        private readonly IValidator<LoginDto>? _loginValidator;
        private readonly IValidator<string>? _tokenValidator;

        public AuthController(IAuthService authService,
            IValidator<RegisterDto> registerValidator,
            IValidator<LoginDto>? loginValidator,
            IValidator<string>? tokenValidator)
        {
            _authService = authService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _tokenValidator = tokenValidator;
        }

        [Roles("admin,manager")]
        [HttpGet("dashboard")]
        public IActionResult GetAdminDashboard()
        {
            return Ok("Welcome to admin dashboard!");
        }


        [HttpGet("public")]
        public IActionResult GetPublicData()
        {
            return Ok("Anyone can access this endpoint.");
        }


        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginDto loginDto, CancellationToken cancellationToken)
        {
            if (_loginValidator is null)
            {
                return Problem(
                    detail: "Validation service is not available.",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }

            var result = await _loginValidator.ValidateAsync(loginDto);
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            if (_authService is null)
            {
                return StatusCode(500, "Authentication service is not available.");
            }
            var loginResponse = await _authService.LoginAsync(loginDto, cancellationToken);
            if (loginResponse == null)
                return Unauthorized("Invalid credentials.");

            return Ok(loginResponse);
        }


        [HttpGet("validate-token")]
        public async Task<ActionResult<UserInfo>> ValidateToken([FromQuery] string token)
        {
            if (_tokenValidator is null)
            {
                return Problem(
                    detail: "Validation service is not available.",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
            var result = await _tokenValidator.ValidateAsync(token);
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            if (_authService is null)
            {
                return StatusCode(500, "Authentication service is not available.");
            }

            var userInfoResponse = await _authService.ValidateToken(token);
            if (userInfoResponse.IsFailed)
                return Unauthorized("Invalid or expired token.");

            return Ok(userInfoResponse.Value);
        }


        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> Register([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
        {
            if (_registerValidator is null)
            {
                return Problem(
                    detail: "Validation service is not available.",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }

            var result = await _registerValidator.ValidateAsync(registerDto);
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            if (_authService is null)
            {
                return StatusCode(500, "Authentication service is not available.");
            }

            var registerResponse = await _authService.RegisterAsync(registerDto, cancellationToken);
            if (!string.IsNullOrEmpty(registerResponse!.ErrorMessage))
            {
                return StatusCode(500, registerResponse.ErrorMessage);
            }

            return CreatedAtAction(
                nameof(Register),
                new { id = registerResponse?.User?.Id }, registerResponse?.User);
        }


        [HttpPost("token")]
        public async Task<ActionResult<TokenResponse>> GenerateToken([FromBody] LoginDto loginDto, CancellationToken cancellationToken)
        {
            if (_loginValidator is null)
            {
                return Problem(
                    detail: "Validation service is not available.",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }

            var result = await _loginValidator.ValidateAsync(loginDto);
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            if (_authService is null)
            {
                return StatusCode(500, "Authentication service is not available.");
            }

            try
            {
                var tokenResponse = await _authService.GenerateToken(loginDto, cancellationToken);

                if (tokenResponse == null)
                    return Unauthorized("Invalid email or password.");

                return Ok(tokenResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
