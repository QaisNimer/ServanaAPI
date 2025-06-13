using ServanaAPP.Models;

namespace ServanaAPP.DTOs.ClientHomeScreen.Response
{
    public class TopRatedWorkerDTO
    {
        public User Worker { get; set; }
        public double AverageStars { get; set; }
        public int TotalRatings { get; set; }
    }
}
