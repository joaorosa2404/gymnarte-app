namespace GymnArteApp.Server.Services.Interface
{
    public interface ITokenService
    {
        string GenerateToken(Models.User user, Models.Enums.UserRole role);
    }
}
