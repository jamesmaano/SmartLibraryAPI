namespace MauiApp1.Models
{
    public class Catalog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Catalog(string name, string description = "")
        {
            Name = name;
            Description = description;
        }
    }
}