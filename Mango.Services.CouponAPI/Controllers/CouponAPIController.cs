using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        public AppDbContext _db;
        public IMapper _mapper;

        public ResponseDto _response;

        public CouponAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> couponList = _db.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(couponList);
                //return _response;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
              Coupon couponList = _db.Coupons.First(x => x.CouponId == id);

                // Convert the Coupon object to CouponDto object 
                //CouponDto couponDto = new CouponDto()
                //{
                //    CouponId = id,
                //    CouponCode = couponList.CouponCode,
                //    MinAmount = couponList.MinAmount,
                //    DiscountAmount = couponList.DiscountAmount

                //};
                //_response.Result= couponDto;
                // Or use AutoMapper to map the Coupon object to CouponDto object
                _response.Result=_mapper.Map<CouponDto>(couponList);


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon coupon = _db.Coupons.First(x => x.CouponCode == code);

                _response.Result = _mapper.Map<CouponDto>(coupon);


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Add(coupon);
                _db.SaveChanges();
                _response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Update(coupon);
                _db.SaveChanges();
                _response.Result = _mapper.Map<CouponDto>(coupon);


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = ex.Message;
            }
            return _response;
        }


        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Coupon coupon = _db.Coupons.First(u=>u.CouponId==id);
                _db.Coupons.Remove(coupon);
                _db.SaveChanges();

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = ex.Message;
            }
            return _response;
        }
    }
}
