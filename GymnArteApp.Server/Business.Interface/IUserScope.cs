namespace GymnArteApp.Server.Business.Interface
{
    public interface IUserScope
    {
        Task<IEnumerable<Models.UserScope>> GetAllUserScopesAsync();
        Task<Models.UserScope> GetUserScopeByIdAsync(int id);
        Task<Models.UserScope> GetUserScopeByUserIdAsync(int userId);
        Task<Models.UserScope> CreateUserScopeAsync(Models.UserScope userScope, string token);
        Task<Models.UserScope> UpdateUserScopeAsync(Models.UserScope userScope, int id, string token);
        Task<bool> DisableUserScopeAsync(int id, string token);
    }
}