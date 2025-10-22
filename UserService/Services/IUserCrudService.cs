using UserService.Models;

namespace UserService.Services
{
    public interface IUserCrudService
    {
        public Task<List<User>> GetAllAsync();
        public Task<User?> GetByIdAsync(int id);
        public Task<User> CreateAsync(User user);
        public Task<User?> UpdateAsync(int id, User user);
        public Task<bool> DeleteAsync(int id);
    }
}
