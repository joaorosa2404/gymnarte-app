namespace GymnArteApp.Server.Repo.Interface
{
    public interface IBiometricData
    {
        Task<Models.BiometricData> GetBiometricDataByUserIdAsync(int id, string token);
        Task<IEnumerable<Models.BiometricData>> GetHistoryByUserIdAsync();
        Task<Models.BiometricData> CreateBiometricDataAsync(Models.BiometricData user);
        Task<Models.BiometricData> UpdateBiometricDataAsync(int id, Models.BiometricData biometricData, string token);
        Task<bool> DeleteBiometricDataAsync(int id, string token);
    }
}
