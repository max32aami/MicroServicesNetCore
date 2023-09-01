using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;

        //whenever in constructor its dependency injection and should be defined in program.cs to access globally
        public CouponAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _response = new ResponseDTO();
            _mapper = mapper;
        }
        [HttpGet]
        public ResponseDTO Get() 
        {
            try
            {
                IEnumerable<Coupon> objList = _db.Coupons.ToList();
                //_response.Result = objList;
                _response.Result = _mapper.Map<IEnumerable<CouponDTO>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpGet]
        [Route("{id:int}")]
        public ResponseDTO Get(int id)
        {
            try
            {
                Coupon? Obj = _db.Coupons.FirstOrDefault(p => p.CouponId == id);

                //_response.Result = Obj;
                _response.Result = _mapper.Map<CouponDTO>(Obj);

            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetByCode/:{code}")]
        public ResponseDTO GetByCode(string code)
        {
            try
            {
                Coupon? Obj = _db.Coupons.FirstOrDefault(p => p.CouponCode.ToLower() == code.ToLower());
                _response.Result = _mapper.Map<CouponDTO>(Obj);
                if (Obj == null) 
                {
                    _response.IsSucess = false;
                }

            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO Post([FromBody] CouponDTO couponDTO)
        {
            try
            {
                Coupon Obj = _mapper.Map<Coupon>(couponDTO);
               _db.Coupons.Add(Obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<Coupon>(Obj);
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")] 
        public ResponseDTO put([FromBody] CouponDTO couponDTO)
        {
            try
            {
                Coupon Obj = _mapper.Map<Coupon>(couponDTO);
                _db.Coupons.Update(Obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<Coupon>(Obj);
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO Delete(int id)
        {
            try
            {
                Coupon? Obj = _db.Coupons.FirstOrDefault(p => p.CouponId == id);
                _db.Coupons.Remove(Obj);
                _db.SaveChanges();
                _response.IsSucess=true;
                _response.Message = "Record Deleted Successfuly";
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
