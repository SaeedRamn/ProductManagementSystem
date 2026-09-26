namespace ProductManagementSystem.Service.Interfaces
{
    public interface IFileService
    {
        public Task<string> Upload(IFormFile file, string location);
        public bool DeletePhysicalFile(string path);
    }
}
