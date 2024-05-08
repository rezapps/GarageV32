using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace GarageV32.Models;

public class ParkedVehicle
{
	// Primary key or PK
	[Key]
	public int Id { get; set; }

	[RegularExpression(@"^[a-zA-Z]{3}[0-9]{3}$", ErrorMessage = "Registeration Number is combination of 3 letters and 3 numbers. ex: ABC123")]
	[Display(Name = "Registeration Number")]
	public required string RegNr { get; set; }
    public enum VehicleColor
	{
		White,
		Black,
		Silver,
		Gray,
		Red,
		Blue,
		Brown,
		Green,
		Yellow
    }
	public VehicleColor Color { get; set; }
	[Range(2005, 2025, ErrorMessage = "Model Year must be between 2005 and 2025.")]
	public required int Year { get; set; }
	public required string Brand { get; set; }
	public required string Model { get; set; }
	public required int Wheels { get; set; }
	[DisplayName("Vehicle Type")]
	public string? Category { get; set; }
    [DisplayName("Entry Time")]
	public DateTime EnteryTime { get; private set; } = DateTime.Now;
	public DateTime? ExitTime { get; set; }

	[DisplayName("Zone")]
	public required int GarageZoneId { get; set; }
	[Range(1,30, ErrorMessage = "There are only 30 spots in this zone!")]
    [DisplayName("Parking Spot")]
	public required int SpotNumber { get; set; }
	[DisplayName("Member")]
	public int? MemberId { get; set; }
	public int? VehicleTypeId { get; set; }

	// Navigation properties
	public VehicleType? VehicleType { get; set; }
	public GarageZone? GarageZone { get; set; }
	public Member? Member { get; set; }
}
