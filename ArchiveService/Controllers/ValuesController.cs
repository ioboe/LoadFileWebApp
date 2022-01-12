using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace ArchiveService.Controllers
{
    public class ValuesController : ApiController
    {
        public HttpResponseMessage Post()
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count != 1)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            //var name = httpRequest.Params["FileId"];
            var postedFile = httpRequest.Files[0];
            var filePath = HttpContext.Current.Server.MapPath("~/ArchiveFiles/" + postedFile.FileName);
            var zipFilePath = $"{filePath}.zip";


            postedFile.SaveAs(filePath);

            using (var archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
            {
                archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
            }

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        public HttpResponseMessage GetFile(string name)
        {
            if (String.IsNullOrEmpty(name))
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            var localFilePath = HttpContext.Current.Server.MapPath($"~/ArchiveFiles/{name}");

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = name;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");

            return response;
        }
    }
}
