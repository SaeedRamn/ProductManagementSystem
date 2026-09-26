using ProductManagementSystem.Models;
using ProductManagementSystem.ViewModels;

namespace ProductManagementSystem.Service.Interfaces
{
    public interface IProductService
    {
        public Task<List<Product>> GetProducts();
        public Task<Product?> GetProductById(int id);
        public Task<string> AddProduct(Product product, List<IFormFile>? files);
        public Task<string> UpdateProduct(Product product);
        public Task DeleteProduct(int id);
        public Task<bool> IsProductNameExist(string name, int id = 0);
    }
}