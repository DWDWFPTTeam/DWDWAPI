﻿using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Validation
{
    public class ShiftValidation
    {
        private readonly IShiftRepository shiftRepository;
        private readonly UnitOfWork unitOfWorks;

        public ShiftValidation(IShiftRepository shiftRepository, UnitOfWork unitOfWorks)
        {
            this.shiftRepository = shiftRepository;
            this.unitOfWorks = unitOfWorks;
        }
    }
}
