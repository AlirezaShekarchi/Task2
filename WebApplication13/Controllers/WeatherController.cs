using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApplication13.Entities;
using WebApplication13.Services.HighTempCityService;

namespace WebApplication13.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private  readonly IHighTempCityService _highTempCityService;
        public WeatherController(IHighTempCityService highTempCityService)
        {
            _highTempCityService = highTempCityService;
        }
        [HttpPost("GetWeather")]
        public void  GetWeather(string cityName, string apiKey)
        {
            Weathers WeatherInfo = new Weathers();
            RecurringJob.AddOrUpdate(() => GetWeatherRecurring(cityName, apiKey, WeatherInfo), Cron.Minutely);
        }
        [HttpPost]
        public async Task<Weathers> GetWeatherRecurring(string cityName, string apiKey, Weathers WeatherInfo)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.openweathermap.org");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("/data/2.5/weather?q=" + cityName + "&appid=" + apiKey);
                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    WeatherInfo = JsonConvert.DeserializeObject<Weathers>(EmpResponse);

                }
                if (double.Parse(WeatherInfo.main.temp_max) > 300)
                {
                    HighTempCity highTempCity = new HighTempCity();
                    highTempCity.CityName = WeatherInfo.name;
                    highTempCity.Temp = WeatherInfo.main.temp_max;
                    _highTempCityService.AddHighTempCity(highTempCity);
                }
            }

            return WeatherInfo;
        }
    }
}
