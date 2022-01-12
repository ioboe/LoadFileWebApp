using LoadFileWebApp.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace LoadFileWebApp.Controllers
{
    public class HomeController : Controller
    {
        //this should be changed to DependecyInjection
        private static DataAccessLayer dal = new DataAccessLayer();
        
        private static string archiveServerUrl = "http://localhost:64647/api/Values/";
        public ActionResult Index()
        {
            ViewBag.ArchiveServerUrl = archiveServerUrl;

            return View();
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {
            var file = HttpContext.Request.Files[0];

            var startTime = DateTime.Now;
            HttpResponseMessage result = null;

            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    var target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    byte[] Bytes = target.ToArray();


                    file.InputStream.Read(Bytes, 0, Bytes.Length);
                    var fileContent = new ByteArrayContent(Bytes);
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName };


                    content.Add(fileContent);

                    content.Add(new StringContent("123"), "FileId");
                    //content.Headers.Add("Key", "abc23sdflsdf");
                    result = client.PostAsync(archiveServerUrl, content).Result;


                }
            }

            var duration = DateTime.Now - startTime;

            dal.LogFileArchived(
                new FileLog 
                {
                    Name = file.FileName, 
                    Durtation = duration, 
                    StartTime = startTime, 
                    Status = result.StatusCode.ToString()
                });

            return Json(new { status = (result.StatusCode == System.Net.HttpStatusCode.Created) ? "success" : "error" }, JsonRequestBehavior.AllowGet);
        }


        
    }
}