using FluentResults;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using UserService.Data;
using UserService.Data.Entities;
using UserService.Models.Responses;

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

        public async Task<Result> Delete(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userDbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
                if (user is null)
                    return null!;

                _userDbContext.Users.Remove(user);
                await _userDbContext!.SaveChangesAsync(cancellationToken);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to delete user: {ex.Message}");
            }
        }

        public async Task<Result<List<UserResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var users = await _userDbContext.Users.ToListAsync(cancellationToken);
                var userResponses = users.Select(user => new UserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    ClientId = user.ClientId,
                    Roles = user.Roles
                }).ToList();
                return Result.Ok<List<UserResponse>>(userResponses);
            }
            catch (Exception ex)
            {
                return Result.Fail<List<UserResponse>>($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<UserResponse?>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userDbContext.Users.FindAsync(id, cancellationToken);
                var userResponse = new UserResponse 
                {
                    Id = user!.Id,
                    Username = user.Username,
                    Email = user.Email,
                    ClientId = user.ClientId,
                    Roles = user.Roles
                };
                return Result.Ok<UserResponse?>(userResponse);
            }
            catch (Exception ex)
            {
                return Result.Fail<UserResponse?>($"Database error: {ex.Message}");
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

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _userDbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<User?> GetUserEntityByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _userDbContext.Users.FindAsync(new object[] { id }, cancellationToken);
        }
    }
}
