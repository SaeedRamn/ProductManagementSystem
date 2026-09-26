using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagementSystem.ViewModels
{
    public class AddProductViewModel
    {
        [Required]
        [Remote("IsProductNameExist", "Product", HttpMethod = "Post", ErrorMessage = "Name Is Already Exists")]
        public string Name { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "Min value equal 1 and Max Value equal 500000")]
        public decimal Price { get; set; }
        [NotMapped]
        public List<IFormFile>? Files { get; set; }
        public int CategoryId { get; set; }
    }
}