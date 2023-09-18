
using CarModelsApi.Models;
using CarModelsApi.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CarModelsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarModelsController : ControllerBase
    {
        private readonly ILogger<CarModelsController> _logger;
        public IConfiguration _configuration { get; }

        public CarModelsController(ILogger<CarModelsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetCarModels")]
        public async Task<ActionResult> GetCarModels(string carMake, string carYear)
        {
            try
            {
                var producedCarModels = new List<string>();

                var url = new Uri(_configuration["ExternalApis:GetModelsForMakeIdYear"]);
                using (var client = new RestClient(url))
                {
                    var request = new RestRequest();

                    var carMakeId = GetCarMakeId(carMake);

                    request.AddParameter("makeId", carMakeId.ToString());
                    request.AddParameter("modelyear", carYear.ToString());
                    request.AddParameter("format", "json");

                    var response = await client.GetAsync(request);
                    var modelsForMakeIdYear = JsonConvert.DeserializeObject<GetModelsForMakeIdYearResponseModel>(response.Content);

                    foreach(var car in modelsForMakeIdYear.Results.DistinctBy(i => i.Model_Name))
                    {
                        producedCarModels.Add(car.Model_Name);
                    }
                }

                return Ok(new
                {
                    Models = producedCarModels
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        private int? GetCarMakeId(string carMakeName)
        {
            int? carMakeId = 0;

            var carMakeListQuery = System.IO.File.ReadAllLines("CarMake.csv")
                .Skip(1)
                .Where(l => l.Length > 0)
                .Select(CarMakeCsv.ParseCsvCarMakeToCarMake);

            carMakeId = carMakeListQuery.Where(i => i.make_name == carMakeName.ToLower()).FirstOrDefault()?.make_id;

            return carMakeId;
        }
    }
}
