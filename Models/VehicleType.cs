using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace GarageV32.Models;

public class VehicleType
{
	[Key]
	public int Id { get; set; }
	[DisplayName("Type")]
	public string? Name { get; set; }
	[Range(1, 3, ErrorMessage = "Size is between 1 and 3 spots long.")]
	public int Size { get; set; }
	public string? Image { get; set; }

	// Navigation property
	public ICollection<ParkedVehicle>? ParkedVehicles { get; set; }
}
