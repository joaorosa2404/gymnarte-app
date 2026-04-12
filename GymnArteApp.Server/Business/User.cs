using GymnArteApp.Server.Business.Interface;

namespace GymnArteApp.Server.Business
{
    public class User : IUser
    {
        private readonly Repo.Interface.IUser _userRepo;

        public User(Repo.Interface.IUser userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<IEnumerable<Models.User>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepo.GetAllUsersAsync();
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business layer while retrieving all users.", ex);
            }
        }

        public async Task<Models.User> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _userRepo.GetUserByIdAsync(id);
                if (user == null) throw new Exception($"User with ID {id} not found.");

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while retrieving user ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Models.User>> GetUsersByScopeAsync(int scopeId)
        {
            try
            {
                var users = await _userRepo.GetUsersByScopeAsync(scopeId);
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while retrieving users for scope ID {scopeId}.", ex);
            }
        }

        public async Task<Models.User> CreateUserAsync(Models.User user)
        {
            try
            {
                var newUser = await _userRepo.CreateUserAsync(user);
                return newUser;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business layer while creating user.", ex);
            }
        }

        public async Task<Models.User> UpdateUserAsync(int id, Models.User user, string token)
        {
            try
            {
                var updatedUser = await _userRepo.UpdateUserAsync(id, user, token);
                return updatedUser;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while updating user ID {id}.", ex);
            }
        }

        public async Task<bool> DeleteUserAsync(int id, string token)
        {
            try
            {
                var result = await _userRepo.DeleteUserAsync(id, token);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while deleting user ID {id}.", ex);
            }
        }
    }
}