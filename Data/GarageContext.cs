using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GarageV32.Models;

namespace GarageV32.Data
{
    public class GarageContext : DbContext
    {
        public GarageContext (DbContextOptions<GarageContext> options)
            : base(options)
        {
        }

        public DbSet<GarageV32.Models.ParkedVehicle> ParkedVehicle { get; set; } = default!;
        public DbSet<GarageV32.Models.VehicleType> VehicleType { get; set; } = default!;
        public DbSet<GarageV32.Models.Member> Member { get; set; } = default!;
        public DbSet<GarageV32.Models.GarageZone> GarageZone { get; set; } = default!;
    }
}
