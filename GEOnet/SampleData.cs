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
            if (!context.geoModels.Any())
            {
                context.geoModels.AddRange(
                    new geoModel
                    {
                        nameID = "test00",
                        X = 00.00,
                        Y = 00.00,
                        geoDT = "00.00.00.00.00"
                    }
               );
                context.SaveChanges();
            }

            if (!context.geoUsers.Any())
            {
                context.geoUsers.AddRange(
                    new geoUser
                    {
                        username = "test00",
                        dt = "00.00.00.00.00"
                    }
               );
                context.SaveChanges();
            }
        }
    }
}
