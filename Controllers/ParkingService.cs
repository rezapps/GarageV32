using GarageV32.Data;
using GarageV32.Models;

namespace GarageV32;

public class ParkingService
{
    private readonly GarageContext _context;

    public ParkingService(GarageContext context)
    {
        _context = context;
    }

    public decimal CalculateParkingFee(ParkedVehicle vehicle)
    {
        var duration = (DateTime.Now - vehicle.EnteryTime).TotalMinutes;
        var baseRate = 50m; // Example rate
        var hourlyRate = 20m; // Example rate

        // Example calculation
        return baseRate + hourlyRate * (decimal)(duration / 60);
    }
}
