﻿using DWDW_API.Core.Constants;
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
        ShiftViewModel UpdateShift(int userID, int locationID, ShiftUpdateModel shift);
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

            return shiftRepository.GetAll().Select(x =>
            {
                var shiftViewModel = x.ToViewModel<ShiftViewModel>();
                shiftViewModel.RoomCode = shiftRepository.GetRoomCode(x.RoomId);
                shiftViewModel.UserName = shiftRepository.GetUsername(x.ArrangementId);
                shiftViewModel.UserId = shiftRepository.GetWorkerID(x.ArrangementId);
                return shiftViewModel;
            });
        }

        public ShiftViewModel GetByID(int id)
        {
            var result = new ShiftViewModel();
            var shift = shiftRepository.Find(id);
            if (shift != null)
            {
                result = shift.ToViewModel<ShiftViewModel>();
                int? arrangementID = result.ArrangementId;
                int? roomID = result.RoomId;
                result.RoomCode = shiftRepository.GetRoomCode(roomID);
                result.UserName = shiftRepository.GetUsername(arrangementID);
                result.UserId = shiftRepository.GetWorkerID(arrangementID);
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
            if (location != null)
            {
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
            }
            else
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            return result;
        }

        public IEnumerable<ShiftViewModel> GetShiftFromLocationByDateManager(int userID, int locationID, DateTime date)
        {
            IEnumerable<ShiftViewModel> result = new List<ShiftViewModel>();
            var arrangementRepo = unitOfWork.ArrangementRepository;
            var arrangement = arrangementRepo.CheckLocationManagerWorker(userID, locationID);
            if (arrangement != null)
            {
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
            }
            else
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_BELONG_TO_MANAGER);
            }
            return result;
        }

        public IEnumerable<ShiftViewModel> GetShiftFromLocationByDateWorker(int userID, int locationID, DateTime date)
        {
            IEnumerable<ShiftViewModel> result = new List<ShiftViewModel>();
            var arrangementRepo = unitOfWork.ArrangementRepository;
            var arrangement = arrangementRepo.CheckLocationManagerWorker(userID, locationID);
            if (arrangement != null)
            {
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
            }
            else
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_BELONG_TO_WORKER);
            }
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

        //public ShiftViewModel CreateShift(int managerId, int locationID, ShiftCreateModel shift)
        //{

        //    ShiftViewModel result;
        //    var arrangementRepo = this.unitOfWork.ArrangementRepository;
        //    var roomRepo = this.unitOfWork.RoomRepository;
        //    var arrangementWorker = arrangementRepo.GetArrangementOfUserInThisLocation(shift.WorkerID, locationID);
        //    if (arrangementWorker == null)
        //    {
        //        throw new BaseException(ErrorMessages.ARRANGEMENT_IS_NOT_EXISTED);
        //    }
        //    bool CheckManagerLocation = arrangementRepo.CheckUserShift(managerId, arrangementWorker.ArrangementId);
        //    if (!CheckManagerLocation)
        //    {
        //        throw new BaseException(ErrorMessages.INVALID_MANAGER);
        //    }
        //    bool CheckRoomLocation = roomRepo.CheckRoomLocation(shift.RoomId, arrangementWorker.ArrangementId);
        //    if (!CheckRoomLocation)
        //    {
        //        throw new BaseException(ErrorMessages.ROOM_IS_EXISTED);
        //    }

        //    shiftRepository.DisableOldSameShift(arrangementWorker.ArrangementId, shift.RoomId, shift.Date);
        //    shiftRepository.Add(new Shift
        //    {
        //        ArrangementId = arrangementWorker.ArrangementId,
        //        Date = shift.Date,
        //        RoomId = shift.RoomId,
        //        IsActive = true
        //    });
        //    result = shiftRepository.GetLatest().ToViewModel<ShiftViewModel>();
        //    result.RoomCode = shiftRepository.GetRoomCode(result.RoomId);
        //    result.UserName = shiftRepository.GetUsername(result.ArrangementId);
        //    result.UserId = shiftRepository.GetWorkerID(result.ArrangementId);

        //    return result;
        //}

        public ShiftViewModel UpdateShift(int userID, int locationID, ShiftUpdateModel shift)
        {
            var result = new ShiftViewModel();
            var shiftU = shiftRepository.Find(shift.ShiftId);
            if (shiftU != null)
            {
                var arrangementRepo = this.unitOfWork.ArrangementRepository;
                var roomRepo = this.unitOfWork.RoomRepository;
                bool CheckManagerLocation = arrangementRepo.CheckUserShift(userID, shiftU.ArrangementId);
                var arrangementWorker = arrangementRepo.GetArrangementOfUserInThisLocation(shift.WorkerID, locationID);
                if (CheckManagerLocation && arrangementWorker != null)
                {
                    bool CheckManagerUpdateLocation = arrangementRepo.CheckUserShift(userID, arrangementWorker.ArrangementId);
                    bool CheckRoomUpdateLocation = roomRepo.CheckRoomLocation(shift.RoomId, arrangementWorker.ArrangementId);
                    if (CheckManagerUpdateLocation == true && CheckRoomUpdateLocation == true)
                    {
                        shiftU.ArrangementId = arrangementWorker.ArrangementId;
                        shiftU.Date = shift.Date;
                        shiftU.RoomId = shift.RoomId;
                        shiftRepository.DisableOldSameShift(shiftU.ArrangementId, shiftU.RoomId, shiftU.Date);
                        shiftRepository.Update(shiftU);
                        result = shiftU.ToViewModel<ShiftViewModel>();
                        result.RoomCode = shiftRepository.GetRoomCode(result.RoomId);
                        result.UserName = shiftRepository.GetUsername(result.ArrangementId);
                        result.UserId = shiftRepository.GetWorkerID(result.ArrangementId);
                    }
                    else
                    {
                        throw new BaseException(ErrorMessages.INVALID_MANAGER);
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

        public ShiftViewModel UpdateShiftActive(int userID, ShiftActiveModel shift)
        {
            var result = new ShiftViewModel();
            var shiftActive = shiftRepository.Find(shift.ShiftId);
            if (shiftActive != null)
            {
                var arrangementRepo = unitOfWork.ArrangementRepository;
                bool CheckRelated = arrangementRepo.CheckUserShift(userID, shiftActive.ArrangementId);
                if (CheckRelated == true)
                {
                    //Neu set cho shift nay tu Disable sang Active thi phai disable shift giong nhu vay.
                    if (shift.IsActive == true)
                    {
                        shiftRepository.DisableOldSameShift(shiftActive.ArrangementId, shiftActive.RoomId, shiftActive.Date);
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
                    throw new BaseException(ErrorMessages.INVALID_MANAGER);
                }
            }
            else
            {
                throw new BaseException(ErrorMessages.SHIFT_IS_NOT_EXISTED);
            }
            return result;
        }

        public ShiftViewModel CreateShift(int managerID, ShiftCreateModel shiftCreated)
        {
            if(shiftCreated.Date.CompareTo(DateTime.Now.Date) < 0)
            {
                throw new BaseException(ErrorMessages.DATE_INVALID);
            }

            var managerArrangements = this.unitOfWork.ArrangementRepository.Get(arr => arr.LocationId == shiftCreated.LocationId
                                                                                && arr.UserId == managerID 
                                                                                && arr.IsActive == true, null, "")
                                                                                .FirstOrDefault();
            if(managerArrangements == null)
            {
                throw new BaseException(ErrorMessages.ARRANGEMENT_IS_NOT_EXISTED);
            }
            var workerArrangements = this.unitOfWork.ArrangementRepository.Get(arr => arr.LocationId == shiftCreated.LocationId
                                                                               && arr.UserId == shiftCreated.WorkerID
                                                                               && arr.IsActive == true, null, "")
                                                                               .FirstOrDefault();
            if(workerArrangements == null)
            {
                throw new BaseException(ErrorMessages.ARRANGEMENT_IS_NOT_EXISTED);
            }
            var room = this.unitOfWork.RoomRepository.Get(r => r.RoomId == shiftCreated.RoomId
                                                                && r.LocationId == shiftCreated.LocationId
                                                                && r.IsActive == true).FirstOrDefault();
            if(room == null)
            {
                throw new BaseException(ErrorMessages.ROOM_IS_NOT_EXISTED);
            }
            var shifts = this.unitOfWork.ShiftRepository.Get(shift => shift.RoomId == shiftCreated.RoomId
                                                             && shift.Date.Value.CompareTo(shiftCreated.Date) == 0
                                                             && shift.IsActive == true, null, "").FirstOrDefault();
                                                              
            if(shifts != null)
            {
                throw new BaseException(ErrorMessages.SHIFT_IS_EXISTED);
            }
            var shiftEntity = new Shift
            {
                ArrangementId = workerArrangements.ArrangementId,
                RoomId = shiftCreated.RoomId,
                Date = shiftCreated.Date,
                IsActive = true,
            };
            shiftRepository.Add(shiftEntity);

            return shiftEntity.ToViewModel<ShiftViewModel>();
        }
    }
}
