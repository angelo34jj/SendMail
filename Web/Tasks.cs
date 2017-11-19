using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Web
{
    public static class Tasks
    {
        public static string SendMail(MailRequest request)
        {
            try
            {
                var json = new JavaScriptSerializer().Serialize(request);
                var apiUrl = System.Configuration.ConfigurationManager.AppSettings["APIUrl"];

                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = client.PostAsync("api/comms/sendmail", byteContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    return ("Mail sent successfully");
                }
                else
                {
                    return ("Mail send failed. Please try again later");
                }
            }
            catch (Exception ex)
            {
                //log ex
                return ("Mail send failed. Please try again later");
            }
            
        }

    }
}