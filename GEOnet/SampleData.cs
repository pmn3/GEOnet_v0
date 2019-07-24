using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GEOnet.Models;


namespace GEOnet
{
    public class SampleData
    {
        public static void Initialize(geoContext context)
        {
            DateTime localDate = DateTime.Now;
            if (!context.geoUsers.Any())
            {
                context.geoUsers.AddRange(
                    new geoUser
                    {
                        username = "test00",
                        namedevice="device00",
                        //tm = "00:00:00",
                        tm = localDate.ToString("HH:mm:ss"),
                        //dt = "00.00.00"
                        dt = localDate.ToString("dd.MM.yyyy")
                    }
               );
                context.SaveChanges();
            }

            if (!context.geoModels.Any())
            {
                context.geoModels.AddRange(
                    new geoModel
                    {
                        nameID = "test00",
                        X = 00.00,
                        Y = 00.00,
                        geonamedevice = "device00",
                        geoTM = localDate.ToString("HH:mm:ss"),
                        geoDT = localDate.ToString("dd.MM.yyyy")

                    }
               );
                context.SaveChanges();
            }            
        }
    }
}
