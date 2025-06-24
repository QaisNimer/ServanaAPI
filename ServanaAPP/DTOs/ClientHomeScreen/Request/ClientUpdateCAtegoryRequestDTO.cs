namespace ServanaAPP.DTOs.ClientHomeScreen.Request
{
    public class ClientUpdateCAtegoryRequestDTO
    {
        public int id { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public IFormFile ?CategoryImage { get; set; }
    }
}
