namespace GymnArteApp.Server.Repo.Interface
{
    public interface IUserScope
    {

        Task<IEnumerable<Models.UserScope>> GetAllUserScopesAsync();
        Task<Models.UserScope> GetUserScopeByIdAsync(int id);
        Task<Models.UserScope> CreateUserScopeAsync(Models.UserScope userScope);
        Task<Models.UserScope> UpdateUserScopeAsync(int id, Models.UserScope userScope);
        Task<bool> DeleteUserScopeAsync(int id);
    }
}
