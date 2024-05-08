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

        public DbSet<ParkedVehicle> ParkedVehicle { get; set; } = default!;
        public DbSet<VehicleType> VehicleType { get; set; } = default!;
        public DbSet<Member> Member { get; set; } = default!;
        public DbSet<GarageZone> GarageZone { get; set; } = default!;
    }
    
}
