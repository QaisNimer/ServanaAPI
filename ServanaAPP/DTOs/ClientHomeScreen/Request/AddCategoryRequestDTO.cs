namespace ServanaAPP.DTOs.ClientHomeScreen.Request
{
    public class AddCategoryRequestDTO
    {
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public IFormFile ?CategoryImage { get; set; }
    }
}
