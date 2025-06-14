namespace ServanaAPP.DTOs.Profile.Request
{
    public class UpdateProfileRequestDTO
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public string AddressTitle { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        // for workers only
        public decimal? PricePerHour { get; set; }
        public int CategoryID { get; set; }
    }
}
