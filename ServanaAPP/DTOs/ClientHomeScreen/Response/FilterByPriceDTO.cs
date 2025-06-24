using ServanaAPP.Models;

namespace ServanaAPP.DTOs.ClientHomeScreen.Response
{
    public class FilterByPriceDTO
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public decimal? PricePerHour { get; set; }
        public string? ProfileImage { get; set; }
    }
}