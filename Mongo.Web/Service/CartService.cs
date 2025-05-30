using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Mongo.Web.Models;
using Mongo.Web.Service.IService;

namespace Mango.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;
        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/ApplyCoupon"
            });
        }

        public async Task<ResponseDto?> EmailCart(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/EmailCartRequest"
            });
        }

        public async Task<ResponseDto?> GetCartByUserIdAsnyc(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ShoppingCartAPIBase + "/api/cart/GetCart/" + userId
            });
        }


        public async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDetailsId,
                Url = SD.ShoppingCartAPIBase + "/api/cart/RemoveCart"
            });
        }


        public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/CartUpsert"
            });
        }

        public async Task<ResponseDto?> GetWIshListByUserIdAsnyc(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ShoppingCartAPIBase + "/api/cart/GetWishList/" + userId
            });
        }

        public async Task<ResponseDto?> AddWIshListByUserIdAsnyc(int ProductId, string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Url = SD.ShoppingCartAPIBase + "/api/cart/InsertWishList/"+ ProductId + "/" + userId
            });
        }
        public async Task<ResponseDto?> RemoveWIshListByUserIdAsnyc(int ProductId,string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ShoppingCartAPIBase + "/api/cart/RemoveWishList/"+ ProductId + "/" + userId
            });
        }
    }
}