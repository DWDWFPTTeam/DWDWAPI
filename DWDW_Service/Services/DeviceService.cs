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
        RoomDeviceViewModel AssignDeviceToRoom(RoomDeviceCreateModel roomDevice);
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
            return deviceRepository.GetAll().Select(x => x.ToViewModel<DeviceViewModel>());
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
            var result = new DeviceViewModel();
            var device = deviceRepository.Find(deviceID);
            if (device != null)
            {
                result = device.ToViewModel<DeviceViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.DEVICE_LIST_EMPTY);
            }
            return result;
        }

        public DeviceViewModel CreateDevice(DeviceCreateModel device)
        {
            var result = new DeviceViewModel();
            var checkDevice = deviceRepository.CheckDeviceCodeExisted(device.DeviceCode);
            if (checkDevice == null)
            {
                deviceRepository.Add(new Device
                {
                    DeviceCode = device.DeviceCode,
                    IsActive = true
                });
                result = deviceRepository.GetDeviceCode(device.DeviceCode).ToViewModel<DeviceViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.DEVICE_IS_EXISTED);
            }
            return result;
        }

        public DeviceViewModel UpdateDevice(DeviceUpdateModel device)
        {
            var result = new DeviceViewModel();
            var deviceUpdate = deviceRepository.Find(device.DeviceId);
            if (deviceUpdate != null)
            {
                deviceUpdate.DeviceCode = device.DeviceCode;
                deviceRepository.Update(deviceUpdate);
                result = deviceUpdate.ToViewModel<DeviceViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.DEVICE_IS_NOT_EXISTED);

            }
            return result;
        }

        public DeviceViewModel UpdateDeviceActive(DeviceActiveModel device)
        {
            var result = new DeviceViewModel();
            var deviceUpdate = deviceRepository.Find(device.DeviceId);
            if (deviceUpdate != null)
            {
                using( var transaction = unitOfWork.CreateTransaction())
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
                    }catch(Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }            
            }
            else
            {
                throw new BaseException(ErrorMessages.DEVICE_IS_NOT_EXISTED);
            }
            return result;
        }

        public IEnumerable<DeviceViewModel> GetActiveDeviceFromLocation(int locationID)
        {
            IEnumerable<DeviceViewModel> result = new List<DeviceViewModel>();
            var locationRepo = unitOfWork.LocationRepository;
            var location = locationRepo.Find(locationID);
            if (location != null)
            {
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
                result = deviceList.Select(x => x.ToViewModel<DeviceViewModel>());
            }
            else
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            return result;
        }


        public IEnumerable<DeviceViewModel> GetActiveDeviceFromLocationManager(int userID, int locationID)
        {
            IEnumerable<DeviceViewModel> result = new List<DeviceViewModel>();
            var locationRepo = unitOfWork.LocationRepository;
            var location = locationRepo.Find(locationID);
            bool check = deviceRepository.CheckUserLocation(userID, locationID);
            if (check == true && location != null)
            {
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
                result = deviceList.Select(x => x.ToViewModel<DeviceViewModel>());
            }
            else
            {
                throw new BaseException(ErrorMessages.LOCATION_USER_NOT_EXISTED);
            }
            return result;
        }

        public DeviceViewModel GetActiveDeviceFromRoom(int roomID)
        {
            var result = new DeviceViewModel();
            var roomRepo = unitOfWork.RoomRepository;
            var room = roomRepo.Find(roomID);
            if (room != null)
            {
                var devices = deviceRepository.GetDeviceFromRoom(room.RoomId);
                if (devices != null)
                {
                    result = devices.ToViewModel<DeviceViewModel>();
                }
                else
                {
                    throw new BaseException(ErrorMessages.ROOM_IS_NOT_EXISTED);
                }
            }
            else
            {
                throw new BaseException(ErrorMessages.ROOM_IS_NOT_EXISTED);
            }
            return result;
        }

        public DeviceViewModel GetActiveDeviceFromRoomManager(int userID, int roomID)
        {
            var result = new DeviceViewModel();
            var roomRepo = unitOfWork.RoomRepository;
            var room = roomRepo.Find(roomID);
            bool check = deviceRepository.CheckUserRoom(userID, roomID);

            if (check == true && room != null)
            {
                var devices = deviceRepository.GetDeviceFromRoom(room.RoomId);
                if (devices != null)
                {
                    result = devices.ToViewModel<DeviceViewModel>();
                }
                else
                {
                    throw new BaseException(ErrorMessages.ROOM_IS_NOT_EXISTED);
                }
            }
            else
            {
                throw new BaseException(ErrorMessages.ROOM_USER_NOT_EXISTED);
            }
            return result;
        }

        public RoomDeviceViewModel AssignDeviceToRoom(RoomDeviceCreateModel roomDevice)
        {
            var result = new RoomDeviceViewModel();
            var roomDeviceRepo = unitOfWork.RoomDeviceRepository;
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
            result = roomDeviceRepo.GetLatest().ToViewModel<RoomDeviceViewModel>();
            return result;
        }
    }
}
