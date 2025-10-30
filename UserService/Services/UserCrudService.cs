using System.Threading;
using UserService.Data.Entities;
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

        public async Task<User> CreateAsync(User user, CancellationToken cancellationToken)
        {
            await _userRepository.Create(user, cancellationToken);
            return user;
        }
        /*
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var existing = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null) return false;

            _userRepository.Delete(existing);
            return true;
        }

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _userRepository.GetAllAsync(cancellationToken);
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _userRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<User?> UpdateAsync(int id, User user, CancellationToken cancellationToken)
        {
            var existing = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null) return null;

            existing.FirstName = user.FirstName;
            existing.Email = user.Email;

            //_userRepository.Update(existing);

            return existing;
        }
    */
    }
}
