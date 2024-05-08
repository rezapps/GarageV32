namespace GarageV32.Models;
public class InvoiceViewModel
{
	public required string RegNr { get; set; }
	public int MemberId { get; set; }
	public DateTime EntryTime { get; set; }
	public DateTime ExitTime { get; set; }
	public double TotalMinutes => (ExitTime - EntryTime).TotalMinutes;
	public decimal Fee { get; set; }
}
