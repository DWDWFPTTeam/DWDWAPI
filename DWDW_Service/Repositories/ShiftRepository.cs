using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface IShiftRepository: IBaseRepository<Shift>
    {
        List<Shift> GetShiftByDate(DateTime date);
        Shift GetLatest();


    }
    public class ShiftRepository : BaseRepository<Shift>, IShiftRepository
    {
        public ShiftRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public List<Shift> GetShiftByDate(DateTime date)
        {
            
            return this.dbContext.Set<Shift>().Where(x => x.Date == date.Date).ToList();
        }

        public Shift GetLatest()
        {
            return this.dbContext.Set<Shift>().OrderByDescending(x => x.ShiftId).First();
        }

    }

}
