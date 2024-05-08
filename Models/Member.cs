using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GarageV32.Models;

public class Member
{
	// Primary key or PK
	[Key]
	public int Id { get; set; }
	[DisplayName("First Name")]
	public required string FirstName { get; set; }
	[DisplayName("Last Name")]
	public required string LastName { get; set; }
	[RegularExpression(@"^[0-9]{8}-[0-9]{4}$", ErrorMessage ="Person Number must be in YYYYMMDD-XXXX format")]
	[DisplayName("Person Number")]
	public required string PersonNumber { get; set; }
	[DisplayName("Member ID")]
	public string? UserName { get; set; }

	[DisplayName("Member")]
	[DataType(DataType.Date)]
	public DateTime MemberSince { get; private set; } = DateTime.Now;

	public ICollection<ParkedVehicle>? ParkedVehicles { get; set; }
}
