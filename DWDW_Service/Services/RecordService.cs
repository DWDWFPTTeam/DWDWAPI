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
        RecordViewModel SaveRecord(string deviceCode, string image);
        IEnumerable<RecordViewModel> GetRecordByLocationId(int locationId);
        IEnumerable<RecordViewModel> GetRecordsByLocationIdBetweenTime
            (int locationId, DateTime startDate, DateTime endDate);
        IEnumerable<RecordViewModel> GetRecordsByLocationIdAndTime
            (int locationId, DateTime date);
        void Notify(string deviceCode);
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
            var record = recordRepository.GetRecordsByLocationId(locationId)
                .Select(r => r.ToViewModel<RecordViewModel>());
            return record;
        }

        public IEnumerable<RecordViewModel> GetRecordsByLocationIdBetweenTime(int locationId, DateTime startDate, DateTime endDate)
        {
            var record = recordRepository
                .GetRecordsByLocationIdBetweenTime(locationId, startDate, endDate)
                .Select(r => r.ToViewModel<RecordViewModel>());
            return record;
        }

        public IEnumerable<RecordViewModel> GetRecordsByLocationIdAndTime(int locationId, DateTime date)
        {
            var record = recordRepository
                .GetRecordsByLocationIdAndTime(locationId, date)
                .Select(r => r.ToViewModel<RecordViewModel>());
            return record;
        }

        public RecordViewModel SaveRecord(string deviceCode, string image)
        {
            RecordViewModel result;
            var device = unitOfWork.DeviceRepository.GetDeviceCode(deviceCode);
            if (device != null)
            {
                using (var transaction = unitOfWork.CreateTransaction())
                {
                    try
                    {
                        
                        var record = new Record
                        {
                            DeviceId = device.DeviceId,
                            Image = image,
                            RecordDateTime = DateTime.Now,
                        };
                        //save record
                        recordRepository.Add(record);
                        result = record.ToViewModel<RecordViewModel>();

                        //save notify
                        var notificationRepo = unitOfWork.NotificationRepository;
                       

                        transaction.Commit();
                    }
                    catch (Exception e)
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

        public void Notify(string deviceCode)
        {
            var device = unitOfWork.DeviceRepository.GetDeviceCode(deviceCode);
            if(device == null)
            {
                throw new BaseException(ErrorMessages.DEVICE_IS_NOT_EXISTED);
            }
            //Get room from device is placed
            var room = unitOfWork.RoomDeviceRepository.Get(rd => rd.DeviceId == device.DeviceId
                                                                 && rd.IsActive == true, null, "Room")
                                                                .FirstOrDefault().Room;
            //Get Manager who manage the room
            var user = unitOfWork.ArrangementRepository.Get(a => a.LocationId == room.LocationId
                                                            && a.IsActive == true, null, "User")
                                                            .Select(a => a.User)
                                                            .Where(u => u.RoleId.ToString().Equals(Constant.MANAGER))
                                                            .FirstOrDefault();
            //Prepare message and destination
            var byteArray = generateNotify(user, room);

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

        private byte[] generateNotify(User user, Room room)
        {
            //string deviceToken = GetDeviceToken(deviceID);

            //prepare data for message
            var now = DateTime.Now.ToString("H:mm");
            var titleText = "Detect drowsiness!";
            var bodyText = "Worker " + user.UserName + " has a drowsiness in room: " + room.RoomCode + " at " + now;
            var data = new
            {
                data = new
                {
                    title = titleText,
                    message = bodyText
                },
                to = user.DeviceToken
            };

            var xc = JsonConvert.SerializeObject(data);
            var byteArray = Encoding.UTF8.GetBytes(xc);
            return byteArray;
        }
    }
}
