namespace ServanaAPP.Models
{
    public class Category
    {
        public int CategoryID { get; set; } // Identity (1,1)
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string? CategoryImage { get; set; }

        // Navigation Property
        public ICollection<User> Users { get; set; }
    }
}
