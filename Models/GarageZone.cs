using System.ComponentModel.DataAnnotations;
namespace GarageV32.Models;

public class GarageZone
{
	[Key]
	[Range(1, 4, ErrorMessage = "There are only 4 zones in the garage")]
	public int Id { get; set; }
	public int Capacity { get; private set; } = 30;
	public ICollection<ParkedVehicle>? ParkedVehicles { get; set; }
}
