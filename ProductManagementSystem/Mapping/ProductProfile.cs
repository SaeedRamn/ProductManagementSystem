using AutoMapper;
using ProductManagementSystem.Models;
using ProductManagementSystem.ViewModels;

namespace ProductManagementSystem.Mapping
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductViewModel, Product>();
        }
    }
}
