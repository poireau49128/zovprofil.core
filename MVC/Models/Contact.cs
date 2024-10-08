using static System.Collections.Specialized.BitVector32;

namespace MVC.Models
{
    public class Contact
    {
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string? Languages { get; set; }
        public string? Phone { get; set; }
        public string? Description { get; set; }
        public bool isMale { get; set; }
        public int SectionId { get; set; }
        public Section Section { get; set; }
    }

    public class Section
    {
        public int SectionId { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Fax { get; set; }
        public string? Phone { get; set; }
        public List<Contact>? Contacts { get; set; }
    }
}
