using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace VideoPlayerServer2.Controllers
{
    public class ViewController : ApiController
    {
        public void Get()
        {
            StorageUploader u = new StorageUploader();
            u.GetDetailsFromCosmosDBAsync();
            //return new string[] { "value1", "value2" };
        }
    }
}
