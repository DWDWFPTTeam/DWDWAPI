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
        IEnumerable<Record> GetRecordByLocationId(int locationId);
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

        public byte[] generateNotify(User user, Room room)
        {
            //string deviceToken = GetDeviceToken(deviceID);

            //prepare data for message
            var now = DateTime.Now.ToString("H:mm");
            var titleText = "Detect drowsiness!";
            var bodyText = "Worker " + user.UserName + " has a drowsiness in room: " + room.RoomCode + " at " + now;
            var data = new
            {
                notification = new
                {
                    title = titleText,
                    body = bodyText
                },
                to = user.DeviceToken
            };

            var xc = JsonConvert.SerializeObject(data);
            var byteArray = Encoding.UTF8.GetBytes(xc);
            return byteArray;
        }

        public IEnumerable<Record> GetRecordByLocationId(int locationId)
        {
            var record = recordRepository.GetRecordsByLocationId(locationId);
            return record;
        }

        public RecordViewModel SaveRecord(string deviceCode, string image)
        {
            RecordViewModel result;
            var device = unitOfWork.DeviceRepository.GetDeviceCode(deviceCode);
            if (device != null)
            {
                var record = SendNotification(device.DeviceId.Value, image);
                recordRepository.Add(record);
                result = record.ToViewModel<RecordViewModel>();
            }
            else
            {
                throw new BaseException(ErrorMessages.DEVICE_IS_NOT_EXISTED);
            }

            return result;
        }

        public Record SendNotification(int deviceID, string image)
        {
            //Get room from device is placed
            var room = unitOfWork.RoomDeviceRepository.Get(rd => rd.DeviceId == deviceID
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

            return new Record
            {
                DeviceId = deviceID,
                Image = image,
                RecordDateTime = DateTime.Now,
            };


        }
    }
}
