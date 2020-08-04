using DWDW_API.Core.Constants;
using DWDW_API.Core.Entities;
using DWDW_API.Core.ViewModels;
using DWDW_Service.Repositories;
using DWDW_Service.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using DWDW_API.Core.Infrastructure;

namespace DWDW_Service.Services
{
    public interface IRecordService : IBaseService<Record>
    {
        RecordViewModel SaveRecord(RecordReceivedModel record);
        IEnumerable<RecordViewModel> GetRecordByLocationId(int locationId);
        IEnumerable<RecordViewModel> GetRecordsByLocationIdBetweenTime
            (int locationId, DateTime startDate, DateTime endDate);
        IEnumerable<RecordViewModel> GetRecordsByLocationIdAndTime
            (int locationId, DateTime date);
        IEnumerable<RecordViewModel> GetRecordByWorkerDate(int workerID, DateTime date);
        IEnumerable<RecordViewModel> GetRecordByWorkerDateForManager(int managerID, int workerID, DateTime date);
    }

    public class RecordService : BaseService<Record>, IRecordService
    {
        private readonly IRecordRepository recordRepository;
        public RecordService(UnitOfWork unitOfWork, IRecordRepository recordRepository) : base(unitOfWork)
        {
            this.recordRepository = recordRepository;
        }

        //public string GetDeviceToken(int deviceID)
        //{
        //    var manager = recordRepository.GetDeviceToken(deviceID);
        //    string managerDeviceToken = manager.DeviceToken;
        //    return managerDeviceToken;
        //}



        public IEnumerable<RecordViewModel> GetRecordByLocationId(int locationId)
        {
            var roomRepo = this.unitOfWork.RoomRepository;
            var record = recordRepository.GetRecordsByLocationId(locationId)
                .Select(r => r.ToViewModel<RecordViewModel>()).ToList();
            foreach (var element in record)
            {
                int? deviceID = element.DeviceId;
                var room = roomRepo.GetRoomFromDevice(deviceID);
                element.RoomId = room.RoomId;
            }
            return record;
        }

        public IEnumerable<RecordViewModel> GetRecordByWorkerDate(int workerID, DateTime date)
        {
            IEnumerable<RecordViewModel> result = new List<RecordViewModel>();
            
            var locationRepo = unitOfWork.LocationRepository;
            var roomRepo = this.unitOfWork.RoomRepository;
            var roomDeviceRepo = unitOfWork.RoomDeviceRepository;

            //Lay ra danh sach id cua cac item lien quan toi worker
            List<int?> locationRelatedID = locationRepo.GetLocationByUser(workerID);
            List<int?> roomRelatedID = roomRepo.GetRelatedRoomIDFromLocation(locationRelatedID);
            List<int?> deviceRelatedID = roomDeviceRepo.GetRelatedDeviceIDFromRoom(roomRelatedID);

            var recordList = recordRepository.GetRecordByWorkerDate(deviceRelatedID, date);
            result = recordList.Select(x => x.ToViewModel<RecordViewModel>()).ToList();
            foreach(var element in result)
            {
                int? deviceID = element.DeviceId;
                var room = roomRepo.GetRoomFromDevice(deviceID);
                element.RoomId = room.RoomId;
            }
            return result;
        }

        public IEnumerable<RecordViewModel> GetRecordByWorkerDateForManager(int managerID, int workerID, DateTime date)
        {
            IEnumerable<RecordViewModel> result = new List<RecordViewModel>();

            var locationRepo = unitOfWork.LocationRepository;
            var roomRepo = this.unitOfWork.RoomRepository;
            var roomDeviceRepo = unitOfWork.RoomDeviceRepository;

            List<int?> locationManagerRelatedID = locationRepo.GetLocationByUser(workerID);
            List<int?> locationUserRelatedID = locationRepo.GetLocationByUser(workerID);
            
            //User thuoc Manager
            bool userChildManager = locationUserRelatedID.All(x => locationManagerRelatedID.Contains(x));
            if (userChildManager == true)
            {
                //Lay ra danh sach id cua cac item lien quan toi worker
                List<int?> roomRelatedID = roomRepo.GetRelatedRoomIDFromLocation(locationUserRelatedID);
                List<int?> deviceRelatedID = roomDeviceRepo.GetRelatedDeviceIDFromRoom(roomRelatedID);

                var recordList = recordRepository.GetRecordByWorkerDate(deviceRelatedID, date);
                result = recordList.Select(x => x.ToViewModel<RecordViewModel>()).ToList();
                foreach (var element in result)
                {
                    int? deviceID = element.DeviceId;
                    var room = roomRepo.GetRoomFromDevice(deviceID);
                    element.RoomId = room.RoomId;
                }
            }
            return result;
        }

