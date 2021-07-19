using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication13.Entities;

namespace WebApplication13.Services.HighTempCityService
{
   public interface IHighTempCityService
    {
        string  AddHighTempCity(HighTempCity highTempCity);
        HighTempCity GetHighTempCity(int Id);
    }
}
