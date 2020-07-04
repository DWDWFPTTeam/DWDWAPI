using DWDW_API.Core.Constants;
using DWDW_API.Core.Entities;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DWDW_Service.Services
{
    public interface IShiftService: IBaseService<Shift>
    {
        IEnumerable<ShiftViewModel> GetAll();
        ShiftViewModel GetByID(int id);
        IEnumerable<ShiftViewModel> GetShiftByDate(DateTime date);
        ShiftViewModel CreateShift(int userID, ShiftCreateModel shift);
        ShiftViewModel UpdateShift(int userID, ShiftUpdateModel shift);
    }
    public class ShiftService : BaseService<Shift>, IShiftService
    {
        private readonly IShiftRepository shiftRepository;
        private readonly IArrangementRepository arrangementRepository;
        private readonly IRoomRepository roomRepository;
        public ShiftService(UnitOfWork unitOfWork, IShiftRepository shiftRepository
            , IArrangementRepository arrangementRepository
            , IRoomRepository roomRepository) : base(unitOfWork)
        {
            this.shiftRepository = shiftRepository;
            this.arrangementRepository = arrangementRepository;
            this.roomRepository = roomRepository;
        }

        public IEnumerable<ShiftViewModel> GetAll()
        {
            return shiftRepository.GetAll().Select(x => x.ToViewModel<ShiftViewModel>());
        }

        public ShiftViewModel GetByID(int id)
        {
            var result = new ShiftViewModel();
            var shift = shiftRepository.Find(id);
            if (shift != null)
            {
                result = shift.ToViewModel<ShiftViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.DEVICE_LIST_EMPTY);
            }
            return result;
        }

        public IEnumerable<ShiftViewModel> GetShiftByDate(DateTime date)
        {
            IEnumerable<ShiftViewModel> result = new List<ShiftViewModel>();
            var test = shiftRepository.GetShiftByDate(date);
            result = test.Select(x => x.ToViewModel<ShiftViewModel>());
            return result;
        }

        public ShiftViewModel CreateShift(int userID, ShiftCreateModel shift)
        {
            var result = new ShiftViewModel();
            bool CheckManagerLocation = arrangementRepository.CheckUserShift(userID, shift.ArrangementId);
            bool CheckRoomLocation = roomRepository.CheckRoomLocation(shift.RoomId, shift.ArrangementId);
            if (CheckManagerLocation == true && CheckRoomLocation == true)
            {
                shiftRepository.Add(new Shift
                {
                    ArrangementId = shift.ArrangementId,
                    Date = shift.Date,
                    RoomId = shift.RoomId,
                    ShiftType = shift.ShiftType,
                    IsActive = true
                });
                result = shiftRepository.GetLatest().ToViewModel<ShiftViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.INVALID_MANAGER);
            }
            return result;
        }

        //Not yet
        public ShiftViewModel UpdateShift(int userID, ShiftUpdateModel shift)
        {
            var result = new ShiftViewModel();
            var shiftU = shiftRepository.Find(shift.ShiftId);
            if (shiftU != null)
            {
                bool CheckManagerLocation = arrangementRepository.CheckUserShift(userID, shiftU.ArrangementId);
                if (CheckManagerLocation)
                {
                    bool CheckManagerUpdateLocation = arrangementRepository.CheckUserShift(userID, shift.ArrangementId);
                    bool CheckRoomUpdateLocation = roomRepository.CheckRoomLocation(shift.RoomId, shift.ArrangementId);
                    if (CheckManagerUpdateLocation == true && CheckRoomUpdateLocation == true)
                    {

                    }
                }
                else
                {
                    throw new BaseException(ErrorMessages.INVALID_MANAGER);
                }
            }
            else
            {
                throw new BaseException(ErrorMessages.SHIFT_IS_NOT_EXISTED);
            }
            return result;
        }


    }
}
