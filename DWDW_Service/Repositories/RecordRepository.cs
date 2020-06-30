using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface IRecordRepository : IBaseRepository<Record>
    {

    }
    public class RecordRepository : BaseRepository<Record>, IRecordRepository
    {
        public RecordRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
