using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace GarageV32.Models;

public class GarageZone
{
	[Key]
    [DisplayName("Zone ID")]
	public int Id { get; set; }
	public required int Capacity { get; set; }

    public required string ZoneName { get; set; }

    [DisplayName("Occupied Spots")]
    public List<int>? OccupiedSpotsList { get; set; } = [];
	public ICollection<ParkedVehicle>? ParkedVehicles { get; set; }
}
