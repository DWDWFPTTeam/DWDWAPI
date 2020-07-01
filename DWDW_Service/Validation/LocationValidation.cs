using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Validation
{
    public class LocationValidation
    {
        private readonly ILocationRepository locationRepository;

        public LocationValidation(ILocationRepository locationRepository)
        {
            this.locationRepository = locationRepository;
        }
    }
}
