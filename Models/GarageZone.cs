using System.ComponentModel;
using System.ComponentModel.DataAnnotations;namespace GarageV32.Models;

public class GarageZone
{
	[Key]
    [DisplayName("Zone ID")]
	public int Id { get; set; }
	public int Capacity { get; set; }
	public ICollection<ParkedVehicle>? ParkedVehicles { get; set; }
}
