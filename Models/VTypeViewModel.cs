using Microsoft.AspNetCore.Mvc.Rendering;
namespace GarageV32.Models;

public class VTypeViewModel
{
	public List<ParkedVehicle>? ParkedVehicles { get; set; }
	public List<VehicleType>? VehicleTypes { get; set; }
	public SelectList? Categories { get; set; }
	public string? VehicleCategory { get; set; }
	public string? SearchString { get; set; }
}
