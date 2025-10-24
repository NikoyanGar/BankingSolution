using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserService.Auth;
using UserService.Models;
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

        [HttpPost("Register")]
        public async Task<object> Register([FromBody] RegisterDto registerDto)
        {
            var validationResult = await _registerValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var response = await _authService.RegisterAsync(registerDto);
            if (response == null)
                return BadRequest("Email/Username already exists.");

            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<object> Login([FromBody] LoginDto loginDto)
        {
            var validationResult = await _loginValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var response = await _authService.LoginAsync(loginDto);
            if (response == null)
                return Unauthorized("Invalid credentials.");

            return Ok(response);
        }

        [HttpGet("ValidateToken")]
        public async Task<object> ValidateToken([FromQuery] string token)
        {
            var validationResult = await _tokenValidator.ValidateAsync(token);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var userId = await _authService.ValidateToken(token);
            if (userId == null)
                return Unauthorized("Invalid or expired token.");

            return Ok(new { userId });
        }

        [HttpPost("GenerateToken")]
        public async Task<object> GenerateToken([FromBody] LoginDto loginDto)
        {
            var validationResult = await _loginValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }
            var tokenResult = await _authService!.GenerateToken(loginDto.Email, loginDto.Password);

            return Ok(new
            {
                Token = tokenResult?.Token,
                ExpiresAt = tokenResult?.ExpiresAt
            });
        }   

    }
}
