using DWDW_API.Core.Constants;
using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DWDW_Service.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User CheckUserNameExisted(string userName);
        User GetUserByUsernamePassword(string username, string password);
        User GetUserByUsername(string username);
        IEnumerable<User> GetWorkerFromLocation(int locationId);
        IEnumerable<User> GetUserFromLocation(int locationId);
    }
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public User CheckUserNameExisted(string userName)
        {
            return dbContext.Set<User>().FirstOrDefault(x => x.UserName == userName && x.IsActive == true);
        }

        public User GetUserByUsername(string username)
        {
            return this.dbContext.Set<User>().FirstOrDefault(x => x.UserName.Trim().ToLower()
                                                            .Equals(username.Trim().ToLower()));
        }

        public User GetUserByUsernamePassword(string username, string password)
        {

            var user = this.dbContext.Set<User>().FirstOrDefault(x => x.UserName.Equals(username)
                                                                     && x.Password.Equals(password));
            return user;
        }

        public IEnumerable<User> GetWorkerFromLocation(int locationId)
        {
            IEnumerable<User> result = new List<User>();
            result = dbContext.Set<User>().Where(x => x.Arrangement.Any(y => y.LocationId == locationId
            && y.IsActive == true) && x.RoleId == int.Parse(Constant.WORKER)).ToList();
            return result;
        }

        public IEnumerable<User> GetUserFromLocation(int locationId)
        {
            IEnumerable<User> result = new List<User>();
            result = dbContext.Set<User>().Where(x => x.Arrangement.Any(y => y.LocationId == locationId
            && y.IsActive == true)).ToList();
            return result;
        }
    }
}
