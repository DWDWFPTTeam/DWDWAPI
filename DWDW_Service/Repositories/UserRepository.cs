using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DWDW_Service.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByUsernamePassword(string username, string password);
        User GetUserByUsername(string username);
        IEnumerable<User> GetWorkerFromLocation(int locationId);
    }
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public User GetUserByUsername(string username)
        {
            return this.dbContext.Set<User>().FirstOrDefault(x => x.UserName.Trim().ToLower()
                                                            .Equals(username.Trim().ToLower()));
        }

        public async Task<User> GetUserByUsernamePassword(string username, string password)
        {

            var user = await this.dbContext.Set<User>().FirstOrDefaultAsync(x => x.UserName.Equals(username)
                                                                     && x.Password.Equals(password));
            return user;
        }

        public IEnumerable<User> GetWorkerFromLocation(int locationId)
        {
            IEnumerable<User> result = new List<User>();
            result = dbContext.Set<User>().Where(x => x.Arrangement.Any(y => y.LocationId == locationId
            && y.IsActive == true) && x.RoleId == 3).ToList();
            return result;
        }
    }
}
