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
using EsepteApi.MachineLearningNoticeProperty.Model;

namespace EsepteApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoticePropertyController : ControllerBase
    {
        IWebHostEnvironment _appEnvironment;

        public NoticePropertyController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

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


            string typeRU = "";
            switch (predictionResult.Prediction)
            {
                case "2d_plan":
                    typeRU = "2d планировака";
                    break;
                case "2d_plan_photo":
                    typeRU = "документ";
                    break;
                case "3d_plan":
                    typeRU = "3d планировака";
                    break;
                case "artwork":
                    typeRU = "логотип";
                    break;
                case "desk_bred_property":
                    typeRU = "спам АН";
                    break;
                case "desk_bred_text_and_photo":
                    typeRU = "спам";
                    break;
                case "document":
                    typeRU = "документ";
                    break;
                case "map":
                    typeRU = "карта";
                    break;
                case "poster":
                    typeRU = "спам";
                    break;
                case "real":
                    typeRU = "реальное";
                    break;
                default:
                    typeRU = "-";
                    break;
            }

            return "{\"typeRU\": \"" + typeRU + "\" }";
        }

    }
}

//2d_plan
//2d_plan_photo
//3d_plan
//artwork
//desk_bred_property
//desk_bred_text_and_photo
//document
//map
//poster
//real