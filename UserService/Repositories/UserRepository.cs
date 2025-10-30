using FluentResults;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Data.Entities;

namespace UserService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _userDbContext;

        public UserRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<Result> Create(User user, CancellationToken cancellationToken = default)
        {
            try
            {
                await _userDbContext.Users.AddAsync(user, cancellationToken);
                await _userDbContext.SaveChangesAsync(cancellationToken);
                return Result.Ok();
            }
            catch (Exception ex) 
            {
                return Result.Fail($"Failed to create user: {ex.Message}");
            }
        }

        public void Delete(User user)
        {
            _userDbContext.Users.Remove(user);
            _userDbContext.SaveChangesAsync();
        }

        public async Task<Result<List<User>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var users = await _userDbContext.Users.ToListAsync(cancellationToken);
                return Result.Ok<List<User>>(users);
            }
            catch (Exception ex)
            {
                return Result.Fail<List<User>>($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<User?>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userDbContext.Users.FindAsync(id, cancellationToken);
                return Result.Ok<User?>(user);
            }
            catch (Exception ex)
            {
                return Result.Fail<User?>($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<User?>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userDbContext.Users.FirstOrDefaultAsync(userItem => userItem.Email.ToLower() == email.ToString().ToLower(), cancellationToken);
                return Result.Ok<User?>(user);
            }
            catch (Exception ex)
            {
                return Result.Fail<User?>($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<User?>> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userDbContext.Users.FirstOrDefaultAsync(userItem => userItem.Username.ToLower() == username.ToString().ToLower(), cancellationToken);
                return Result.Ok<User?>(user);
            }
            catch (Exception ex)
            {
                return Result.Fail<User?>($"Failed to get user by username: {ex.Message}");
            }
        }
    
    }
}
