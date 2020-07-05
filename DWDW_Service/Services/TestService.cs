using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DWDW_Service.Services
{
    public interface ITestService
    {

    }
    public class TestService
    {
        private static string authorizationKey = "AAAA2d4Cw1E:APA91bEvIFr0lk6FOBIvwKnIn9qITwnfU7w15j2X0IBoREUCzmcwuLH-TmC93vlhhBlF1XwO170pc7I2HuEEYvHiqAeWr5f2pGiW3AuuNORcn1ikDdyjipHdBHVthG5qdeCoHLw6-v8_";
        public void SendNotification(byte[] byteArray)
        {
            try
            {
                string sender_id = "935732626257";
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "POST";
                //tRequest.ContentType = "";
                tRequest.ContentType = "application/json";
                tRequest.Headers.Add($"Authorization: key={authorizationKey}");
                //tRequest.Headers.Add($"Authorization: key={server_api_key}");
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
