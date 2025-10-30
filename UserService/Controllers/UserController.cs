using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserService.Data.Entities;
using UserService.Models.Responses;
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
        

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancelationToken)
        {
            var users = await _userCrudService!.GetAllAsync(cancelationToken);
            if (users.IsFailed)
                return BadRequest();
            return Ok(users.Value);
        }
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancelationToken)
        {
            var result = await _idValidator.ValidateAsync(id);
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            var user = await _userCrudService!.GetByIdAsync(id, cancelationToken);
            if (user.IsFailed)
                return BadRequest();

            return user.Value == null ? NotFound() : Ok(user.Value);
        }
        

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user, CancellationToken cancelationToken)
        {
            var result = await _userValidator.ValidateAsync(user);
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            var created = await _userCrudService!.CreateAsync(user, cancelationToken);
            if (created.IsFailed)
                return BadRequest();

            return Created(nameof(GetById), created.Value);
        }
        

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] User user, CancellationToken cancelationToken)
        {
            var result = await _userValidator.ValidateAsync(user);
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            var updated = await _userCrudService!.UpdateAsync(id, user, cancelationToken);
            if(updated.IsFailed)
                return BadRequest();

            return updated.Value == null ? NotFound() : Ok(updated.Value);
        }
        
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancelationToken)
        {
            var result = await _idValidator.ValidateAsync(id);
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            var deleted = await _userCrudService!.DeleteAsync(id, cancelationToken);
            if (deleted.IsFailed)
                return BadRequest();

            return deleted.Value ? NoContent() : NotFound();
        }
    }
}
