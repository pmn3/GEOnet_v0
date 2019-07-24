using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GEOnet.Models
{
    public class geoUser
    {
        public int Id { get; set; }
        public string username { get; set; } //имя пользователя
        public string namedevice { get; set; } //имя устройства

        public string dt { get; set; } //дата

        public string tm { get; set; } //время
    }
}
