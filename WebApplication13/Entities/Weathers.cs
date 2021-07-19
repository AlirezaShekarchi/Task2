using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication13.Entities
{
    public class Weathers
    {
        public coord coord { get; set; }
        public List <weather> Weather { get; set; }
        public main main { get; set; }
        public string description { get; set; }
        public string name { get; set; }
    }
    public class coord
    {
        public string lon { get; set; }
        public string lat { get; set; }
    }
    public class weather
    {
        public string main { get; set; }

    }
    public class main
    {
        public string temp_min { get; set; }
        public string temp_max { get; set; }
    }
}
