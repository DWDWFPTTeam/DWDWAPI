using DWDW_API.Core.Infrastructure;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Services
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity
    {

    }

    public class BaseService<TEntity> where TEntity : BaseEntity
    {
        protected readonly UnitOfWork unitOfWork;

        public BaseService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
