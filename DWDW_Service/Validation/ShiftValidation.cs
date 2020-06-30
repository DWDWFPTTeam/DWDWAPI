using DWDW_Service.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Validation
{
    public class ShiftValidation
    {
        private readonly IShiftRepository shiftRepository;

        public ShiftValidation(IShiftRepository shiftRepository)
        {
            this.shiftRepository = shiftRepository;
        }
    }
}
