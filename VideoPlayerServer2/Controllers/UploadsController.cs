using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace VideoPlayerServer2.Controllers
{
    public class UploadsController : ApiController
    {
        // POST api/values
        public HttpResponseMessage Post([FromBody]FileDetails req)
        {
            StorageUploader u = new StorageUploader();
            var re = u.Upload(req.fileName, req.fileSize, req.fileUploadedPath);

            return Request.CreateResponse(HttpStatusCode.OK, re);
        }
    }
}
