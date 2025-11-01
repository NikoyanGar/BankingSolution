using FluentResults;
using UserService.Data.Entities;
using UserService.Models.Responses;

namespace UserService.Services
{
    public interface IUserCrudService
    {
        public Task<Result<List<UserResponse>?>> GetAllAsync(CancellationToken cancellationToken);
        public Task<Result<UserResponse?>> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<Result<UserResponse?>> CreateAsync(User user, CancellationToken cancellationToken);
        public Task<Result<UserResponse?>> UpdateAsync(int id, User user, CancellationToken cancellationToken);
        public Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
