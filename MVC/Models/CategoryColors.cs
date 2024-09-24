using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class CategoryColors
    {
        public long ImageId { get; set; }
        public string? FileName { get; set; }
        public string? Color { get; set; }
    }
}
