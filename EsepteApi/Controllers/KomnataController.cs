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
using EsepteApi.MachineLearning.Model;

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

        //// для распознования типа помещения на фотографии
        //[HttpGet]
        //public async Task<string> Get(string imageLink)
        //{

        //    // путь к скачиванию файла
        //    string imageName = "Image_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss") + Path.GetExtension(imageLink);
        //    string path = "/Uploads/" + imageName;
        //    Debug.Print(path);

        //    // скачать картинку по ссылке
        //    using (WebClient client = new WebClient())
        //    {
        //        client.DownloadFile(new Uri(imageLink), _appEnvironment.WebRootPath + path);
        //    }

        //    // получить распознование
        //    var response = await client.GetAsync(esepteKomnataUrl + "/komnata/" + imageName);
        //    var responseString = await response.Content.ReadAsStringAsync();

        //    return responseString;
        //}

        [HttpGet]
        public string Get(string imageLink)
        {

            // путь к скачиванию файла
            string imageName = "Image_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss") + Path.GetExtension(imageLink);
            string path = _appEnvironment.WebRootPath + "/Uploads/" + imageName;
            Debug.Print(path);
            // скачать картинку по ссылке
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(imageLink), path);
            }
            // Получить распознование

            // Create single instance of sample data from first line of dataset for model input
            ModelInput sampleData = new ModelInput()
            {
                ImageSource = path,
            };

            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);

            string typeRU = "";
            switch(predictionResult.Prediction)
            {
                case "bathroom":
                    typeRU = "ванная";
                    break;
                case "bedroom":
                    typeRU = "спальняя";
                    break;
                case "corridor":
                    typeRU = "коридор";
                    break;
                case "dining_room":
                    typeRU = "столовая";
                    break;
                case "elevator":
                    typeRU = "лифт";
                    break;
                case "garage":
                    typeRU = "гараж";
                    break;
                case "kitchen":
                    typeRU = "кухня";
                    break;
                case "livingroom":
                    typeRU = "гостиная";
                    break;
                case "office":
                    typeRU = "офис";
                    break;
                case "waitingroom":
                    typeRU = "приемная";
                    break;
                default:
                    typeRU = "-";
                    break;
            }

            return "{\"typeRU\": \"" + typeRU + "\" }";
        }

    }
}
