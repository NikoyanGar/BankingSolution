using FluentResults;
using UserService.Data.Entities;
using UserService.Models.Responses;
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

        public async Task<Result<UserResponse?>> CreateAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                await _userRepository!.Create(user, cancellationToken);
                var userResponse = new UserResponse
                {
                    Id = user.Id,
                    ClientId = user.ClientId,
                    Username = user.Username,
                    Email = user.Email,
                    Roles = user.Roles
                };
                return Result.Ok<UserResponse?>(userResponse);
            }
            catch(Exception ex)
            {
                return Result.Fail<UserResponse?>(ex.Message);
            }
        }
        
        public async Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userRepository!.Delete(id, cancellationToken);
                if (result == null) return Result.Ok<bool>(false);

                return Result.Ok<bool>(true);
            }
            catch (Exception ex)
            {
                return Result.Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<List<UserResponse>?>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userRepository!.GetAllAsync(cancellationToken);
                var userResponses = users.Value.Select(user => new UserResponse
                {
                    Id = user.Id,
                    ClientId = user.ClientId,
                    Username = user.Username,
                    Email = user.Email,
                    Roles = user.Roles
                }).ToList();
                return Result.Ok<List<UserResponse>?>(userResponses);
            }
            catch (Exception ex)
            {
                return Result.Fail<List<UserResponse>?>(ex.Message);
            }
        }

        public async Task<Result<UserResponse?>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository!.GetByIdAsync(id, cancellationToken);
                if (user.Value == null) return null!;
                var userResponse = new UserResponse
                {
                    Id = user.Value!.Id,
                    ClientId = user.Value.ClientId,
                    Username = user.Value.Username,
                    Email = user.Value.Email,
                    Roles = user.Value.Roles
                };
                return Result.Ok<UserResponse?>(userResponse);
            }
            catch (Exception ex)
            {
                return Result.Fail<UserResponse?>(ex.Message);
            }
        }

        
        public async Task<Result<UserResponse?>> UpdateAsync(int id, User user, CancellationToken cancellationToken)
        {
            try
            {
                var userEntity = await _userRepository!.GetUserEntityByIdAsync(id, cancellationToken);
                if (userEntity == null) return null!;

                userEntity.Roles = user.Roles;
                userEntity.Email = user.Email;
                userEntity.FirstName = user.FirstName;
                userEntity.LastName = user.LastName;
                userEntity.Username = user.Username;
                userEntity.ClientId = user.ClientId;
                userEntity.Password = user.Password;
                await _userRepository.SaveChangesAsync(cancellationToken);

                var userResponse = new UserResponse
                {
                    Id = userEntity.Id,
                    ClientId = userEntity.ClientId,
                    Username = userEntity.Username,
                    Email = userEntity.Email,
                    Roles = userEntity.Roles
                };
                return Result.Ok<UserResponse?>(userResponse);
            }
            catch (Exception ex)
            {
                return Result.Fail<UserResponse?>(ex.Message);
            }
        }
    }
}
