using DWDW_API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Repositories
{
    public interface IArrangementRepository: IBaseRepository<Arrangement>
    {

    }
    public class ArrangementRepository : BaseRepository<Arrangement>, IArrangementRepository
    {
        public ArrangementRepository(DbContext dbContext) : base(dbContext)
        {

        }

    }
}
