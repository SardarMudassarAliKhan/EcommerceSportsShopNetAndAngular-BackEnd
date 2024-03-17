using Core.Entities.Identity;
using Core.Interfaces;
using Infrastracture.Data;
using Microsoft.Extensions.Logging;

namespace Infrastracture.Repositories
{
    public class UsersRepository : IUsersRepository<AppUser>, IDisposable
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger _logger;

        public UsersRepository(ILogger<AppUser> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        // Create a new user
        public async Task<AppUser> Create(AppUser appuser)
        {
            try
            {
                if (appuser != null)
                {
                    var obj = _appDbContext.Add<AppUser>(appuser);
                    await _appDbContext.SaveChangesAsync();
                    return obj.Entity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user");
                throw;
            }
        }

        // Delete an existing user
        public void Delete(AppUser appuser)
        {
            try
            {
                if (appuser != null)
                {
                    var obj = _appDbContext.Remove(appuser);
                    if (obj != null)
                    {
                        _appDbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user");
                throw;
            }
        }

        // Retrieve all users
        public IEnumerable<AppUser> GetAll()
        {
            try
            {
                var obj = _appDbContext.AppUsers.ToList();
                if (obj != null)
                    return obj;
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all users");
                throw;
            }
        }

        // Retrieve user by ID
        public AppUser GetById(string Id)
        {
            try
            {
                if (Id != null)
                {
                    var Obj = _appDbContext.AppUsers.FirstOrDefault(x => x.Id == Id);
                    if (Obj != null)
                        return Obj;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user by Id");
                throw;
            }
        }

        // Update an existing user
        public void Update(AppUser appuser)
        {
            try
            {
                if (appuser != null)
                {
                    var obj = _appDbContext.Update(appuser);
                    if (obj != null)
                        _appDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user");
                throw;
            }
        }

        public AppUser GetByUserName(string userName)
        {
            try
            {
                if (userName != null)
                {
                    var obj = _appDbContext.AppUsers.FirstOrDefault(x => x.UserName == userName);
                    if (obj != null)
                        return obj;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user");
                throw;
            }
        }

        public AppUser GetByEmail(string email)
        {
            try
            {
                if (email != null)
                {
                    var obj = _appDbContext.AppUsers.FirstOrDefault(x => x.Email == email);
                    if (obj != null)
                        return obj;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user");
                throw;
            }
        }

        // Dispose the repository
        public void Dispose()
        {
            _appDbContext.Dispose();
        }


    }
}