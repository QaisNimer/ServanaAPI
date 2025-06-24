namespace ServanaAPP.Helpers.ImageHelpers
{
    public class FileUploadHelper
    {
        public static async Task<string> UploadFileAsync(IFormFile file, string folderName = "Uploads")
        {
            if (file == null || file.Length == 0)
                return null;

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

            // Path: SolutionRoot/Uploads
            var solutionFolder = Directory.GetCurrentDirectory();
            var uploadPath = Path.Combine(solutionFolder, folderName);

            Directory.CreateDirectory(uploadPath);

            var fullPath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path for DB if needed
            return Path.Combine(folderName, fileName).Replace("\\", "/");
        }
    }
}
