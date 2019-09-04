﻿using System;
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

        public string geonamedevice { get; set; } //имя устройства
        public string geoDT { get; set; } // дата получения координат
        public string geoTM { get; set; } // время получения координат
    }

    //public class geoUser
    //{
    //    public int Id { get; set; }
    //    public string username { get; set; } //имя пользователя
    //    public string dt { get; set; } //дата

    //}
}