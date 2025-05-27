//using Mango.Services.Mongo.Web.Models.Dto;
using Mango.Web.Models;

namespace Mongo.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetCouponsAsync(string couponCode);
        Task<ResponseDto?> GetAllCouponsAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto);
        Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto);
        Task<ResponseDto?> DeleteCouponAsync(int id);
    }
}
