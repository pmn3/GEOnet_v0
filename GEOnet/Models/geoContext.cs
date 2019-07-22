using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GEOnet.Models
{
    public class geoContext : DbContext
    {
        public DbSet<geoModel> geoModels {get; set;}
        public DbSet<geoUser> geoUsers { get; set; }

        public geoContext(DbContextOptions<geoContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
