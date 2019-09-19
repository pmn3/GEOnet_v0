using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GEOnet.Models
{
    public class geoModel
    {
        public int Id { get; set; }
        public string NameUser { get; set; } //имя пользователя 
        public double LatitudeX { get; set; } // Широта
        public double LongitudeY { get; set; } // Долгота

        public string geonamedevice { get; set; } //имя устройства
        public string geoDT { get; set; } // дата получения координат
        public string geoTM { get; set; } // время получения координат
    }

}
