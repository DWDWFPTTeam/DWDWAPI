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
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography.X509Certificates;

namespace DWDW_Service.Services
{
    public interface IRecordService : IBaseService<Record>
    {
        Task<RecordViewModel> SaveRecord(RecordReceivedModel record, string imageRoot);
        IEnumerable<RecordImageViewModel> GetRecordByLocationId(int locationId);
        IEnumerable<RecordViewModel> GetRecordsByLocationDate(RecordLocationReceivedViewModel record);
        IEnumerable<RecordViewModel> GetRecordsByLocationIdAndTime
            (int locationId, DateTime date);
        IEnumerable<RecordViewModel> GetRecordByWorkerDate(int workerID, DateTime date);
        IEnumerable<RecordViewModel> GetRecordByWorkerDateForManager(int managerID, int workerID, DateTime date);
        RecordImageViewModel GetRecordById(int recordId);
        Task<RecordViewModel> SaveRecord(RecordReceivedByteModel recordReceived, string imageRoot);
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



        //public IEnumerable<RecordViewModel> GetRecordByLocationId(int locationId)
        //{
        //    var roomRepo = this.unitOfWork.RoomRepository;
        //    var record = recordRepository.GetRecordsByLocationId(locationId)
        //        .Select(r => r.ToViewModel<RecordViewModel>()).ToList();
        //    foreach (var element in record)
        //    {
        //        int? deviceID = element.DeviceId;
        //        var room = roomRepo.GetRoomFromDevice(deviceID);
        //        element.RoomId = room.RoomId;
        //    }
        //    return record;
        //}

        public IEnumerable<RecordImageViewModel> GetRecordByLocationId(int locationId)
        {
            var roomRepo = this.unitOfWork.RoomRepository;
            var records = recordRepository.GetRecordsByLocationId(locationId)
                .Select(r =>
                {
                    var record = r.ToViewModel<RecordImageViewModel>();
                    record.RoomId = roomRepo.GetRoomFromDevice(r.DeviceId).RoomId;
                    record.ImageByte = File.ReadAllBytes(r.Image);
                    return record;
                }).ToList();

            return records;
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
            foreach (var element in result)
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

            List<int?> locationManagerRelatedID = locationRepo.GetLocationByUser(managerID);
            List<int?> locationUserRelatedID = locationRepo.GetLocationByUser(workerID);

            //User thuoc Manager
            bool userChildManager = locationUserRelatedID.All(x => locationManagerRelatedID.Contains(x));
            if (userChildManager != true)
            {
                throw new BaseException(ErrorMessages.MANAGER_WORKER_NOT_EXISTED);
            }

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

            return result;
        }

        //public IEnumerable<RecordViewModel> GetRecordsByLocationDate(int locationId, DateTime startDate, DateTime endDate)
        //{
        //    var locationRepo = this.unitOfWork.LocationRepository;
        //    var location = locationRepo.Find(locationId);
        //    if (location == null) throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);

        //    var roomRepo = this.unitOfWork.RoomRepository;
        //    List<RecordViewModel> records = new List<RecordViewModel>();
        //    int check = DateTime.Compare(startDate, endDate);
        //    if (check < 0)
        //    {
        //        records = recordRepository
        //        .GetRecordsByLocationIdBetweenTime(locationId, startDate, endDate)
        //        .Select(r => r.ToViewModel<RecordViewModel>()).ToList();
        //    }
        //    else if (check == 0)
        //    {
        //        records = recordRepository
        //        .GetRecordsByLocationIdAndTime(locationId, startDate)
        //        .Select(r => r.ToViewModel<RecordViewModel>()).ToList();
        //    }
        //    else
        //    {
        //        DateTime f;
        //        f = startDate;
        //        startDate = endDate;
        //        endDate = f;
        //        records = recordRepository
        //        .GetRecordsByLocationIdBetweenTime(locationId, startDate, endDate)
        //        .Select(r => r.ToViewModel<RecordViewModel>()).ToList();
        //    }

