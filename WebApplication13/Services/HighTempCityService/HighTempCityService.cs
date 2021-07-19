using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication13.Entities;
using WebApplication13.Entities.Data;

namespace WebApplication13.Services.HighTempCityService
{
    public class HighTempCityService : IHighTempCityService
    {
        private readonly TaskDbContext db;
        public HighTempCityService(TaskDbContext _taskDbContext)
        {
            db = _taskDbContext;
        }
        public string AddHighTempCity(HighTempCity highTempCity)
        {
            try
            {
                db.highTempCities.Add(highTempCity);
                db.SaveChanges();
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public HighTempCity GetHighTempCity(int Id)
        {
            return db.highTempCities.Find(Id);
        }
    }
}
