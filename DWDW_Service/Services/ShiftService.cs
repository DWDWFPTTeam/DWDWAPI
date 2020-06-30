using DWDW_API.Core.Entities;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Services
{
    public interface IShiftService: IBaseService<Shift>
    {

    }
    public class ShiftService : BaseService<Shift>, IShiftService
    {
        private readonly IShiftRepository shiftRepository;
        public ShiftService(UnitOfWork unitOfWork, IShiftRepository shiftRepository) : base(unitOfWork)
        {
            this.shiftRepository = shiftRepository;
        }

    }
}
