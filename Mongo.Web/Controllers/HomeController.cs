using System.Diagnostics;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mongo.Web.Models;
using Newtonsoft.Json;

namespace Mongo.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        public HomeController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }



        public async Task<IActionResult> Index()
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
        //[Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDto? list = new();
            ResponseDto? response = await _productService.GetProductByIdAsync(productId);
            if (response != null && response.IsSuccess) 
            {
                list = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Messages;
            }

            return View(list);

        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto, string submitAction)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == "sub")?.Value;

            if (submitAction == "cart")
            {
                CartDto cartDto = new CartDto
                {
                    CartHeader = new CartHeaderDto { UserId = userId },
                    CartDetails = new List<CartDetailsDto>
            {
                new CartDetailsDto
                {
                    ProductId = productDto.ProductId,
                    Count = productDto.Count
                }
            }
                };

                var response = await _cartService.UpsertCartAsync(cartDto);

                if (response?.IsSuccess == true)
                {
                    TempData["success"] = "Item has been added to the Shopping Cart";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response?.Messages;
                    return View(productDto);
                }
            }
            else if (submitAction == "wishlist")
            {
                ResponseDto? response = await _cartService.AddWIshListByUserIdAsnyc(productDto.ProductId, userId);
                TempData["success"] = "Item added to Wishlist (coming soon)";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Unknown action.";
            return View(productDto);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
