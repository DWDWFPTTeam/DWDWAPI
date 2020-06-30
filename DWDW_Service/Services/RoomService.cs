using DWDW_API.Core.Entities;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_Service.Services
{
    public interface IRoomService : IBaseService<Room>
    {

    }
    public class RoomService : BaseService<Room>, IRoomService
    {
        private readonly IRoomRepository roomRepository;
        public RoomService(UnitOfWork unitOfWork, IRoomRepository roomRepository) : base(unitOfWork)
        {
            this.roomRepository = roomRepository;
        }

    }
}