        public IEnumerable<RecordViewModel> GetRecordsByLocationIdBetweenTime(int locationId, DateTime startDate, DateTime endDate)
        {
            var roomRepo = this.unitOfWork.RoomRepository;
            var record = recordRepository
                .GetRecordsByLocationIdBetweenTime(locationId, startDate, endDate)
                .Select(r => r.ToViewModel<RecordViewModel>()).ToList();
            foreach (var element in record)
            {
                int? deviceID = element.DeviceId;
                var room = roomRepo.GetRoomFromDevice(deviceID);
                element.RoomId = room.RoomId;
            }
            return record;
        }

        public IEnumerable<RecordViewModel> GetRecordsByLocationIdAndTime(int locationId, DateTime date)
        {
            var roomRepo = this.unitOfWork.RoomRepository;
            var record = recordRepository
                .GetRecordsByLocationIdAndTime(locationId, date)
                .Select(r => r.ToViewModel<RecordViewModel>());
            foreach (var element in record)
            {
                int? deviceID = element.DeviceId;
                var room = roomRepo.GetRoomFromDevice(deviceID);
                element.RoomId = room.RoomId;
            }
            return record;
        }

        public RecordViewModel SaveRecord(RecordReceivedModel record)
        {
            RecordViewModel result;
            var device = unitOfWork.DeviceRepository.GetDeviceCode(record.DeviceCode);
            if (device != null)
            {
                var recordEntity = new Record
                {
                    DeviceId = device.DeviceId,
                    Type = record.Type,
                    Image = record.Image,
                    RecordDateTime = DateTime.Now
                };
                var notificationFCM = this.CreateNotificationFCM(recordEntity, device.DeviceCode);
                using (var transaction = unitOfWork.CreateTransaction())
                {
                    try
                    {
                        //save record
                        recordRepository.Add(recordEntity);
                        //save noti
                        unitOfWork.NotificationRepository.Add(notificationFCM.NotificationVM.ToEntity<Notifications>());
                        transaction.Commit();
                        result = recordEntity.ToViewModel<RecordViewModel>();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
                this.SendNotify(notificationFCM);

            }
            else
            {
                throw new BaseException(ErrorMessages.DEVICE_IS_NOT_EXISTED);
            }

            return result;
        }
        
        private NotificationFCMViewModel CreateNotificationFCM(Record record, string deviceCode)
        {
         
            //Get room from device is placed
            var room = unitOfWork.RoomDeviceRepository.Get(rd => rd.DeviceId == record.DeviceId
                                                                 && rd.IsActive == true, null, "Room")
                                                                .FirstOrDefault().Room;
            if(room == null)
            {
                throw new BaseException(ErrorMessages.ROOM_NOT_FOUND);
            }
            //Get Manager who manage the room
            var manager = unitOfWork.ArrangementRepository.Get(a => a.LocationId == room.LocationId
                                                            && a.IsActive == true, null, "User")
                                                            .Select(a => a.User)
                                                            .Where(u => u.RoleId.ToString().Equals(Constant.MANAGER))
                                                            .FirstOrDefault();
            if(manager == null)
            {
                throw new BaseException(ErrorMessages.MANAGER_NOT_FOUND);
            }

            //Get the shift of room and time
            var shift = unitOfWork.ShiftRepository.GetShiftByRoomDate(room.RoomId, record.RecordDateTime);
            if (shift == null)
            {
                throw new BaseException(ErrorMessages.SHIFT_IS_NOT_EXISTED);
            }

            var worker = unitOfWork.UserRepository.Find(shift.Arrangement.UserId);
            var notification = unitOfWork.NotificationRepository.CreateNotification(record, room, manager, worker, deviceCode);

            return new NotificationFCMViewModel {
                Title = notification.MessageTitle,
                Message = notification.MessageContent,
                DeviceToken = manager.DeviceToken,
                NotificationVM = notification.ToViewModel<NotificationViewModel>()
            };
        }

        private byte[] generateNotify(NotificationFCMViewModel notificationFCM)
        {
            var data = new
            {
                data = new
                {
                    title = notificationFCM.Title,
                    message = notificationFCM.Message
                },
                to = notificationFCM.DeviceToken
            };

            var xc = JsonConvert.SerializeObject(data);
            var byteArray = Encoding.UTF8.GetBytes(xc);
            return byteArray;
        }

        private void SendNotify(NotificationFCMViewModel notificationFCM)
        {

            var byteArray = generateNotify(notificationFCM);

            var authorizationKey = Constant.FIREBASE_AUTHORIZATION_KEY;
            var sender_id = Constant.FIREBASE_SENDER_ID;



            var tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "POST";
            tRequest.ContentType = "application/json";
            tRequest.Headers.Add($"Authorization: key={authorizationKey}");
            tRequest.Headers.Add($"Sender: id={sender_id}");


            tRequest.ContentLength = byteArray.Length;
            var dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            var tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();
            var tReader = new StreamReader(dataStream);

            var tResponseFromServer = tReader.ReadToEnd();

            tReader.Close();
            dataStream.Close();
            tResponse.Close();
        }

       
    }
}
