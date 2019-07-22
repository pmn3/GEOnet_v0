using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GEOnet.Models
{
    public class geoModel
    {
        public int Id { get; set; }
        public string nameID { get; set; } //имя пользователя - id
        public double X { get; set; } // координата X
        public double Y { get; set; } // координата Y
        public string geoDT { get; set; } // дата и время получения координат
    }

    public class geoUser
    {
        public int ID { get; set; }
        public string username { get; set; } //имя пользователя
        public string dt { get; set; } //дата

    }
}
