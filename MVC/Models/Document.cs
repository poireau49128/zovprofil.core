namespace MVC.Models
{
    public class Document
    {
        public int DocumentId { get; set; }
        public string? Category { get; set; }
        public string? FileName { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? FileType { get; set; }
    }
}
