using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
    [Table("ClientsCatalogImages")]
    public class Product
    {
        [Key]
        public long ImageId { get; set; }
        public long? FileSize { get; set; }
        public bool? ToSite { get; set; }
        public bool? CatSlider { get; set; }
        public int ProductType {  get; set; }
        public string? Category { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        public string? Description { get; set; }
        public string? FileName { get; set; }
        public long ConfigId { get; set; }
        public string? Sizes { get; set; }
        public string? Material { get; set; }
        public bool? Basic { get; set; }
		public bool? Latest { get; set; }
	}
}
