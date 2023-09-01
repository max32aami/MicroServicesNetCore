using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
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
        public async Task<IActionResult> ProductCreate()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {

            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.CreateProductsAsync(model);
                if (response != null && response.IsSucess)
                {
                    TempData["success"] = response?.Message;
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }
        public async Task<IActionResult> ProductDelete(int ProductId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(ProductId);
            if (response != null && response.IsSucess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
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
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            ResponseDto? response = await _productService.DeletProductAsync(productDto.ProductId);
            if (response != null && response.IsSucess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                TempData["success"] = response?.Message;
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(productDto);

        }

		public async Task<IActionResult> ProductEdit(int productId)
		{
			ResponseDto? response = await _productService.GetProductByIdAsync(productId);

			if (response != null && response.IsSucess)
			{
				ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
				return View(model);
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> ProductEdit(ProductDto productDto)
		{
            //if (ModelState.IsValid)
            //{
            productDto.ImageUrl = "qqewqweqe";
				ResponseDto? response = await _productService.UpdateProductsAsync(productDto);

				if (response != null && response.IsSucess)
				{
					TempData["success"] = "Product updated successfully";
					return RedirectToAction(nameof(ProductIndex));
				}
				else
				{
					TempData["error"] = response?.Message;
				}
			//}
			return View(productDto);
		}
	}
}
