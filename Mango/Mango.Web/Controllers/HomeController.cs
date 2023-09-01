using Mango.Web.Models;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        
		private readonly IProductService _productService;
		public HomeController(IProductService productService)
        {
			_productService = productService;
		}

        public async Task<IActionResult> Index()
        {
			List<ProductDto>? List = new();
			ResponseDto? response = await _productService.GetAllProductsAsync();
			if (response != null && response.IsSucess)
			{
				List = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
				TempData["success"] = response?.Message;
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return View(List);
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