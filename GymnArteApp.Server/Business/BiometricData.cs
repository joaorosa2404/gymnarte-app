using GymnArteApp.Server.Business.Interface;

namespace GymnArteApp.Server.Business
{
    public class BiometricData : IBiometricData
    {
        private readonly Repo.Interface.IBiometricData _biometricDataRepo;

        public BiometricData(Repo.Interface.IBiometricData biometricDataRepo)
        {
            _biometricDataRepo = biometricDataRepo;
        }

        public async Task<Models.BiometricData> GetBiometricDataByUserIdAsync(int id, string token)
        {
            try
            {
                var latestData = await _biometricDataRepo.GetBiometricDataByUserIdAsync(id, token);
                return latestData;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while retrieving latest biometric data for user ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Models.BiometricData>> GetHistoryByUserIdAsync(int userId)
        {
            try
            {
                var history = await _biometricDataRepo.GetHistoryByUserIdAsync(userId);
                return history;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while retrieving biometric history for user ID {userId}.", ex);
            }
        }

        public async Task<Models.BiometricData> CreateBiometricDataAsync(Models.BiometricData user)
        {
            try
            {
                var newData = await _biometricDataRepo.CreateBiometricDataAsync(user);
                return newData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business layer while creating biometric record.", ex);
            }
        }

        public async Task<Models.BiometricData> UpdateBiometricDataAsync(int id, Models.BiometricData biometricData, string token)
        {
            try
            {
                var updatedData = await _biometricDataRepo.UpdateBiometricDataAsync(id, biometricData, token);
                return updatedData;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while updating biometric record ID {id}.", ex);
            }
        }

        public async Task<bool> DeleteBiometricDataAsync(int id, string token)
        {
            try
            {
                var result = await _biometricDataRepo.DeleteBiometricDataAsync(id, token);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while deleting biometric record ID {id}.", ex);
            }
        }

        public async Task<bool> DisableBiometricDataAsync(int id, string token)
        {
            try
            {
                var result = await _biometricDataRepo.DisableBiometricDataAsync(id, token);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while disabling biometric record ID {id}.", ex);
            }
        }
    }
}