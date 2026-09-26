using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Data;
using ProductManagementSystem.Models;
using ProductManagementSystem.Service.Interfaces;
using ProductManagementSystem.ViewModels;

namespace ProductManagementSystem.Service.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public ProductService(IFileService fileService, ApplicationDbContext context)
        {
            _fileService = fileService;
            _context = context;
        }

        public async Task<string> AddProduct(Product product, List<IFormFile>? files)
        {
            try
            {
                await _context.Product.AddAsync(product);
                await _context.SaveChangesAsync();
                
                if (files != null && files.Count() > 0)
                {
                    var pathList = new List<string>();
                    foreach (var file in files)
                    {
                        var path = await _fileService.Upload(file, "/images/");
                        if (!path.StartsWith("/images/"))
                        {
                            return path;
                        }
                        pathList.Add(path);
                    }


                    var productImages = new List<ProductImages>();
                    foreach (var file in pathList)
                    {
                        var productImage = new ProductImages();
                        productImage.ProductId = product.Id;
                        productImage.Path = file;
                        productImages.Add(productImage);
                    }

                    _context.ProductImages.AddRange(productImages);
                    await _context.SaveChangesAsync();
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message + "--" + ex.InnerException;
            }
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Product
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product != null)
            {
                foreach (var img in product.ProductImages)
                {
                    _fileService.DeletePhysicalFile(img.Path);
                }

                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Product
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Product
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<string> UpdateProduct(Product product)
        {
            try
            {
                var exProduct = await _context.Product.FirstOrDefaultAsync(x => x.Id == product.Id);
                if (exProduct == null)
                {
                    return "Problem";
                }

                exProduct.Name = product.Name;
                exProduct.Price = product.Price;
                exProduct.CategoryId = product.CategoryId;

                await _context.SaveChangesAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message + "--" + ex.InnerException;
            }
        }

        public async Task<bool> IsProductNameExist(string name, int id = 0)
        {
            return await _context.Product
                .AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.Id != id);
        }
    }
}   