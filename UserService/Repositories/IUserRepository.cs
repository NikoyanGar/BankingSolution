using FluentResults;
using UserService.Data.Entities;
using UserService.Models.Responses;

namespace UserService.Repositories
{
    public interface IUserRepository
    {
        public Task<Result> Create(User user, CancellationToken cancellationToken);
        public Task<Result<List<UserResponse>>> GetAllAsync(CancellationToken cancellationToken);
        public Task<Result<UserResponse?>> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<Result> Delete(int id, CancellationToken cancellationToken);
        public Task<Result<User?>> GetByUsernameAsync(string username, CancellationToken cancellationToken);
        public Task<Result<User?>> GetByEmailAsync(string email, CancellationToken cancellationToken);
        public Task SaveChangesAsync(CancellationToken cancellationToken);
        public Task<User?> GetUserEntityByIdAsync(int id, CancellationToken cancellationToken);
    }
}
