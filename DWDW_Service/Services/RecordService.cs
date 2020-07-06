using DWDW_API.Core.Entities;
using DWDW_API.Core.ViewModels;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Services
{
    public interface IRecordService : IBaseService<Record>
    {
        RecordViewModel SaveRecord(int deviceId);
    }

    public class RecordService : BaseService<Record>, IRecordService
    {
        private readonly IRecordRepository recordRepository;
        public RecordService(UnitOfWork unitOfWork, IRecordRepository recordRepository) : base(unitOfWork)
        {
            this.recordRepository = recordRepository;
        }

        public RecordViewModel SaveRecord(int deviceId)
        {
            return null;
        }
    }
}
