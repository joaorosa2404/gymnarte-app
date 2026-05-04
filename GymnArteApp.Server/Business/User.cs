using GymnArteApp.Server.Business.Interface;

namespace GymnArteApp.Server.Business
{
    public class User : IUser
    {
        private readonly Repo.Interface.IUser _userRepo;

        public User(Repo.Interface.IUser userRepo) => _userRepo = userRepo;

        public async Task<IEnumerable<Models.User>> GetAllUsersAsync()
        {
            return await _userRepo.GetAllUsersAsync();
        }

        public async Task<Models.User> GetUserByIdAsync(int id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);
            if (user is null) throw new Exception($"User with ID {id} not found.");
            return user;
        }

        public async Task<IEnumerable<Models.User>> GetUsersByScopeAsync(int scopeId)
        {
            return await _userRepo.GetUsersByScopeAsync(scopeId);
        }

        public async Task<IEnumerable<Models.User>> GetUsersByRoleAsync(Models.Enums.UserRole role)
        {
            return await _userRepo.GetUsersByRoleAsync(role);
        }

        public async Task<Models.User> CreateUserAsync(Models.User user)
        {
            return await _userRepo.CreateUserAsync(user);
        }

        public async Task<Models.User> UpdateUserAsync(int id, Models.User user, string token)
        {
            return await _userRepo.UpdateUserAsync(id, user, token);
        }

        public async Task<bool> DeleteUserAsync(int id, string token)
        {
            return await _userRepo.DeleteUserAsync(id, token);
        }
    }
}
