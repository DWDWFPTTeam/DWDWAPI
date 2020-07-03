using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Validation
{
    public class RoomValidation
    {
        private readonly IRoomRepository roomRepository;

        public RoomValidation(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }
    }
}
