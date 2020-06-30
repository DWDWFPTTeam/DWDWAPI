using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWDW_Service.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByUsernamePassword(string username, string password);
        User GetUserByUsername(string username);
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
    }
}
