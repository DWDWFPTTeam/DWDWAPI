using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Validation
{
    public class RecordValidation
    {
        private readonly IRecordRepository recordRepository;
        private readonly UnitOfWork unitOfWorks;

        public RecordValidation(IRecordRepository recordRepository, UnitOfWork unitOfWorks)
        {
            this.recordRepository = recordRepository;
            this.unitOfWorks = unitOfWorks;
        }
    }
}
