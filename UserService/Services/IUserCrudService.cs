using UserService.Data.Entities;

namespace UserService.Services
{
    public interface IUserCrudService
    {
       /* public Task<List<User>> GetAllAsync(CancellationToken cancellationToken);
        public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);*/
        public Task<User> CreateAsync(User user, CancellationToken cancellationToken);
       /* public Task<User?> UpdateAsync(int id, User user, CancellationToken cancellationToken);
        public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);*/
    }
}
