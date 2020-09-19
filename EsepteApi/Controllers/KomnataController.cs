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
using System.Net;

namespace EsepteApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KomnataController : ControllerBase
    {
        IWebHostEnvironment _appEnvironment;
        private static readonly HttpClient client = new HttpClient();
        private static readonly string esepteKomnataUrl = "http://localhost:6012";

        public KomnataController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        // для распознования типа помещения на фотографии
        [HttpGet]
        public async Task<string> Get(string imageLink)
        {

            // путь к скачиванию файла
            string imageName = "Image_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss") + Path.GetExtension(imageLink);
            string path = "/Uploads/" + imageName;
            Debug.Print(path);

            // скачать картинку по ссылке
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(imageLink), _appEnvironment.WebRootPath + path);
            }

            // получить распознование
            var response = await client.GetAsync(esepteKomnataUrl + "/komnata/" + imageName);
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

    }
}
