using ProductManagementSystem.Service.Interfaces;

namespace ProductManagementSystem.Service.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public bool DeletePhysicalFile(string path)
        {
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + path);
            if (File.Exists(directoryPath))
            {
                File.Delete(directoryPath);
                return true;
            }
            return false;
        }

        public async Task<string> Upload(IFormFile file, string location)
        {
            if (file == null || file.Length <= 0)
                return "Problem";

            try
            {
                // شيل السلاشات الزيادة عشان نبني المسار صح
                var path = _webHostEnvironment.WebRootPath + location;
                var extension = Path.GetExtension(file.FileName);
                var fileName = Guid.NewGuid().ToString().Replace("-", string.Empty) + extension;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (FileStream fileStream = File.Create(path + fileName))
                {
                    await file.CopyToAsync(fileStream);
                    fileStream.Flush();
                    return $"{location}/{fileName}";
                }

            }
            catch (Exception ex)
            {
                return ex.Message + "--" + ex.InnerException;
            }
        }
    }
}