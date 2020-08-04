using DWDW_API.Core.Constants;
using DWDW_API.Core.Entities;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DWDW_Service.Services
{
    public interface IDeviceService : IBaseService<Device>
    {
        IEnumerable<DeviceViewModel> GetAll();
        IEnumerable<DeviceViewModel> GetDeviceCode(string deviceCode);
        DeviceViewModel GetDeviceID(int deviceID);
        DeviceViewModel CreateDevice(DeviceCreateModel device);
        DeviceViewModel UpdateDevice(DeviceUpdateModel device);
        DeviceViewModel UpdateDeviceActive(DeviceActiveModel device);
        IEnumerable<DeviceViewModel> GetActiveDeviceFromLocation(int locationID);
        IEnumerable<DeviceViewModel> GetActiveDeviceFromLocationManager(int userID, int locationID);
        DeviceViewModel GetActiveDeviceFromRoom(int roomID);
        DeviceViewModel GetActiveDeviceFromRoomManager(int userID, int roomID);
        RoomDeviceAssignModel AssignDeviceToRoom(RoomDeviceCreateModel roomDevice);
    }
    public class DeviceService : BaseService<Device>, IDeviceService
    {
        private readonly IDeviceRepository deviceRepository;
        public DeviceService(UnitOfWork unitOfWork, IDeviceRepository deviceRepository) : base(unitOfWork)
        {
            this.deviceRepository = deviceRepository;
        }

        public IEnumerable<DeviceViewModel> GetAll()
        {
            var result = deviceRepository.GetAll().Select(x => x.ToViewModel<DeviceViewModel>()).ToList();
            foreach (var element in result)
            {
                int? deviceID = element.DeviceId;
                element.RoomCode = deviceRepository.GetRoomCode(deviceID);
                element.LocationCode = deviceRepository.GetLocationCode(deviceID);
                element.RoomId = deviceRepository.GetRoomID(deviceID);
                element.LocationId = deviceRepository.GetLocationID(deviceID);
                element.StartDate = deviceRepository.GetStartDate(deviceID);
                element.EndDate = deviceRepository.GetEndDate(deviceID);
            }
            return result;
        }

        public IEnumerable<DeviceViewModel> GetDeviceCode(string deviceCode)
        {
            IEnumerable<DeviceViewModel> result = new List<DeviceViewModel>();
            var deviceCodeList = deviceRepository.GetDeviceByCode(deviceCode);
            int count = deviceCodeList.Count();
            result = deviceCodeList.Select(x => x.ToViewModel<DeviceViewModel>());
            return result;
        }

        public DeviceViewModel GetDeviceID(int deviceID)
        {
            DeviceViewModel result;
            var device = deviceRepository.Find(deviceID);
            if (device == null)
            {
                throw new BaseException(ErrorMessages.DEVICE_LIST_EMPTY);
            }
            result = device.ToViewModel<DeviceViewModel>();
            result.RoomCode = deviceRepository.GetRoomCode(deviceID);
            result.LocationCode = deviceRepository.GetLocationCode(deviceID);
            result.RoomId = deviceRepository.GetRoomID(deviceID);
            result.LocationId = deviceRepository.GetLocationID(deviceID);
            result.StartDate = deviceRepository.GetStartDate(deviceID);
            result.EndDate = deviceRepository.GetEndDate(deviceID);
            return result;
        }

        //DatNDD refactors this function
        public DeviceViewModel CreateDevice(DeviceCreateModel device)
        {
            DeviceViewModel result;
            var checkDevice = deviceRepository.CheckDeviceCodeExisted(device.DeviceCode);
            if (checkDevice != null)
            {
                throw new BaseException(ErrorMessages.DEVICE_IS_EXISTED);
            }
            var deviceEntity = device.ToEntity<Device>();
            deviceEntity.IsActive = true;
            deviceRepository.Add(deviceEntity);
            result = deviceEntity.ToViewModel<DeviceViewModel>();
            return result;
        }

        public DeviceViewModel UpdateDevice(DeviceUpdateModel device)
        {
            DeviceViewModel result;
            var deviceUpdate = deviceRepository.Find(device.DeviceId);
            if (deviceUpdate == null)
            {
                throw new BaseException(ErrorMessages.DEVICE_IS_NOT_EXISTED);
            }
            var checkDevice = deviceRepository.CheckDeviceCodeExisted(device.DeviceCode);
            if (checkDevice != null)
            {
                throw new BaseException(ErrorMessages.DEVICE_IS_EXISTED);
            }
            deviceUpdate.DeviceCode = device.DeviceCode;
            deviceRepository.Update(deviceUpdate);
            result = deviceUpdate.ToViewModel<DeviceViewModel>();
            return result;
        }

        public DeviceViewModel UpdateDeviceActive(DeviceActiveModel device)
        {
            DeviceViewModel result;
            var deviceUpdate = deviceRepository.Find(device.DeviceId);
            if (deviceUpdate == null)
            {
                throw new BaseException(ErrorMessages.DEVICE_IS_NOT_EXISTED);
            }
            using (var transaction = unitOfWork.CreateTransaction())
            {
                try
                {
                    deviceUpdate.IsActive = device.IsActive;
                    if (device.IsActive == false)
                    {
                        var roomDeviceRepo = unitOfWork.RoomDeviceRepository;
                        roomDeviceRepo.DisableDeviceRoom(deviceUpdate.DeviceId);
                    }
                    deviceRepository.Update(deviceUpdate);
                    result = deviceUpdate.ToViewModel<DeviceViewModel>();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
            return result;
        }

        public IEnumerable<DeviceViewModel> GetActiveDeviceFromLocation(int locationID)
        {
            IEnumerable<DeviceViewModel> result;
            var locationRepo = unitOfWork.LocationRepository;
            var location = locationRepo.Find(locationID);
            if (location == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            var roomRepo = unitOfWork.RoomRepository;
            var roomList = roomRepo.GetRoomFromLocation(locationID);
            var deviceList = new List<Device>();
            for (int i = 0; i < roomList.Count; i++)
            {
                var roomAt = roomList.ElementAt(i);
                var deviceListAt = deviceRepository.GetDeviceFromRoom(roomAt.RoomId);
                if (deviceListAt != null)
                {
                    deviceList.Add(deviceListAt);
                }
            }
            result = deviceList.Select(x => x.ToViewModel<DeviceViewModel>()).ToList();
            foreach (var element in result)
            {
                int? deviceID = element.DeviceId;
                element.RoomCode = deviceRepository.GetRoomCode(deviceID);
                element.LocationCode = deviceRepository.GetLocationCode(deviceID);
                element.RoomId = deviceRepository.GetRoomID(deviceID);
                element.LocationId = deviceRepository.GetLocationID(deviceID);
                element.StartDate = deviceRepository.GetStartDate(deviceID);
                element.EndDate = deviceRepository.GetEndDate(deviceID);
            }
            return result;
        }


        public IEnumerable<DeviceViewModel> GetActiveDeviceFromLocationManager(int userID, int locationID)
        {
            IEnumerable<DeviceViewModel> result;
            var locationRepo = unitOfWork.LocationRepository;
            var location = locationRepo.Find(locationID);
            if (location == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            bool check = deviceRepository.CheckUserLocation(userID, locationID);
            if (check == false)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_BELONG_TO_MANAGER);
            }
            var roomRepo = unitOfWork.RoomRepository;
            var roomList = roomRepo.GetRoomFromLocation(locationID);
            var deviceList = new List<Device>();
            for (int i = 0; i < roomList.Count; i++)
            {
                var roomAt = roomList.ElementAt(i);
                var deviceListAt = deviceRepository.GetDeviceFromRoom(roomAt.RoomId);
                if (deviceListAt != null)
                {
                    deviceList.Add(deviceListAt);
                }
            }
            result = deviceList.Select(x => x.ToViewModel<DeviceViewModel>()).ToList();
            foreach (var element in result)
            {
                int? deviceID = element.DeviceId;
                element.RoomCode = deviceRepository.GetRoomCode(deviceID);
                element.LocationCode = deviceRepository.GetLocationCode(deviceID);
                element.RoomId = deviceRepository.GetRoomID(deviceID);
                element.LocationId = deviceRepository.GetLocationID(deviceID);
                element.StartDate = deviceRepository.GetStartDate(deviceID);
                element.EndDate = deviceRepository.GetEndDate(deviceID);
            }
            return result;
        }

        public DeviceViewModel GetActiveDeviceFromRoom(int roomID)
        {
            DeviceViewModel result;
            var roomRepo = unitOfWork.RoomRepository;
            var room = roomRepo.Find(roomID);
            if(room == null)
            {
                throw new BaseException(ErrorMessages.ROOM_IS_NOT_EXISTED);
            }
            var devices = deviceRepository.GetDeviceFromRoom(room.RoomId);
            if(devices == null)
            {
                throw new BaseException(ErrorMessages.ROOM_DEVICE_EMPTY);
            }
            result = devices.ToViewModel<DeviceViewModel>();
            result.RoomCode = deviceRepository.GetRoomCode(result.DeviceId);
            result.LocationCode = deviceRepository.GetLocationCode(result.DeviceId);
            result.RoomId = deviceRepository.GetRoomID(result.DeviceId);
            result.LocationId = deviceRepository.GetLocationID(result.DeviceId);
            result.StartDate = deviceRepository.GetStartDate(result.DeviceId);
            result.EndDate = deviceRepository.GetEndDate(result.DeviceId);
            return result;
        }

        public DeviceViewModel GetActiveDeviceFromRoomManager(int userID, int roomID)
        {
            DeviceViewModel result;
            var roomRepo = unitOfWork.RoomRepository;
            var room = roomRepo.Find(roomID);
            if (room == null)
            {
                throw new BaseException(ErrorMessages.ROOM_IS_NOT_EXISTED);
            }
            bool check = deviceRepository.CheckUserRoom(userID, roomID);
            if (check == false)
            {
                throw new BaseException(ErrorMessages.ROOM_USER_NOT_EXISTED);
            }
            var devices = deviceRepository.GetDeviceFromRoom(room.RoomId);
            if (devices == null)
            {
                throw new BaseException(ErrorMessages.ROOM_DEVICE_EMPTY);
            }
            result = devices.ToViewModel<DeviceViewModel>();
            result.RoomCode = deviceRepository.GetRoomCode(result.DeviceId);
            result.LocationCode = deviceRepository.GetLocationCode(result.DeviceId);
            result.RoomId = deviceRepository.GetRoomID(result.DeviceId);
            result.LocationId = deviceRepository.GetLocationID(result.DeviceId);
            result.StartDate = deviceRepository.GetStartDate(result.DeviceId);
            result.EndDate = deviceRepository.GetEndDate(result.DeviceId);
            return result;
        }

        public RoomDeviceAssignModel AssignDeviceToRoom(RoomDeviceCreateModel roomDevice)
        {
            RoomDeviceAssignModel result;
            var roomDeviceRepo = unitOfWork.RoomDeviceRepository;
            var roomRepo = unitOfWork.RoomRepository;
            var deviceRepo = unitOfWork.DeviceRepository;
            roomDeviceRepo.DisableDeviceRoom(roomDevice.DeviceId);
            roomDeviceRepo.DisableRoomDevice(roomDevice.RoomId);
            roomDeviceRepo.Add(new RoomDevice
            {
                RoomId = roomDevice.RoomId,
                DeviceId = roomDevice.DeviceId,
                StartDate = roomDevice.StartDate,
                EndDate = roomDevice.EndDate,
                IsActive = true
            });
            result = roomDeviceRepo.GetLatest().ToViewModel<RoomDeviceAssignModel>();
            var room = roomRepo.Find(result.RoomId);
            var device = deviceRepo.Find(result.DeviceId);
            result.RoomCode = room.RoomCode;
            result.DeviceCode = device.DeviceCode;
            return result;
        }
    }
}
