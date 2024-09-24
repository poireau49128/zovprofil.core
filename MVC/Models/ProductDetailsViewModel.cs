using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Models
{
    public class ProductDetailsViewModel
    {
        public long ImageId { get; set; }
        public int ProductType { get; set; }
        public string? Sizes { get; set; }
        public string? Material { get; set; }
        public long ConfigId { get; set; }
        public int MatrixId { get; set; }
        public string? Category { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        public string? Description { get; set; }
        public string? FileName { get; set; }
        public string? techStoreFileName { get; set; }
        public IEnumerable<CategoryColors>? CategoryColors { get; set; }
        public Tuple<string, List<string>>? CatSlider { get; set; }
    }
}
