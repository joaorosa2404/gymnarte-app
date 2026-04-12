using GymnArteApp.Server.Business.Interface;

namespace GymnArteApp.Server.Business
{
    public class UserScope : IUserScope
    {
        private readonly Repo.Interface.IUserScope _userScopeRepo;

        public UserScope(Repo.Interface.IUserScope userScopeRepo)
        {
            _userScopeRepo = userScopeRepo;
        }

        public async Task<IEnumerable<Models.UserScope>> GetAllUserScopesAsync()
        {
            try
            {
                var scopes = await _userScopeRepo.GetAllUserScopesAsync();
                return scopes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business layer while retrieving all user scopes.", ex);
            }
        }

        public async Task<Models.UserScope> GetUserScopeByIdAsync(int id)
        {
            try
            {
                var scope = await _userScopeRepo.GetUserScopeByIdAsync(id);
                if (scope == null) throw new Exception($"User scope with ID {id} not found.");

                return scope;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while retrieving user scope ID {id}.", ex);
            }
        }

        public async Task<Models.UserScope> GetUserScopeByUserIdAsync(int userId)
        {
            try
            {
                var scope = await _userScopeRepo.GetUserScopeByUserIdAsync(userId);
                return scope;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while retrieving user scope for user ID {userId}.", ex);
            }
        }

        public async Task<Models.UserScope> CreateUserScopeAsync(Models.UserScope userScope, string token)
        {
            try
            {
                var newScope = await _userScopeRepo.CreateUserScopeAsync(userScope, token);
                return newScope;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business layer while creating user scope.", ex);
            }
        }

        public async Task<Models.UserScope> UpdateUserScopeAsync(Models.UserScope userScope, int id, string token)
        {
            try
            {
                var updatedScope = await _userScopeRepo.UpdateUserScopeAsync(userScope, id, token);
                return updatedScope;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while updating user scope ID {id}.", ex);
            }
        }

        public async Task<bool> DisableUserScopeAsync(int id, string token)
        {
            try
            {
                var result = await _userScopeRepo.DisableUserScopeAsync(id, token);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while disabling user scope ID {id}.", ex);
            }
        }
    }
}