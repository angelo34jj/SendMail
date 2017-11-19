using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API
{
    public class CommsController : ApiController
    {
        [HttpPost]
        public IHttpActionResult SendMail(MailRequest request)
        {

            var response = Untilities.SenMail(request);

            if (response.Key)
            {
                return Ok();
            }
            else
            {
                IHttpActionResult result = InternalServerError(new Exception("Unable to send mail. Please try again later."));
                return result;
            }

        }

     


    }
}
