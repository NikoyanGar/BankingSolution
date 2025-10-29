using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Data.Entities;

namespace UserService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext? _userDbContext;

        public UserRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task Create(User user, CancellationToken cancellationToken = default)
        {
            await _userDbContext.Users.AddAsync(user, cancellationToken);
            await _userDbContext.SaveChangesAsync(cancellationToken);
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

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _userDbContext.Users.FirstOrDefaultAsync(userItem => userItem.Email.ToLower() == email.ToString().ToLower(), cancellationToken);
        }

        public async Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _userDbContext.Users.FindAsync(id, cancellationToken);
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await _userDbContext.Users.FirstOrDefaultAsync(userItem => userItem.Username.ToLower() == username.ToString().ToLower(), cancellationToken);
        }

        public void Update(User user)
        {
            _userDbContext?.Users.Update(user);
            _userDbContext.SaveChangesAsync();
        }
    }
}
