namespace backend.Models
{
    public class University
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public List<string> WebPages { get; set; }
        public List<string> Domains { get; set; }
    }
}