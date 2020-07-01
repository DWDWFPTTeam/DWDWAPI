using DWDW_API.Core.Entities;
using DWDW_API.Core.ViewModels;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Services
{
    public interface ILocationService : IBaseService<Location>
    {

    }
    public class LocationService : BaseService<Location>, ILocationService
    {
        private readonly ILocationRepository locationRepository;
        public LocationService(UnitOfWork unitOfWork, ILocationRepository locationRepository) : base(unitOfWork)
        {
            this.locationRepository = locationRepository;
        }

    }
}
