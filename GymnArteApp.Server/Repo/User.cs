using GymnArteApp.Server.Repo.Interface;
using Microsoft.EntityFrameworkCore;

namespace GymnArteApp.Server.Repo
{
    public class User : IUser
    {
        private readonly Data.GymDbContext _context;

        public User(Data.GymDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.User>> GetAllUsersAsync()
        {
            try
            {
                var users = await _context.Users.AsNoTracking().ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving users.", ex);
            }
        }

        public async Task<Models.User> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the user with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Models.User>> GetUsersByScopeAsync(int scopeId)
        {
            try
            {
                var users = await _context.Users.AsNoTracking().Where(u => (int)u.UserScopeId == scopeId).ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving users with scope ID {scopeId}.", ex);
            }
        }

        public async Task<Models.User> CreateUserAsync(Models.User user)
        {
            try
            {
                user.CreationDate = DateTime.UtcNow;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the user.", ex);
            }
        }

        public async Task<Models.User> UpdateUserAsync(int id, Models.User user, string token)
        {
            try
            {
                var existingUser = await this.GetUserByIdAsync(id);
                if (existingUser == null)
                {
                    throw new Exception($"User with ID {id} not found.");
                }
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.BirthDate = user.BirthDate;
                existingUser.Gender = user.Gender;
                existingUser.Name = user.Name;
                existingUser.Surname = user.Surname;
                existingUser.Phone = user.Phone;
                existingUser.UpdatedDate = DateTime.UtcNow;
                existingUser.UpdatedUserId = user.UserId;

                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();

                return existingUser;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the user with ID {id}.", ex);
            }
        }

        public Task<bool> DeleteUserAsync(int id, string token)
        {
            throw new NotImplementedException();
        }


    }
}
