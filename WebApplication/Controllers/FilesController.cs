using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using WebApplication.QX_Core.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class FileController : Controller
    {
        private IHostingEnvironment hostingEnv;
        string[] fileFormatArray = { "pdf"};
        public FileController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }

        [HttpPost]
        public IActionResult Post()
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            int flag = 0;

            //size > 100MB refuse upload !
            if (size > 104857600)
            {
                flag = 1;
                //return Json("pdf total size > 100MB , server refused !");
            }

            List<string> filePathResultList = new List<string>();

            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');

                string filePath = hostingEnv.WebRootPath + $@"\Files\Files\";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string suffix = fileName.Split('.')[fileName.Split('.').Length-1];
                if (!fileFormatArray.Contains(suffix))
                {
                    flag = 1;
                    //return Json("the file format not support ! you must upload files that suffix like 'pdf'.");
                }

                fileName = Guid.NewGuid() + "." + suffix;

                string fileFullName = filePath + fileName;

                using (FileStream fs = System.IO.File.Create(fileFullName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                filePathResultList.Add($"/src/Files/{fileName}");            
            }
            if (flag == 1)
            {
                return Json("Fail");
            }
            else
            {
                return Json(filePathResultList[0].Remove(0, 10));
            }
            
        }
    }

}