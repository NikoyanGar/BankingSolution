using UserService.Models;

namespace UserService.Repositories
{
    public interface IUserRepository
    {
        public Task Create(User user);
        public Task<List<User>> GetAllAsync();
        public void Update(User user);
        public Task<User> GetByIdAsync(int id);
        public void Delete(User user);
        public Task<User?> GetByUsernameAsync(string username);
        public Task<User?> GetByEmailAsync(string email);
        public Task<string> GetNextClientIdAsync();
    }
}
