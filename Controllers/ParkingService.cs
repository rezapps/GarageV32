
using System;
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
        var durationInMinutes = (DateTime.Now - vehicle.EnteryTime).TotalMinutes;
        decimal baseRate;
        decimal minuteRate;

        switch (vehicle.VehicleType?.Size)
        {
            case 1:
                baseRate = 10m;
                minuteRate = 1m;
                break;
            case 2:
                baseRate = 13m;
                minuteRate = 1.4m;
                break;
            case 3:
            default:
                baseRate = 16m;
                minuteRate = 1.5m;
                break;
        }

        var fee = baseRate + minuteRate * (decimal)durationInMinutes;
        return Math.Round(fee, 2);
    }
    public double CalculateTotalMinutes(ParkedVehicle vehicle)
    {
        var durationInMinutes = (DateTime.Now - vehicle.EnteryTime).TotalMinutes;
        return Math.Round(durationInMinutes, 2);
    }

    public bool IsSpotAvailable(int spotNumber)
        {
            return !_context.ParkedVehicle.Any(v => v.SpotNumber == spotNumber);
        }

        public ParkedVehicle FindVehicleByRegNr(string regNr)
        {
            return _context.ParkedVehicle.FirstOrDefault(v => v.RegNr == regNr);
        }
   
}