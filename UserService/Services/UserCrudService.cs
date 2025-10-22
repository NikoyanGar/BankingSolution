using UserService.Models;
using UserService.Repositories;

namespace UserService.Services
{
    public class UserCrudService: IUserCrudService
    {
        private List<User>? users;
        private readonly IUserRepository? _userRepository;

        public UserCrudService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            users = new List<User>();
        }

        public async Task<User> CreateAsync(User user)
        {
            await _userRepository.Create(user);
            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _userRepository.GetByIdAsync(id);
            if (existing == null) return false;

            _userRepository.Delete(existing);
            return true;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> UpdateAsync(int id, User user)
        {
            var existing = await _userRepository.GetByIdAsync(id);
            if (existing == null) return null;

            existing.FirstName = user.FirstName;
            existing.Email = user.Email;

            _userRepository.Update(existing);

            return existing;
        }
    }
}
