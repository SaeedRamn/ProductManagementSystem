using ProductManagementSystem.Models;

namespace ProductManagementSystem.Service.Interfaces
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetCategoriesAsync();
    }
}