        //    foreach (var element in records)
        //    {
        //        int? deviceID = element.DeviceId;
        //        var room = roomRepo.GetRoomFromDevice(deviceID);
        //        element.RoomId = room.RoomId;
        //    }
        //    return records;
        //}
        public IEnumerable<RecordViewModel> GetRecordsByLocationDate(RecordLocationReceivedViewModel record)
        {
            var location = this.unitOfWork.LocationRepository.Find(record.LocationId);
            if (location == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            var deviceIdsInLocation = this.unitOfWork.RoomRepository.Get(room => room.LocationId == record.LocationId, null, "RoomDevice")
                                                                      .Select(room => room.RoomDevice)
                                                                      .Select(roomDevice => roomDevice.FirstOrDefault().DeviceId);
            if (deviceIdsInLocation.Count() == 0)
            {
                throw new BaseException(ErrorMessages.LOCATION_HAVE_NO_DEVICE);
            }
            var listRecords = new List<Record>();
            foreach (var id in deviceIdsInLocation)
            {
                var records = this.recordRepository.Get(r => r.DeviceId == id
                                                        && r.RecordDateTime >= record.StartDate.Date
                                                        && r.RecordDateTime <= record.EndDate.Date, null, "");
                listRecords.AddRange(records);
            }

            var recordEntities =  listRecords.OrderByDescending(r => r.RecordId).ToList();
            return recordEntities.Select(r => r.ToViewModel<RecordViewModel>());
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

        public async Task<RecordViewModel> SaveRecord(RecordReceivedModel record, string imageRoot)
        {
            RecordViewModel result;
            var device = unitOfWork.DeviceRepository.GetDeviceCode(record.DeviceCode);
            if (device != null)
            {
                var recordEntity = new Record
                {
                    DeviceId = device.DeviceId,
                    Type = record.Type,
                    RecordDateTime = DateTime.Now,

                };
                var notificationFCM = this.CreateNotificationFCM(recordEntity, device.DeviceCode);
                if (record.Image.Length <= 0)
                {
                    throw new BaseException("Image is not existed!");
                }
                if (!Directory.Exists(imageRoot))
                {
                    Directory.CreateDirectory(imageRoot);

                }
                var dir = new DirectoryInfo(imageRoot);
                var imagePath = imageRoot + record.Image.Name + dir.GetFiles().Length + ".jpg";
                using (var fileStream = File.Create(imagePath))
                {
                    await record.Image.CopyToAsync(fileStream);
                    fileStream.Flush();
                    recordEntity.Image = imagePath;
                }
                using (var transaction = unitOfWork.CreateTransaction())
                {
                    try
                    {
                        //save record
                        await recordRepository.AddAsync(recordEntity);
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

        public async Task<RecordViewModel> SaveRecord(RecordReceivedByteModel record, string imageRoot)
        {
            RecordViewModel result;
            var device = unitOfWork.DeviceRepository.GetDeviceCode(record.DeviceCode);
            if(device == null)
            {
                throw new BaseException(ErrorMessages.DEVICE_IS_NOT_EXISTED);
            }
            var recordEntity = new Record
            {
                DeviceId = device.DeviceId,
                Type = record.Type,
                RecordDateTime = DateTime.Now,

            };
            var notificationFCM = this.CreateNotificationFCM(recordEntity, device.DeviceCode);
            if (record.Image.Length <= 0)
            {
                throw new BaseException("Image is not existed!");
            }
            if (!Directory.Exists(imageRoot))
            {
                Directory.CreateDirectory(imageRoot);

            }
            var dir = new DirectoryInfo(imageRoot);
            var imagePath = imageRoot + "image" + dir.GetFiles().Length + ".jpg";
            using (Image image = Image.FromStream(new MemoryStream(record.Image)))
            {
                image.Save(imagePath, ImageFormat.Jpeg);  // Or Png
                recordEntity.Image = imagePath;
            }

            using (var transaction = unitOfWork.CreateTransaction())
            {
                try
                {
                    //save record
                    await recordRepository.AddAsync(recordEntity);
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

            return result;
        }

        private NotificationFCMViewModel CreateNotificationFCM(Record record, string deviceCode)
        {

            //Get room from device is placed
            var room = unitOfWork.RoomDeviceRepository.Get(rd => rd.DeviceId == record.DeviceId
                                                                 && rd.IsActive == true, null, "Room")
                                                                .FirstOrDefault().Room;
            if (room == null)
            {
                throw new BaseException(ErrorMessages.ROOM_NOT_FOUND);
            }
            //Get Manager who manage the room
            var manager = unitOfWork.ArrangementRepository.Get(a => a.LocationId == room.LocationId
                                                            && a.IsActive == true, null, "User")
                                                            .Select(a => a.User)
                                                            .Where(u => u.RoleId.ToString().Equals(Constant.MANAGER))
                                                            .FirstOrDefault();
            if (manager == null)
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

            return new NotificationFCMViewModel
            {
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

        public RecordImageViewModel GetRecordById(int recordId)
        {
            var recordEntity = recordRepository.Find(recordId);
            if (recordEntity == null)
            {
                throw new BaseException(ErrorMessages.RECORDID_IS_NOT_EXISTED);
            }
            var recordVM = recordEntity.ToViewModel<RecordImageViewModel>();
            recordVM.ImageByte = File.ReadAllBytes(recordEntity.Image);

            return recordVM;
        }

       
    }
}
