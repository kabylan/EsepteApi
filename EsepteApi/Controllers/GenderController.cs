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
using EsepteApi.MachineLearningGender.Model;

namespace EsepteApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenderController : ControllerBase
    {
        IWebHostEnvironment _appEnvironment;

        public GenderController(IWebHostEnvironment appEnvironment)
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
                case "Female":
                    typeRU = "Ж";
                    break;
                case "Male":
                    typeRU = "М";
                    break;
                default:
                    typeRU = "-";
                    break;
            }

            return "{\"typeRU\": \"" + typeRU + "\" }";
        }

    }
}
