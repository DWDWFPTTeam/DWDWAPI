using DWDW_API.Core.Entities;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Services
{
    public interface IDeviceService: IBaseService<Device>
    {

    } 
    public class DeviceService : BaseService<Device>, IDeviceService
    {
        private readonly IDeviceRepository deviceRepository;
        public DeviceService(UnitOfWork unitOfWork, IDeviceRepository deviceRepository) : base(unitOfWork)
        {
            this.deviceRepository = deviceRepository;
        }
    }
}
