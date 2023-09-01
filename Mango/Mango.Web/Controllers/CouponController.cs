using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            this._couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? List = new();
            ResponseDto? response = await _couponService.GetAllCouponsAsync();
            if (response != null && response.IsSucess) 
            {
                List = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
                TempData["success"] = response?.Message;
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(List);

        }
		public async Task<IActionResult> CouponCreate()
		{
			return View();

		}
        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto model)
        {
           
            if(ModelState.IsValid)
            {
                ResponseDto? response = await _couponService.CreateCouponsAsync(model);
                if (response != null && response.IsSucess)
                {
                    TempData["success"] = response?.Message;
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }
        public async Task<IActionResult> CouponDelete(int CouponId)
        {
            
			ResponseDto? response = await _couponService.GetCouponByIdAsync(CouponId);
			if (response != null && response.IsSucess)
			{
				CouponDto? model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
                TempData["success"] = response?.Message;
                return View(model);
			}
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
            ResponseDto? response = await _couponService.DeletCouponAsync(couponDto.CouponId);
            if (response != null && response.IsSucess)
            {
                CouponDto? model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
                TempData["success"] = response?.Message;
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(couponDto);

        }
    }
}
