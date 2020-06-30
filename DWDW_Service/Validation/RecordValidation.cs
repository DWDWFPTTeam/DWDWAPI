using DWDW_Service.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Validation
{
    public class RecordValidation
    {
        private readonly IRecordRepository recordRepository;

        public RecordValidation(IRecordRepository recordRepository)
        {
            this.recordRepository = recordRepository;
        }

    }
}
