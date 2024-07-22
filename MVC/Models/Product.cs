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
        public int? ProductType {  get; set; }
        public string? Category { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        public string? Description { get; set; }
        public string? FileName { get; set; }

        [NotMapped]
        public string? TechFilename { get; set; }
        public long? ConfigId { get; set; }
        public string? Sizes { get; set; }
        public string? Material { get; set; }
        public bool? Basic { get; set; }
    }

    public class ClientsCatalogFrontsConfig
    {
        [Key]
        public long ConfigId { get; set; }
        public long FrontId { get; set; }
        public long ColorId { get; set; }
        public long TechnoColorId { get; set; }
        public long InsetTypeId { get; set; }
        public long InsetColorId { get; set; }
        public long TechnoInsetTypeID { get; set; }
        public long TechnoInsetColorID { get; set; }
        public long PatinaId { get; set; }

    }

    public class ClientsCatalogDecorConfig
    {
        [Key]
        public long ConfigId { get; set; }
        public long ProductId { get; set; }
        public long DecorId { get; set; }
        public long ColorId { get; set; }
        public long PatinaId { get; set; }
        public long InsetTypeId { get; set; }
        public long InsetColorId { get; set; }
    }

    public class TechStoreDocument
    {
        [Key]
        public long TechStoreDocumentID { get; set; }
        public long TechId { get; set; }
        public string? FileName { get; set; }
        public int DocType { get; set; }
        public long FileSize { get; set; }
    }
}
