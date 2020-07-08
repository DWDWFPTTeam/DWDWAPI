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

namespace DWDW_Service.Services
{
    public interface IRecordService : IBaseService<Record>
    {
        void SendNotification(int deviceID);
    }

    public class RecordService : BaseService<Record>, IRecordService
    {
        private readonly IRecordRepository recordRepository;
        public RecordService(UnitOfWork unitOfWork, IRecordRepository recordRepository) : base(unitOfWork)
        {
            this.recordRepository = recordRepository;
        }

        public string GetDeviceToken(int deviceID)
        {
            var manager = recordRepository.GetDeviceToken(deviceID);
            var managerModel = manager.ToViewModel<UserViewModel>();
            string managerDeviceToken = manager.DeviceToken;
            return managerDeviceToken;
        }
        public byte[] generateNotify(int deviceID)
        {
            string deviceToken = GetDeviceToken(deviceID);
            string now = DateTime.Now.ToString("H:mm");

            string titleText = "Detect drowsiness!";
            string bodyText = "There was a drowsiness in " + room + " at " + now;
            var data = new
            {
                notification = new
                {
                    title = titleText,
                    body = bodyText
                },
                to = deviceToken
            };
            string xc = JsonConvert.SerializeObject(data);
            byte[] byteArray = Encoding.UTF8.GetBytes(xc);
            return byteArray;
        }

        public void SendNotification(int deviceID)
        {
            string authorizationKey = Constant.FIREBASE_AUTHORIZATION_KEY;
            string sender_id = Constant.FIREBASE_SENDER_ID;
            byte[] byteArray = generateNotify(deviceID);
            try
            {         
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                tRequest.Headers.Add($"Authorization: key={authorizationKey}");
                tRequest.Headers.Add($"Sender: id={sender_id}");

                tRequest.ContentLength = byteArray.Length;
                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse tResponse = tRequest.GetResponse();
                dataStream = tResponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStream);

                string tResponseFromServer = tReader.ReadToEnd();

                tReader.Close();
                dataStream.Close();
                tResponse.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
