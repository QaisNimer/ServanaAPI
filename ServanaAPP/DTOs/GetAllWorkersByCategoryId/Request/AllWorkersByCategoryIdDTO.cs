namespace ServanaAPP.DTOs.GetAllWorkersByCategoryId.Request
{
    public class AllWorkersByCategoryIdDTO
    {
        public string FullName { get; set; }
        public string? ProfileImage { get; set; }
        public decimal? PricePerHour { get; set; }
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }

    }
}
