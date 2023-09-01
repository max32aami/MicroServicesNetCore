using AutoMapper;
using Mango.Services.CouponAPI.Models.DTO;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;

        public ProductAPIController(IMapper mapper, AppDbContext db)
        {
            _response = new ();
            _mapper = mapper;
            _db = db;
        }

        [HttpGet]
        [Authorize]
        public ResponseDTO Get()
        {
            try
            {
                IEnumerable<Product> objList = _db.Product.ToList();
                //_response.Result = objList;
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
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
        [Authorize]
        public ResponseDTO Get(int id)
        {
            try
            {
                Product? Obj = _db.Product.FirstOrDefault(p => p.ProductId == id);

                //_response.Result = Obj;
                _response.Result = _mapper.Map<ProductDto>(Obj);

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
        public ResponseDTO Post([FromBody] ProductDto couponDTO)
        {
            try
            {
                Product Obj = _mapper.Map<Product>(couponDTO);
                _db.Product.Add(Obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<Product>(Obj);
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
        public ResponseDTO put([FromBody] ProductDto productDTO)
        {
            try
            {
                Product Obj = _mapper.Map<Product>(productDTO);
                _db.Product.Update(Obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<Product>(Obj);
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
                Product? Obj = _db.Product.FirstOrDefault(p => p.ProductId == id);
                _db.Product.Remove(Obj);
                _db.SaveChanges();
                _response.IsSucess = true;
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

