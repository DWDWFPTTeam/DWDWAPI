using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface IShiftRepository: IBaseRepository<Shift>
    {

    }
    public class ShiftRepository : BaseRepository<Shift>, IShiftRepository
    {
        public ShiftRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
