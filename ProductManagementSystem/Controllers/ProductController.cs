using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagementSystem.Models;
using ProductManagementSystem.Service.Interfaces;
using ProductManagementSystem.ViewModels;

namespace ProductManagementSystem.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IFileService _fileService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IFileService fileService, ICategoryService categoryService, IMapper mapper)
        {
            _productService = productService;
            _fileService = fileService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProducts();
            ViewBag.Count = products.Count();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["categories"] = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddProductViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = _mapper.Map<Product>(model);
            
                    var result = await _productService.AddProduct(product, model.Files);
                    if (result != "Success")
                    {
                        ModelState.AddModelError(string.Empty, result);
                        ViewData["categories"] = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name");
                        return View(model);
                    }

                    return RedirectToAction(nameof(Index));
                }

                ViewData["categories"] = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["categories"] = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewData["categories"] = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Product model)
        {
            try
            {
                if (id != model.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var result = await _productService.UpdateProduct(model);
                    if (result != "Success")
                    {
                        ModelState.AddModelError(string.Empty, result);
                        ViewData["categories"] = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name", model.CategoryId);
                        return View(model);
                    }

                    return RedirectToAction(nameof(Index));
                }

                ViewData["categories"] = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name", model.CategoryId);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["categories"] = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name", model.CategoryId);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> IsProductNameExist(string name, int id)
        {
            var exists = await _productService.IsProductNameExist(name, id);
            if (exists)
            {
                return Json($"Name '{name}' Is Already Exists");
            }
            return Json(true);
        }
    }
}