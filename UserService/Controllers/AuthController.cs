using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserService.Auth;
using UserService.Models.Requests;
using UserService.Models.Responses;
using static UserService.Middlewares.AuthenticationMiddleware;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService? _authService;
        private readonly IValidator<RegisterRequest>? _registerValidator;
        private readonly IValidator<LoginRequest>? _loginValidator;
        private readonly IValidator<string>? _tokenValidator;

        public AuthController(IAuthService authService,
            IValidator<RegisterRequest> registerValidator,
            IValidator<LoginRequest>? loginValidator,
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
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var result = await _loginValidator!.ValidateAsync(loginRequest);
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            var loginResponse = await _authService!.LoginAsync(loginRequest, cancellationToken);
            if (loginResponse.IsFailed)
                return Unauthorized("Invalid credentials.");

            return Ok(loginResponse.Value);
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
        public async Task<ActionResult<RegistrationResponse>> Register([FromBody] RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            var result = await _registerValidator!.ValidateAsync(registerRequest);
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            var registerResponse = await _authService!.RegisterAsync(registerRequest, cancellationToken);
            if (registerResponse.IsFailed)
            {
                return StatusCode(500, registerResponse.Errors);
            }

            return Created(
                nameof(Register),
                registerResponse?.Value);
        }


        [HttpPost("token")]
        public async Task<ActionResult<TokenResponse>> GenerateToken([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var result = await _loginValidator!.ValidateAsync(loginRequest);
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            try
            {
                var tokenResponse = await _authService!.GenerateToken(loginRequest, cancellationToken);

                if (tokenResponse.IsFailed)
                    return Unauthorized("Invalid email or password.");

                return Ok(tokenResponse.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
