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
    public interface IShiftService : IBaseService<Shift>
    {
        IEnumerable<ShiftViewModel> GetAll();
        ShiftViewModel GetByID(int id);
        IEnumerable<ShiftViewModel> GetShiftByDate(DateTime date);
        IEnumerable<ShiftViewModel> GetShiftManager(int userID);
        IEnumerable<ShiftViewModel> GetShiftFromLocationByDate(int locationID, DateTime date);
        IEnumerable<ShiftViewModel> GetShiftFromLocationByDateManager(int userID, int locationID, DateTime date);
        IEnumerable<ShiftViewModel> GetShiftFromLocationByDateWorker(int userID, int locationID, DateTime date);
        IEnumerable<ShiftViewModel> GetShiftByDateManager(int userID, DateTime date);
        IEnumerable<ShiftViewModel> GetShiftWorker(int userID);
        ShiftViewModel CreateShift(int userID, ShiftCreateModel shift);
        ShiftViewModel UpdateShift(int userID, ShiftUpdateModel shift);
        ShiftViewModel UpdateShiftActive(int userID, ShiftActiveModel shift);
    }
    public class ShiftService : BaseService<Shift>, IShiftService
    {
        private readonly IShiftRepository shiftRepository;
        public ShiftService(UnitOfWork unitOfWork, IShiftRepository shiftRepository
            , IRoomRepository roomRepository) : base(unitOfWork)
        {
            this.shiftRepository = shiftRepository;
        }

        public IEnumerable<ShiftViewModel> GetAll()
        {
            var result = shiftRepository.GetAll().Select(x => x.ToViewModel<ShiftViewModel>()).ToList();
            foreach (var element in result)
            {
                int? arrangementID = element.ArrangementId;
                int? roomID = element.RoomId;
                element.RoomCode = shiftRepository.GetRoomCode(roomID);
                element.UserName = shiftRepository.GetUsername(arrangementID);
                element.UserId = shiftRepository.GetWorkerID(arrangementID);
            }
            return result;
        }

        public ShiftViewModel GetByID(int id)
        {
            var result = new ShiftViewModel();
            var shift = shiftRepository.Find(id);
            if (shift == null)
            {
                throw new BaseException(ErrorMessages.SHIFT_IS_NOT_EXISTED);
            }
            result = shift.ToViewModel<ShiftViewModel>();
            int? arrangementID = result.ArrangementId;
            int? roomID = result.RoomId;
            result.RoomCode = shiftRepository.GetRoomCode(roomID);
            result.UserName = shiftRepository.GetUsername(arrangementID);
            result.UserId = shiftRepository.GetWorkerID(arrangementID);
            return result;
        }

        public IEnumerable<ShiftViewModel> GetShiftByDate(DateTime date)
        {
            IEnumerable<ShiftViewModel> result = new List<ShiftViewModel>();
            var test = shiftRepository.GetShiftByDate(date);
            result = test.Select(x => x.ToViewModel<ShiftViewModel>()).ToList();
            foreach (var element in result)
            {
                int? arrangementID = element.ArrangementId;
                int? roomID = element.RoomId;
                element.RoomCode = shiftRepository.GetRoomCode(roomID);
                element.UserName = shiftRepository.GetUsername(arrangementID);
                element.UserId = shiftRepository.GetWorkerID(arrangementID);

            }
            return result;
        }

        public IEnumerable<ShiftViewModel> GetShiftManager(int userID)
        {
            IEnumerable<ShiftViewModel> result = new List<ShiftViewModel>();
            var arrangementRepo = unitOfWork.ArrangementRepository;
            List<int?> arrangementRelated = arrangementRepo.GetArrangementBelongToManager(userID);
            var shiftManager = shiftRepository.GetShiftSubAccount(arrangementRelated);
            result = shiftManager.Select(x => x.ToViewModel<ShiftViewModel>()).ToList();
            foreach (var element in result)
            {
                int? arrangementID = element.ArrangementId;
                int? roomID = element.RoomId;
                element.RoomCode = shiftRepository.GetRoomCode(roomID);
                element.UserName = shiftRepository.GetUsername(arrangementID);
                element.UserId = shiftRepository.GetWorkerID(arrangementID);

            }
            return result;
        }

        public IEnumerable<ShiftViewModel> GetShiftFromLocationByDate(int locationID, DateTime date)
        {
            IEnumerable<ShiftViewModel> result = new List<ShiftViewModel>();
            var arrangementRepo = unitOfWork.ArrangementRepository;
            var locationRepo = unitOfWork.LocationRepository;
            var location = locationRepo.Find(locationID);
            if (location == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            var shiftLocation = shiftRepository.GetShiftFromLocation(locationID);
            var shiftList = shiftLocation.Where(x => x.Date == date.Date).ToList();
            foreach (var element in shiftList)
            {
                int? arrangementID = element.ArrangementId;
                int? roomID = element.RoomId;
                element.RoomCode = shiftRepository.GetRoomCode(roomID);
                element.UserName = shiftRepository.GetUsername(arrangementID);
                element.UserId = shiftRepository.GetWorkerID(arrangementID);
            }
            result = shiftList.Where(x => x.IsActive == true).ToList();
            return result;
        }

        public IEnumerable<ShiftViewModel> GetShiftFromLocationByDateManager(int userID, int locationID, DateTime date)
        {
            IEnumerable<ShiftViewModel> result = new List<ShiftViewModel>();
            var arrangementRepo = unitOfWork.ArrangementRepository;
            var arrangement = arrangementRepo.CheckLocationManagerWorker(userID, locationID);
            if (arrangement == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_BELONG_TO_MANAGER);
            }
            var shiftLocation = shiftRepository.GetShiftFromLocation(locationID);
            var shiftList = shiftLocation.Where(x => x.Date == date.Date).ToList();
            foreach (var element in shiftList)
            {
                int? arrangementID = element.ArrangementId;
                int? roomID = element.RoomId;
                element.RoomCode = shiftRepository.GetRoomCode(roomID);
                element.UserName = shiftRepository.GetUsername(arrangementID);
                element.UserId = shiftRepository.GetWorkerID(arrangementID);

            }
            result = shiftList.Where(x => x.IsActive == true).ToList();
            return result;
        }

        public IEnumerable<ShiftViewModel> GetShiftFromLocationByDateWorker(int userID, int locationID, DateTime date)
        {
            IEnumerable<ShiftViewModel> result = new List<ShiftViewModel>();
            var arrangementRepo = unitOfWork.ArrangementRepository;
            var arrangement = arrangementRepo.CheckLocationManagerWorker(userID, locationID);
            if (arrangement == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_BELONG_TO_WORKER);
            }
            var shiftLocation = shiftRepository.GetShiftFromLocationWorker(userID, locationID);
            var shiftList = shiftLocation.Where(x => x.Date == date.Date).ToList();
            foreach (var element in shiftList)
            {
                int? arrangementID = element.ArrangementId;
                int? roomID = element.RoomId;
                element.RoomCode = shiftRepository.GetRoomCode(roomID);
                element.UserName = shiftRepository.GetUsername(arrangementID);
                element.UserId = shiftRepository.GetWorkerID(arrangementID);

            }
            result = shiftList.Where(x => x.IsActive == true).ToList();
            return result;
        }

        public IEnumerable<ShiftViewModel> GetShiftByDateManager(int userID, DateTime date)
        {
            IEnumerable<ShiftViewModel> result = new List<ShiftViewModel>();
            var availableShift = GetShiftManager(userID);
            //result = availableShift.Select(x => x.Date == date.Date).ToList();
            result = availableShift.Where(x => x.Date == date.Date).ToList();
            foreach (var element in result)
            {
                int? arrangementID = element.ArrangementId;
                int? roomID = element.RoomId;
                element.RoomCode = shiftRepository.GetRoomCode(roomID);
                element.UserName = shiftRepository.GetUsername(arrangementID);
                element.UserId = shiftRepository.GetWorkerID(arrangementID);

            }
            return result;
        }

        public IEnumerable<ShiftViewModel> GetShiftWorker(int userID)
        {
            IEnumerable<ShiftViewModel> result = new List<ShiftViewModel>();
            var arrangementRepo = unitOfWork.ArrangementRepository;
            List<int?> arrangementRelated = arrangementRepo.GetArrangementBelongToWorker(userID);
            var shiftManager = shiftRepository.GetShiftSubAccount(arrangementRelated);
            result = shiftManager.Select(x => x.ToViewModel<ShiftViewModel>()).ToList();
            foreach (var element in result)
            {
                int? arrangementID = element.ArrangementId;
                int? roomID = element.RoomId;
                element.RoomCode = shiftRepository.GetRoomCode(roomID);
                element.UserName = shiftRepository.GetUsername(arrangementID);
                element.UserId = shiftRepository.GetWorkerID(arrangementID);

            }
            return result;
        }

        public ShiftViewModel CreateShift(int userID, ShiftCreateModel shift)
        {
            ShiftViewModel result;
            var arrangementRepo = this.unitOfWork.ArrangementRepository;
            var roomRepo = this.unitOfWork.RoomRepository;
            var locationRepo = this.unitOfWork.LocationRepository;
            var room = roomRepo.Find(shift.RoomId);
            if (room == null)
            {
                throw new BaseException(ErrorMessages.ROOM_IS_NOT_EXISTED);
            }
            var location = locationRepo.Find(room.LocationId);
            if (location == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            var arrangementWorker = arrangementRepo.GetArrangementOfUserInThisLocation(shift.WorkerID, location.LocationId);
            if (arrangementWorker == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_BELONG_TO_WORKER);
            }
            if (shift.Date > arrangementWorker.EndDate || shift.Date < DateTime.Now)
            {
                throw new BaseException(ErrorMessages.SHIFT_DATE_INVALID);
            }
            bool CheckManagerLocation = arrangementRepo.CheckUserShift(userID, arrangementWorker.ArrangementId);
            if (CheckManagerLocation == false)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_BELONG_TO_MANAGER);
            }
            bool shiftCheck = shiftRepository.CheckExistedShift(arrangementWorker.ArrangementId, shift.RoomId, shift.Date);
            if (shiftCheck == false)
            {
                throw new BaseException(ErrorMessages.SHIFT_IS_USED);
            }
            shiftRepository.Add(new Shift
            {
                ArrangementId = arrangementWorker.ArrangementId,
                Date = shift.Date,
                RoomId = shift.RoomId,
                IsActive = true
            });
            result = shiftRepository.GetLatest().ToViewModel<ShiftViewModel>();
            result.RoomCode = shiftRepository.GetRoomCode(result.RoomId);
            result.UserName = shiftRepository.GetUsername(result.ArrangementId);
            result.UserId = shiftRepository.GetWorkerID(result.ArrangementId);
            return result;
        }

        public ShiftViewModel UpdateShift(int userID, ShiftUpdateModel shift)
        {
            ShiftViewModel result;
            var shiftU = shiftRepository.Find(shift.ShiftId);
            if (shiftU == null)
            {
                throw new BaseException(ErrorMessages.SHIFT_IS_NOT_EXISTED);
            }

            var arrangementRepo = this.unitOfWork.ArrangementRepository;
            var roomRepo = this.unitOfWork.RoomRepository;
            var locationRepo = this.unitOfWork.LocationRepository;
            var room = roomRepo.Find(shift.RoomId);
            if (room == null)
            {
                throw new BaseException(ErrorMessages.ROOM_IS_NOT_EXISTED);
            }
            var location = locationRepo.Find(room.LocationId);
            if (location == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            //Xac dinh xem shift dang dinh update co thuoc manager khong
            var arrangementWorker = arrangementRepo.GetArrangementOfUserInThisLocation(shift.WorkerID, location.LocationId);
            if (arrangementWorker == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_BELONG_TO_WORKER);
            }
            if (shift.Date > arrangementWorker.EndDate || shift.Date < DateTime.Now)
            {
                throw new BaseException(ErrorMessages.SHIFT_DATE_INVALID);
            }
            bool CheckManagerLocation = arrangementRepo.CheckUserShift(userID, arrangementWorker.ArrangementId);
            if (CheckManagerLocation == false)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_BELONG_TO_MANAGER);
            }

            //Kiem tra thong tin moi nhap vao
            bool CheckManagerUpdateLocation = arrangementRepo.CheckUserShift(userID, arrangementWorker.ArrangementId);
            if (CheckManagerUpdateLocation == false)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_BELONG_TO_MANAGER);
            }

            bool shiftCheck = shiftRepository.CheckExistedShift(arrangementWorker.ArrangementId, shift.RoomId, shift.Date);
            if (shiftCheck == false)
            {
                throw new BaseException(ErrorMessages.SHIFT_IS_USED);
            }

            shiftU.ArrangementId = arrangementWorker.ArrangementId;
            shiftU.Date = shift.Date;
            shiftU.RoomId = shift.RoomId;
            shiftRepository.Update(shiftU);
            result = shiftU.ToViewModel<ShiftViewModel>();
            result.RoomCode = shiftRepository.GetRoomCode(result.RoomId);
            result.UserName = shiftRepository.GetUsername(result.ArrangementId);
            result.UserId = shiftRepository.GetWorkerID(result.ArrangementId);

            return result;
        }

        public ShiftViewModel UpdateShiftActive(int userID, ShiftActiveModel shift)
        {
            ShiftViewModel result;
            var shiftActive = shiftRepository.Find(shift.ShiftId);
            if (shiftActive == null)
            {
                throw new BaseException(ErrorMessages.SHIFT_IS_NOT_EXISTED);
            }

            var arrangementRepo = unitOfWork.ArrangementRepository;
            bool CheckRelated = arrangementRepo.CheckUserShift(userID, shiftActive.ArrangementId);
            if (CheckRelated != true)
            {
                throw new BaseException(ErrorMessages.INVALID_MANAGER);
            }

            //Neu set cho shift nay tu Disable sang Active thi phai disable shift giong nhu vay.
            if (shift.IsActive == true)
            {
                bool shiftCheck = shiftRepository.CheckExistedShift(shiftActive.ArrangementId, shiftActive.RoomId, shiftActive.Date);
                if (shiftCheck == false)
                {
                    throw new BaseException(ErrorMessages.SHIFT_IS_USED);
                }
                shiftActive.IsActive = shift.IsActive;
                shiftRepository.Update(shiftActive);
                result = shiftActive.ToViewModel<ShiftViewModel>();
                result.RoomCode = shiftRepository.GetRoomCode(result.RoomId);
                result.UserName = shiftRepository.GetUsername(result.ArrangementId);
                result.UserId = shiftRepository.GetWorkerID(result.ArrangementId);
            }
            else
            {
                shiftActive.IsActive = shift.IsActive;
                shiftRepository.Update(shiftActive);
                result = shiftActive.ToViewModel<ShiftViewModel>();
                result.RoomCode = shiftRepository.GetRoomCode(result.RoomId);
                result.UserName = shiftRepository.GetUsername(result.ArrangementId);
                result.UserId = shiftRepository.GetWorkerID(result.ArrangementId);
            }
            return result;
        }
    }
}

