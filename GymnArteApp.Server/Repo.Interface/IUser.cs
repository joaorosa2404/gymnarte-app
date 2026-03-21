using GymnArteApp.Server.Models;
namespace GymnArteApp.Server.Repo.Interface
{
    public interface IUser
    {
        Task<IEnumerable<Models.User>> GetAllUsersAsync();
        Task<Models.User> GetUserByIdAsync(int id);
        Task<IEnumerable<Models.User>> GetUsersByScopeAsync(int scopeId);
        Task<Models.User> CreateUserAsync(Models.User user);
        Task<Models.User> UpdateUserAsync(int id, Models.User user, string token);
        Task<bool> DeleteUserAsync(int id, string token);
    }
}
