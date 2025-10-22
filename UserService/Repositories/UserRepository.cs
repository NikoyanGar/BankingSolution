using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;

namespace UserService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext? _userDbContext;

        public UserRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task Create(User user)
        {
            await _userDbContext.Users.AddAsync(user);
            await _userDbContext.SaveChangesAsync();
        }

        public void Delete(User user)
        {
            _userDbContext.Users.Remove(user);
            _userDbContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userDbContext.Users.ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userDbContext.Users.FirstOrDefaultAsync(userItem => userItem.Email.ToLower() == email.ToString().ToLower());
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _userDbContext.Users.FindAsync(id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _userDbContext.Users.FirstOrDefaultAsync(userItem => userItem.Username.ToLower() == username.ToString().ToLower());
        }

        public void Update(User user)
        {
            _userDbContext?.Users.Update(user);
            _userDbContext.SaveChangesAsync();
        }

        public async Task<string> GetNextClientIdAsync()
        {
            int nextNumber = 1;
            const string prefix = "C";
            var lastUser = await _userDbContext.Users.Where(u => u.ClientId != null).OrderByDescending(u => u.Id).FirstOrDefaultAsync();

            if (lastUser != null)
            {
                var digits = new string(lastUser?.ClientId?.Where(char.IsDigit).ToArray());
                Console.WriteLine($"digits: {digits}");
                if (int.TryParse(digits, out var n))
                    nextNumber = n + 1;
            }

            return $"{prefix}{nextNumber}";
        }
    }
}
