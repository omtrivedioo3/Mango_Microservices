using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Azure;
using Mango.Services.ShoppingCardAPI.Data;
using Mango.Services.ShoppingCardAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mango.Services.ShoppingCardAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly AppDbContext _db;
        private IConfiguration _configuration;
        private ICouponService _couponService;
        public IProductService _productService;
        private string url = "https://localhost:7000/api/product/";
        private readonly HttpClient client = new HttpClient();


        public CartAPIController(AppDbContext db, IProductService productService, ICouponService couponService,
            IMapper mapper, IConfiguration configuration)
        {
            _db = db;
            _productService = productService;
            this._response = new ResponseDto();
            _mapper = mapper;
            _configuration = configuration; 
            _couponService = couponService;
        }

        [HttpGet("GetWishList/{userId}")]
        public async Task<object> GetWishList(string userId)
        {
            try
            {
                IEnumerable<Product> productList = await _db.ProductDetails
           .Where(u => u.UserId == userId)
           .ToListAsync();
                //Product productList = await _db.ProductDetails.FirstOrDefaultAsync(u => u.UserId == userId);
                //IEnumerable<Product> productList = new() { 
                //    productList}
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(productList);
                //_response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
            }
            return _response;
        }

        [HttpPost("InsertWishList/{ProductId}/{userId}")]
        public async Task<object> InsertWishList(int ProductId, string userId)
        {
            try
            {
                using var httpClient = new HttpClient();
                //var response = await httpClient.GetAsync($"https://localhost:7777/api/product/{ProductId}");
                HttpResponseMessage response = client.GetAsync(url+ ProductId).Result;
                if (!response.IsSuccessStatusCode)
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Product not found: {ProductId}";
                    return _response;
                }

                var apiResponse = await response.Content.ReadAsStringAsync();
                var fullJson = JObject.Parse(apiResponse);

                // Step 2: Extract the "result" property
                var resultJson = fullJson["result"]?.ToString();
                var productDto = JsonConvert.DeserializeObject<ProductDto>(resultJson ?? "");

                if (productDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Product could not be deserialized.";
                    return _response;
                }

                Product product = _mapper.Map<Product>(productDto);
                product.UserId = userId;
                _db.ProductDetails.Add(product);
                _db.SaveChanges();

                _response.Result = product;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
            }

            return _response;
        }


        [HttpDelete("RemoveWishList/{ProductId}/{userId}")]
        public async Task<object> RemoveWishList(int ProductId,string userId)
        {
            try
            {

                Product obj = _db.ProductDetails.First(u => u.ProductId == ProductId & u.UserId == userId);
                _db.ProductDetails.Remove(obj);
                _db.SaveChanges();
                _response.Result = obj;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
            }
            return _response;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartHeader crdh = _db.CartHeaders.First(u => u.UserId == userId);
                CartDto cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(crdh)
                };
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_db.CartDetails
                    .Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId));

                IEnumerable<ProductDto> productDtos = await _productService.GetProducts();

                foreach (var item in cart.CartDetails)
                {
                    item.Product = productDtos.FirstOrDefault(u => u.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }

                //apply coupon if any
                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    CouponDto coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                    if (coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }

                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartFromDb = await _db.CartHeaders.FirstAsync(u => u.UserId == cartDto.CartHeader.UserId);
                cartFromDb.CouponCode = cartDto.CartHeader.CouponCode;
                _db.CartHeaders.Update(cartFromDb);
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
            }
            return _response;
        }


        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //create header and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();
                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                     var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cartDto.CartDetails.First().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cartDetailsFromDb == null)
                    {
                        //create cartdetails
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in cart details
                        cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _db.CartDetails
                   .First(u => u.CartDetailsId == cartDetailsId);

                int totalCountofCartItem = _db.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
                _db.CartDetails.Remove(cartDetails);
                if (totalCountofCartItem == 1)
                {
                    var cartHeaderToRemove = await _db.CartHeaders
                       .FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);

                    _db.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _db.SaveChangesAsync();

                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}
