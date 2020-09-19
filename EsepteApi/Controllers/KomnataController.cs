using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using System.Net.Http;

namespace EsepteApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KomnataController : ControllerBase
    {
        IWebHostEnvironment _appEnvironment;
        private static readonly HttpClient client = new HttpClient();

        public KomnataController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        // для распознования типа помещения на фотографии
        [HttpGet]
        public async Task<string> Get(string imageName)
        {
            // получить распознование
            var response = await client.GetAsync("http://localhost:6012/komnata/" + imageName);
            var responseString = await response.Content.ReadAsStringAsync();


            return responseString;
        }

        //// для распознования типа помещения на фотографии
        //[HttpPost]
        //public async Task<IActionResult> Recognize(IFormFileCollection uploads)
        //{
        //    // сохранить файлы
        //    List<Komnata> komnatas = await SaveFiles(uploads);

        //    // получить распознование


        //    return RedirectToAction("Index");
        //}

        //// сохранения файла
        //private async Task<List<Komnata>> SaveFiles(IFormFileCollection uploads)
        //{
        //    List<Komnata> komnatas = new List<Komnata>();

        //    foreach (var uploadedFile in uploads)
        //    {
        //        // путь к папке Files
        //        string path = @"C:\Users\esept\AppData\Local\Packages\CanonicalGroupLimited.UbuntuonWindows_79rhkp1fndgsc\LocalState\rootfs\home\apollo\EsepteKomnataUploads\" + uploadedFile.FileName;

        //        // сохраняем файл в папку Files в каталоге wwwroot
        //        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
        //        {
        //            await uploadedFile.CopyToAsync(fileStream);
        //        }
        //        komnatas.Add(new Komnata { Name = uploadedFile.FileName, Path = path });
        //    }

        //    return komnatas;
        //}


    }
}
