using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Services;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserCrudService? _userCrudService;
        private readonly IValidator<User> _userValidator;
        private readonly IValidator<int> _idValidator;

        public UserController(IUserCrudService userCrudService, IValidator<User> uservalidator, IValidator<int> idValidator)
        {
            _userCrudService = userCrudService;
            _userValidator = uservalidator;
            _idValidator = idValidator;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            List<User> users = await _userCrudService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("GetById")]
        public async Task<object> GetById([FromQuery] int id)
        {
            var validationResult = await _idValidator.ValidateAsync(id);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            User user = await _userCrudService.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost("Create")]
        public async Task<object> Create([FromBody] User user)
        {
            var validationResult = await _userValidator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            User created = await _userCrudService.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("Update")]
        public async Task<object> Update([FromQuery] User user)
        {
            var validationResult = await _userValidator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            User updated = await _userCrudService.UpdateAsync(user.Id, user);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("Delete")]
        public async Task<object> Delete([FromBody] int id)
        {
            var validationResult = await _idValidator.ValidateAsync(id);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            bool deleted = await _userCrudService.DeleteAsync(id);
            return deleted ? Ok() : NotFound();
        }

    }
}
