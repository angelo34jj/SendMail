using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;

namespace API
{
    public static class Untilities
    {
        public static KeyValuePair<bool, string> SenMail(MailRequest request)
        {
            try
            {
                var sendResponse = SendWithSendGrid(request);

                if (sendResponse != "Accepted")
                {
                    //send using the other one
                    sendResponse = SendWithMailGun(request);

                    if (sendResponse != "Accepted")
                    {
                        return new KeyValuePair<bool, string>(false, "Unable to send mail");
                    }
                    else
                    {
                        return new KeyValuePair<bool, string>(true, "");
                    }
                }
                else
                {
                    return new KeyValuePair<bool, string>(true, "");
                }
            }
            catch (Exception exGrid)
            {
                // log exception 
                try
                {
                    //send using the other method
                    var mailGunResponse = SendWithMailGun(request);
                    if (mailGunResponse == "Accepted")
                    {
                        return new KeyValuePair<bool, string>(true, "") ;
                    }
                    else
                    {
                        return new KeyValuePair<bool, string>(false, "Unable to send mail");
                    }
                }
                catch (Exception exGun)
                {
                    //log exception
                    return new KeyValuePair<bool, string>(false, "Unable to send mail");
                }
            }
          
        }

        private static string SendWithSendGrid(MailRequest request)
        {
            StringBuilder sbCc = new StringBuilder("");
            if (!string.IsNullOrEmpty(request.Cc))
            {
                sbCc.Append(",\"cc\": [");
                foreach (string cc in request.Cc.Split(','))
                {
                    {
                        sbCc.Append("{\"email\": \"" + cc
                 + "\"},");
                    }
                    sbCc.Append("]");
                }
            }

            if (!string.IsNullOrEmpty(request.Bcc))
            {
                sbCc.Append(",\"bcc\": [");
                foreach (string bcc in request.Bcc.Split(','))
                {
                    {
                        sbCc.Append("{\"email\": \"" + bcc
                 + "\"},");
                    }
                    sbCc.Append("]");
                }
            }

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.sendgrid.com/v3/mail/send");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Authorization", "Bearer xxxxxxxxxxxxxxxxxxxxx");
            // above xxxxx should be replace with your id, also would be good to have this in config than hard coding

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"personalizations\": ["
                    + "{\"to\": [{\"email\": \"" + request.To
                    + "\"}]"
                       + sbCc.ToString()
                    + "}]"
                    + ",\"from\": {\"email\": \"angelo34jj@yahoo.com\"},\"subject\": \"" +
                    request.Subject + "\",\"content\": [{\"type\": \"text/plain\", \"value\": \"" +
                    request.Message + "\"}]}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            return httpResponse.StatusCode.ToString();
        }

        private static string SendWithMailGun(MailRequest request)
        {
            string url = "https://api.mailgun.net/v3/sandbox0907136a6f2b4518a84b6e3a9bf786d6.mailgun.org/messages";
            using (var client = new WebClient())
            {
                client.Headers.Add("Authorization", "Basic xxxxxxxxxxxxxxxxxxxxxxxx");
                // above xxxxx should be replace with your id, also would be good to have this in config than hard coding
                var data = new NameValueCollection();
                data.Add("from", "Mailgun Sandbox <postmaster@sandbox0907136a6f2b4518a84b6e3a9bf786d6.mailgun.org>");
                if(string.IsNullOrEmpty(request.Cc))
                data.Add("to", request.To);
                if (!string.IsNullOrEmpty(request.Cc))
                {
                    data.Add("cc", request.Cc);
                }
                if (!string.IsNullOrEmpty(request.Bcc))
                {
                    data.Add("bcc", request.Bcc);
                }
                data.Add("subject", request.Subject);
                data.Add("text", request.Message);
                var result = client.UploadValues(url, data);
                //return Encoding.ASCII.GetString(result);

                JObject jResponse = JObject.Parse(Encoding.ASCII.GetString(result));
                var message = jResponse["message"].ToString();

                return (message == "Queued. Thank you.") ? "Accepted" : message;

            }
        }
    }
}