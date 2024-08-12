using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
	[Table("News")]
	public class New
	{
		[Key]
		public long NewsId { get; set; }
		public string? HeaderText { get; set; }
		public string? BodyText { get; set; }
		public DateTime? DateTime { get; set; }
	}
}
