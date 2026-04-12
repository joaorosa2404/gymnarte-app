using Microsoft.EntityFrameworkCore;
using GymnArteApp.Server.Repo.Interface;
using System.IdentityModel.Tokens.Jwt;

namespace GymnArteApp.Server.Repo
{
    public class BiometricData : IBiometricData
    {
        private readonly Data.GymDbContext _context;

        public BiometricData(Data.GymDbContext context)
        {
            _context = context;
        }

        public async Task<Models.BiometricData> GetBiometricDataByUserIdAsync(int id, string token)
        {
            try
            {
                var biometricData = await _context.BiometricData
                    .Where(b => b.UserId == id && b.IsActive)
                    .OrderByDescending(b => b.RecordDate)
                    .Include(b => b.User)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                return biometricData;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the latest biometric data for user ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Models.BiometricData>> GetHistoryByUserIdAsync(int userId)
        {
            try
            {
                var history = await _context.BiometricData
                    .Where(b => b.UserId == userId)
                    .OrderByDescending(b => b.RecordDate)
                    .AsNoTracking()
                    .ToListAsync();
                return history;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving biometric history for user ID {userId}.", ex);
            }
        }

        public async Task<Models.BiometricData> CreateBiometricDataAsync(Models.BiometricData biometric)
        {
            try
            {
                if (biometric.RecordDate == default)
                    biometric.RecordDate = DateTime.UtcNow;

                biometric.IsActive = true;

                _context.BiometricData.Add(biometric);
                await _context.SaveChangesAsync();

                return biometric;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the biometric record.", ex);
            }
        }

        public async Task<Models.BiometricData> UpdateBiometricDataAsync(int id, Models.BiometricData biometricData, string token)
        {
            try
            {
                var existing = await _context.BiometricData.FirstOrDefaultAsync(b => b.BiometricDataId == id);

                if (existing == null)
                {
                    throw new Exception($"Biometric record with ID {id} not found.");
                }

                existing.Weight = biometricData.Weight;
                existing.Height = biometricData.Height;
                existing.Age = biometricData.Age;
                existing.FatPercent = biometricData.FatPercent;
                existing.LeanMassPercent = biometricData.LeanMassPercent;
                existing.BodyWaterPercent = biometricData.BodyWaterPercent;
                existing.VisceralFat = biometricData.VisceralFat;
                existing.IsActive = biometricData.IsActive;
                existing.RecordDate = biometricData.RecordDate;

                _context.BiometricData.Update(existing);
                await _context.SaveChangesAsync();

                return existing;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating biometric record with ID {id}.", ex);
            }
        }

        public async Task<bool> DeleteBiometricDataAsync(int id, string token)
        {
            try
            {
                var biometric = await _context.BiometricData.FirstOrDefaultAsync(b => b.BiometricDataId == id);

                if (biometric == null)
                {
                    throw new Exception($"Biometric record with ID {id} not found.");
                }

                _context.BiometricData.Remove(biometric);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting biometric record with ID {id}.", ex);
            }
        }

        public async Task<bool> DisableBiometricDataAsync(int id, string token)
        {
            try
            {
                var biometric = await _context.BiometricData.FirstOrDefaultAsync(b => b.BiometricDataId == id);

                if (biometric == null)
                {
                    throw new Exception($"Biometric record with ID {id} not found.");
                }

                biometric.IsActive = false;

                _context.BiometricData.Update(biometric);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while disabling biometric record with ID {id}.", ex);
            }
        }

        private int? GetUserIdFromToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid" || c.Type == "sub")?.Value;

                return userIdClaim != null ? int.Parse(userIdClaim) : null;
            }
            catch
            {
                return null;
            }
        }
    }
}