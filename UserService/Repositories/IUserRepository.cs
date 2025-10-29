using FluentResults;
using UserService.Data.Entities;

namespace UserService.Repositories
{
    public interface IUserRepository
    {
        public Task<Result> Create(User user, CancellationToken cancellationToken);
        public Task<List<User>> GetAllAsync(CancellationToken cancellationToken);
        public void Update(User user);
        public Task<User> GetByIdAsync(int id, CancellationToken cancellationToken);
        public void Delete(User user);
        public Task<Result<User?>> GetByUsernameAsync(string username, CancellationToken cancellationToken);
        public Task<Result<User?>> GetByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
