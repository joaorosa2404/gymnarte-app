namespace GymnArteApp.Server.Repo.Interface
{
    public interface IBiometricData
    {
        Task<Models.BiometricData> GetBiometricDataByUserIdAsync(int id, string token); //Procura a ultima entrada de dados biométricos do user
        Task<IEnumerable<Models.BiometricData>> GetHistoryByUserIdAsync(int userId); //Procura o histórico de dados biométricos do user
        Task<Models.BiometricData> CreateBiometricDataAsync(Models.BiometricData user);
        Task<Models.BiometricData> UpdateBiometricDataAsync(int id, Models.BiometricData biometricData, string token);
        Task<bool> DeleteBiometricDataAsync(int id, string token);
        Task<bool> DisableBiometricDataAsync(int id, string token);
    }
}
