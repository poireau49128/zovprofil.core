using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
    [Table("ShopAddresses")]
    public class Dealer
    {
        [Column("ShopAddressID")]
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Site { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Long { get; set; }
        public string? Worktime { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool isFronts { get; set; }
        public bool isProfile { get; set; }
        public bool isFurniture { get; set; }
    }
}
