using DWDW_API.Core.Constants;
using DWDW_API.Core.Entities;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DWDW_Service.Services
{
    public interface IRecordService : IBaseService<Record>
    {
        RecordViewModel SaveRecord(int deviceId, string image);
    }

    public class RecordService : BaseService<Record>, IRecordService
    {
        private readonly IRecordRepository recordRepository;
        public RecordService(UnitOfWork unitOfWork, IRecordRepository recordRepository) : base(unitOfWork)
        {
            this.recordRepository = recordRepository;
        }

        public RecordViewModel SaveRecord(int deviceId, string image)
        {
            if (unitOfWork.DeviceRepository.Find(deviceId) == null)
            {
                throw new BaseException(ErrorMessages.DEVICE_IS_NOT_EXISTED);
            }

            var record = new Record
            {
                DeviceId = deviceId,
                RecordDateTime = DateTime.Now,
                Image = image
            };

            recordRepository.Add(record);
            return record.ToViewModel<RecordViewModel>();
        }
        private void SendNotify(Record record)
        {
            string deviceToken = Constant.DEVICE_HOANG_MOBILETOKEN;

            
            
        }
    }
}
