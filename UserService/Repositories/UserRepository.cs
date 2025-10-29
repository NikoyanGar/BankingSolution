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
            if(user is null)
                return Result.Fail("User cannot be null.");

            if (_userDbContext is null)
                throw new InvalidOperationException("Database context is not initialized.");

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

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _userDbContext.Users.ToListAsync(cancellationToken);
        }

        public async Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _userDbContext.Users.FindAsync(id, cancellationToken);
        }

        public async Task<Result<User?>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            if (_userDbContext is null)
                throw new InvalidOperationException("Database context is not initialized.");

            try
            {
                var user = await _userDbContext.Users.FirstOrDefaultAsync(userItem => userItem.Email.ToLower() == email.ToString().ToLower(), cancellationToken);
                return Result.Ok(user);
            }
            catch (Exception ex)
            {
                return Result.Fail<User?>($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<User?>> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            if (username is null)
                return Result.Fail("Username cannot be null.");

            if (_userDbContext is null)
                throw new InvalidOperationException("Database context is not initialized.");

            try
            {
                var user = await _userDbContext.Users.FirstOrDefaultAsync(userItem => userItem.Username.ToLower() == username.ToString().ToLower(), cancellationToken);
                return Result.Ok(user);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to get user by username: {ex.Message}");
            }
        }

        public void Update(User user)
        {
            _userDbContext?.Users.Update(user);
            _userDbContext.SaveChangesAsync();
        }
    }
}
