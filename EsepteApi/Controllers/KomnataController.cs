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
        private static readonly string esepteKomnataUrl = "http://localhost:6012";

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

            System.Diagnostics.Debug.Print("Using model to make single prediction -- Comparing actual Label with predicted Label from sample data...\n\n");
            System.Diagnostics.Debug.Print($"ImageSource: {sampleData.ImageSource}");
            System.Diagnostics.Debug.Print($"\n\nPredicted Label value {predictionResult.Prediction} \nPredicted Label scores: [{String.Join(",", predictionResult.Score)}]\n\n");
            System.Diagnostics.Debug.Print("=============== End of process, hit any key to finish ===============");


            string komnataTypeRU = "";
            switch(predictionResult.Prediction)
            {
                case "bathroom":
                    komnataTypeRU = "ванная";
                    break;
                case "bedroom":
                    komnataTypeRU = "спальняя";
                    break;
                case "corridor":
                    komnataTypeRU = "коридор";
                    break;
                case "dining_room":
                    komnataTypeRU = "столовая";
                    break;
                case "elevator":
                    komnataTypeRU = "лифт";
                    break;
                case "garage":
                    komnataTypeRU = "гараж";
                    break;
                case "kitchen":
                    komnataTypeRU = "кухня";
                    break;
                case "livingroom":
                    komnataTypeRU = "гостиная";
                    break;
                case "office":
                    komnataTypeRU = "офис";
                    break;
                case "waitingroom":
                    komnataTypeRU = "приемная";
                    break;
                default:
                    komnataTypeRU = "-";
                    break;
            }

            return "{\"komnataType\": \"" + komnataTypeRU + "\" }";
        }

    }
}
