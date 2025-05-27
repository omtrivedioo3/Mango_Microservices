using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Mongo.Web.Service.IService;
using Newtonsoft.Json;

namespace Mongo.Web.Controllers
{
    public class ProductController : Controller
    {
        public IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? list = new();
            ResponseDto? response = await _productService.GetAllProductsAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Messages;
            }

            return View(list);

        }
        public async Task<IActionResult> ProductCreate()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.CreateProductsAsync(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction("ProductIndex");
                }
                else
                {
                    TempData["error"] = response?.Messages;
                }
            }
            return View();
        }

        public async Task<IActionResult> ProductEdit(int ProductId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(ProductId);
            if (response != null && response.IsSuccess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Messages;
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.UpdateProductsAsync(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction("ProductIndex");
                }
                else
                {
                    TempData["error"] = response?.Messages;
                }
            }
            return View(model);
        }
        public async Task<IActionResult> ProductDelete(int ProductId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(ProductId);
            if (response != null && response.IsSuccess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Messages;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto ProductDto)
        {
            ResponseDto? response = await _productService.DeleteProductsAsync(ProductDto.ProductId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product Deleted successfully";
                return RedirectToAction("ProductIndex");
            }
            else
            {
                TempData["error"] = response?.Messages;
            }
            return View(ProductDto);
        }
    }
}

