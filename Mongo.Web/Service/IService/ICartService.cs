﻿using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto?> GetCartByUserIdAsnyc(string userId);
        Task<ResponseDto?> UpsertCartAsync(CartDto cartDto);
        Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId);
        Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto);
        Task<ResponseDto?> EmailCart(CartDto cartDto);
        Task<ResponseDto?> AddWIshListByUserIdAsnyc(int ProductId, string userId);
        Task<ResponseDto?> GetWIshListByUserIdAsnyc(string userId);
        Task<ResponseDto?> RemoveWIshListByUserIdAsnyc(int ProductId, string userId);
    }
}