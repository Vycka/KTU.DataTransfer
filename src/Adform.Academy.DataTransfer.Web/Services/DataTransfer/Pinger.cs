using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;

namespace Adform.Academy.DataTransfer.Web.Services.DataTransfer
{
    public class Pinger
    {
        public static string Ping()
        {
            return ServiceClient.PostRequest("Ping", string.Empty);
        }

        public static string PingError()
        {
            return ServiceClient.PostRequest("Ping3", string.Empty);
        }
    }
}