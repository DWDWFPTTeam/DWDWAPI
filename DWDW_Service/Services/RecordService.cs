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
        IEnumerable<RecordRoomCodeViewModel> GetRecordsByLocationDate(RecordLocationReceivedViewModel record);
        IEnumerable<RecordViewModel> GetRecordsByLocationIdAndTime
            (int locationId, DateTime date);
        IEnumerable<RecordViewModel> GetRecordByWorkerDate(int workerID, DateTime date);
        IEnumerable<RecordViewModel> GetRecordByWorkerDateForManager(int managerID, int workerID, DateTime date);
        IEnumerable<RecordViewModel> GetRecordByLocationWorkerDateForManager(int managerID, int workerID, int locationID, DateTime date);
        IEnumerable<RecordViewModel> GetSleepyRecordByLocationWorkerDateForManager(int managerID, int workerID, int locationID, DateTime date);
        IEnumerable<RecordViewModel> GetDeniedRecordByLocationWorkerDateForManager(int managerID, int workerID, int locationID, DateTime date);
        IEnumerable<RecordViewModel> GetRecordByLocationDateForWorker(int workerID, int locationID, DateTime date);
        IEnumerable<RecordViewModel> GetSleepyRecordByLocationDateForWorker(int workerID, int locationID, DateTime date);
        IEnumerable<RecordViewModel> GetDeniedRecordByLocationDateForWorker(int workerID, int locationID, DateTime date);
        RecordImageViewModel GetRecordById(int recordId);
        RecordViewModel DenyRecordStatusWorker(int userID, RecordStatusModel record);
        //Task<RecordViewModel> SaveRecordByte(RecordReceivedByteModel recordReceived, string imageRoot);
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
            var records = recordRepository.GetRecordsByLocationId(locationId)
                .Select(r =>
                {
                    var record = r.ToViewModel<RecordImageViewModel>();
                    var room = this.unitOfWork.RoomDeviceRepository.Get(rd => rd.DeviceId == r.DeviceId
                                                                        && rd.IsActive == true, null, "Room")
                                                                        .Where(rd => rd.Room != null)
                                                                        .Select(rd => rd.Room).FirstOrDefault();
                    record.RoomCode = room.RoomCode;
                    record.ImageByte = File.ReadAllBytes(r.Image);
                    return record;
                }).ToList();
            records.Reverse();
            return records;
        }

        public IEnumerable<RecordViewModel> GetRecordByWorkerDate(int workerID, DateTime date)
        {
            IEnumerable<RecordViewModel> result = new List<RecordViewModel>();

            var locationRepo = unitOfWork.LocationRepository;
            var roomRepo = this.unitOfWork.RoomRepository;
            var roomDeviceRepo = unitOfWork.RoomDeviceRepository;
            var userRepo = unitOfWork.UserRepository;
            var worker = userRepo.Find(workerID);
            if (worker == null)
            {
                throw new BaseException(ErrorMessages.USERID_IS_NOT_EXISTED);
            }

            List<int?> arrangementWorker = recordRepository.GetRelatedArrangement(workerID);
            var roomID = recordRepository.GetShiftRoomByArrangementDate(arrangementWorker, date);
            if (roomID == null)
            {
                throw new BaseException(ErrorMessages.SHIFT_IS_NOT_EXISTED);
            }
            var recordList = recordRepository.GetRecordByWorkerDate(roomID, date);
            result = recordList.Select(x => x.ToViewModel<RecordViewModel>()).ToList();
            foreach (var element in result)
            {
                element.RoomId = roomID;
            }
            result.Reverse();
            return result;
        }

        public IEnumerable<RecordViewModel> GetRecordByWorkerDateForManager(int managerID, int workerID, DateTime date)
        {
            IEnumerable<RecordViewModel> result = new List<RecordViewModel>();

            var locationRepo = unitOfWork.LocationRepository;
            var roomRepo = this.unitOfWork.RoomRepository;
            var roomDeviceRepo = unitOfWork.RoomDeviceRepository;
            var userRepo = unitOfWork.UserRepository;
            var worker = userRepo.Find(workerID);
            if (worker == null)
            {
                throw new BaseException(ErrorMessages.USERID_IS_NOT_EXISTED);
            }


            List<int?> locationManagerRelatedID = locationRepo.GetLocationByUser(managerID);
            List<int?> locationUserRelatedID = locationRepo.GetLocationByUser(workerID);


            //User thuoc Manager
            //bool userChildManager = locationUserRelatedID.All(x => locationManagerRelatedID.Contains(x));
            //if (userChildManager != true)
            //{
            //    throw new BaseException(ErrorMessages.MANAGER_WORKER_NOT_EXISTED);
            //}

            List<int?> arrangementWorker = recordRepository.GetRelatedArrangement(workerID);
            var roomID = recordRepository.GetShiftRoomByArrangementDate(arrangementWorker, date);
            if (roomID == null)
            {
                throw new BaseException(ErrorMessages.SHIFT_IS_NOT_EXISTED);
            }
            var recordList = recordRepository.GetRecordByWorkerDate(roomID, date);
            result = recordList.Select(x => x.ToViewModel<RecordViewModel>()).ToList();
            foreach (var element in result)
            {
                element.RoomId = roomID;
            }
            result.Reverse();
            return result;
        }

        public IEnumerable<RecordViewModel> GetRecordByLocationWorkerDateForManager(int managerID, int workerID, int locationID, DateTime date)
        {
            IEnumerable<RecordViewModel> result = new List<RecordViewModel>();

            var locationRepo = unitOfWork.LocationRepository;
            var roomRepo = this.unitOfWork.RoomRepository;
            var roomDeviceRepo = unitOfWork.RoomDeviceRepository;
            var userRepo = unitOfWork.UserRepository;
            var shiftRepo = unitOfWork.ShiftRepository;
            var deviceRepo = unitOfWork.DeviceRepository;
            var arrangementRepo = unitOfWork.ArrangementRepository;
            var worker = userRepo.Find(workerID);
            if (worker == null)
            {
                throw new BaseException(ErrorMessages.USERID_IS_NOT_EXISTED);
            }
            var location = locationRepo.Find(locationID);
            if (location == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }

            //Lay ra shift tu location by date cua worker
            var arrangement = arrangementRepo.CheckLocationManagerWorker(workerID, locationID, date);
            if (arrangement == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_BELONG_TO_WORKER);
            }
            var shiftLocation = shiftRepo.GetShiftFromLocationWorker(workerID, locationID);
            var shiftList = shiftLocation.FirstOrDefault(x => x.Date == date.Date && x.IsActive == true);
            //Lấy ra room từ shift và từ room ra device
            var roomID = shiftList.RoomId;
            var device = deviceRepo.GetDeviceFromRoomByDate(roomID, date);
            //Từ device và date ra record
            var record = recordRepository.GetRecordByDeviceDate(device.DeviceId, date);
            result = record.Select(x => x.ToViewModel<RecordViewModel>()).ToList();
            foreach (var element in result)
            {
                var roomRecord = roomRepo.GetRoomFromDevice(element);
                element.RoomId = roomRecord.RoomId;
                element.RoomCode = roomRecord.RoomCode;
            }
            result.Reverse();
            return result;
        }

        public IEnumerable<RecordViewModel> GetSleepyRecordByLocationWorkerDateForManager(int managerID, int workerID, int locationID, DateTime date)
        {
            var records = GetRecordByLocationWorkerDateForManager(managerID, workerID, locationID, date);
            var result = records.Where(x => x.Status == Constant.SLEEPY).ToList();
            return result;
        }
        public IEnumerable<RecordViewModel> GetDeniedRecordByLocationWorkerDateForManager(int managerID, int workerID, int locationID, DateTime date)
        {
            var records = GetRecordByLocationWorkerDateForManager(managerID, workerID, locationID, date);
            var result = records.Where(x => x.Status == Constant.DENY_SLEEPY).ToList();
            return result;
        }

        public IEnumerable<RecordViewModel> GetRecordByLocationDateForWorker(int workerID, int locationID, DateTime date)
        {
            var locationRepo = unitOfWork.LocationRepository;
            var shiftRepo = unitOfWork.ShiftRepository;
            var deviceRepo = unitOfWork.DeviceRepository;
            var roomRepo = unitOfWork.RoomRepository;
            var arrangementRepo = unitOfWork.ArrangementRepository;
            var location = locationRepo.Find(locationID);
            if (location == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }
            bool check = deviceRepo.CheckUserLocation(workerID, locationID);
            if (check == false)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_BELONG_TO_WORKER);
            }

            //Lay ra shift tu location by date cua worker
            var arrangement = arrangementRepo.CheckLocationManagerWorker(workerID, locationID, date);
            if (arrangement == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_BELONG_TO_WORKER);
            }
            var shiftLocation = shiftRepo.GetShiftFromLocationWorker(workerID, locationID);
            var shiftList = shiftLocation.FirstOrDefault(x => x.Date == date.Date && x.IsActive == true);
            if (shiftList == null)
            {
                throw new BaseException(ErrorMessages.RECORD_DATE_NOT_EXISTED);
            }
            //Lấy ra room từ shift và từ room ra device
            var roomID = shiftList.RoomId;
            var device = deviceRepo.GetDeviceFromRoomByDate(roomID, date);
            //Từ device và date ra record
            var record = recordRepository.GetRecordByDeviceDate(device.DeviceId, date);
            var result = record.Select(x => x.ToViewModel<RecordViewModel>()).ToList();
            foreach(var element in result)
            {
                var roomRecord = roomRepo.GetRoomFromDevice(element);
                element.RoomId = roomRecord.RoomId;
                element.RoomCode = roomRecord.RoomCode;
            }
            result.Reverse();
            return result;
        }
        public IEnumerable<RecordViewModel> GetSleepyRecordByLocationDateForWorker(int workerID, int locationID, DateTime date)
        {
            var listRecordOriginal = GetRecordByLocationDateForWorker(workerID, locationID, date);
            var result = listRecordOriginal.Where(x => x.Status == Constant.SLEEPY).ToList();
            return result;
        }
        public IEnumerable<RecordViewModel> GetDeniedRecordByLocationDateForWorker(int workerID, int locationID, DateTime date)
        {
            var listRecordOriginal = GetRecordByLocationDateForWorker(workerID, locationID, date);
            var result = listRecordOriginal.Where(x => x.Status == Constant.DENY_SLEEPY).ToList();
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
        public IEnumerable<RecordRoomCodeViewModel> GetRecordsByLocationDate(RecordLocationReceivedViewModel record)
        {
            var location = this.unitOfWork.LocationRepository.Find(record.LocationId);
            if (location == null)
            {
                throw new BaseException(ErrorMessages.LOCATION_IS_NOT_EXISTED);
            }

            var deviceIdsInLocation = this.unitOfWork.RoomRepository.Get(room => room.LocationId == record.LocationId, null, "RoomDevice")
                                                                      .Select(room => room.RoomDevice)
                                                                      .Where(rd => rd.Count > 0)
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

            var recordEntities = listRecords.OrderByDescending(r => r.RecordId).ToList();
            var recordVM = recordEntities.Select(r =>
            {
                var recordViewModel = r.ToViewModel<RecordRoomCodeViewModel>();
                var room = this.unitOfWork.RoomDeviceRepository.Get(rd => rd.DeviceId == r.DeviceId, null, "Room")
                                                               .Where(rd => rd.Room != null)
                                                               .Select(rd => rd.Room).FirstOrDefault();
                recordViewModel.RoomCode = room.RoomCode;
                return recordViewModel;
            });
            recordVM.Reverse();
            return recordVM;
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
                var room = roomRepo.GetRoomFromDevice(element);
                element.RoomId = room.RoomId;
            }
            record.Reverse();
            return record;
        }

        public async Task<RecordViewModel> SaveRecord(RecordReceivedModel record, string imageRoot)
        {
            var device = unitOfWork.DeviceRepository.GetDeviceCode(record.DeviceCode);
            if (device == null)
            {
                throw new BaseException(ErrorMessages.DEVICE_IS_NOT_EXISTED);

            }
            //Get room from device is placed
            var room = unitOfWork.RoomDeviceRepository.Get(rd => rd.DeviceId == device.DeviceId
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
            var shift = unitOfWork.ShiftRepository.GetShiftByRoomDate(room.RoomId, DateTime.Now);
            if (shift == null)
            {
                throw new BaseException(ErrorMessages.SHIFT_IS_NOT_EXISTED);
            }

            var worker = unitOfWork.UserRepository.Find(shift.Arrangement.UserId);

            var recordEntity = new Record
            {
                UserId = worker.UserId,
                DeviceId = device.DeviceId,
                Type = record.Type,
                RecordDateTime = DateTime.Now,
                Status = Constant.SLEEPY
            };
            var notification = this.unitOfWork.NotificationRepository.CreateNotification(recordEntity, room, manager, worker, device.DeviceCode);
            var notificationFCM = new NotificationFCMViewModel
            {
                Title = notification.MessageTitle,
                Message = notification.MessageContent,
                DeviceToken = manager.DeviceToken,
                NotificationVM = notification.ToViewModel<NotificationViewModel>()
            };
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
                    await unitOfWork.NotificationRepository.AddAsync(notificationFCM.NotificationVM.ToEntity<Notifications>());
                    transaction.Commit();
                    this.SendNotify(notificationFCM);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }



            return recordEntity.ToViewModel<RecordViewModel>(); ;
        }

        //public async Task<RecordViewModel> SaveRecordByte(RecordReceivedByteModel record, string imageRoot)
        //{
        //    RecordViewModel result;
        //    var device = unitOfWork.DeviceRepository.GetDeviceCode(record.DeviceCode);
        //    if (device == null)
        //    {
        //        throw new BaseException(ErrorMessages.DEVICE_IS_NOT_EXISTED);
        //    }
        //    var recordEntity = new Record
        //    {
        //        DeviceId = device.DeviceId,
        //        Type = record.Type,
        //        RecordDateTime = DateTime.Now,

        //    };
        //    var notificationFCM = this.CreateNotificationFCM(recordEntity, device.DeviceCode);
        //    if (record.Image.Length <= 0)
        //    {
        //        throw new BaseException("Image is not existed!");
        //    }
        //    if (!Directory.Exists(imageRoot))
        //    {
        //        Directory.CreateDirectory(imageRoot);

        //    }
        //    var dir = new DirectoryInfo(imageRoot);
        //    var imagePath = imageRoot + "image" + dir.GetFiles().Length + ".jpg";
        //    using (Image image = Image.FromStream(new MemoryStream(record.Image)))
        //    {
        //        image.Save(imagePath, ImageFormat.Jpeg);  // Or Png
        //        recordEntity.Image = imagePath;
        //    }

        //    using (var transaction = unitOfWork.CreateTransaction())
        //    {
        //        try
        //        {
        //            //save record
        //            await recordRepository.AddAsync(recordEntity);
        //            //save noti
        //            unitOfWork.NotificationRepository.Add(notificationFCM.NotificationVM.ToEntity<Notifications>());
        //            transaction.Commit();
        //            result = recordEntity.ToViewModel<RecordViewModel>();
        //        }
        //        catch (Exception e)
        //        {
        //            transaction.Rollback();
        //            throw e;
        //        }
        //    }
        //    this.SendNotify(notificationFCM);

        //    return result;
        //}

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
            var room = this.unitOfWork.RoomDeviceRepository.Get(rd => rd.DeviceId == recordEntity.DeviceId, null, "Room")
                                                               .Where(rd => rd.Room != null)
                                                               .Select(rd => rd.Room).FirstOrDefault();
            if (room == null)
            {
                throw new BaseException(ErrorMessages.ROOM_IS_NOT_EXISTED);
            }
            var arrangement = this.unitOfWork.ShiftRepository.Get(s => s.RoomId == room.RoomId
                                                           && s.Date.Value.Date.CompareTo(recordEntity.RecordDateTime.Value.Date) == 0
                                                           && s.IsActive == true, null, "Arrangement")
                                                           .Where(s => s.Arrangement != null)
                                                           .Select(s => s.Arrangement).FirstOrDefault();
            if (arrangement == null)
            {
                throw new BaseException(ErrorMessages.ARRANGEMENT_IS_NOT_EXISTED);
            }
            var user = this.unitOfWork.UserRepository.Find(arrangement.UserId);
            recordVM.FullName = user.FullName;
            recordVM.RoomCode = room.RoomCode;
            recordVM.ImageByte = File.ReadAllBytes(recordEntity.Image);

            return recordVM;
        }

        public RecordViewModel DenyRecordStatusWorker(int userID, RecordStatusModel recordUpdateStatus)
        {
            var result = new RecordViewModel();
            var userRepo = unitOfWork.UserRepository;
            var roomRepo = unitOfWork.RoomRepository;
            var record = recordRepository.Find(recordUpdateStatus.RecordId);
            if (record == null)
            {
                throw new BaseException(ErrorMessages.RECORDID_IS_NOT_EXISTED);
            }
            if (record.UserId != userID)
            {
                throw new BaseException(ErrorMessages.RECORD_USER_NOT_RELATED);
            }
            record.Status = Constant.DENY_SLEEPY;
            record.Comment = recordUpdateStatus.Comment;
            recordRepository.Update(record);
            result = record.ToViewModel<RecordViewModel>();
            var roomRecord = roomRepo.GetRoomFromDevice(result);
            result.RoomId = roomRecord.RoomId;
            result.RoomCode = roomRecord.RoomCode;
            return result;
        }
    }
}
