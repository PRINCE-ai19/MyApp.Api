using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MyApp.Application.Model_DTO;
using MyApp.Application.Resources;
using MyApp.Application.Store_Interface;

namespace MyApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStoreController : ControllerBase
    {
        private readonly IProductStoreService _productService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ProductStoreController(IProductStoreService productService, IStringLocalizer<SharedResource> localizer)
        {
            _productService = productService;
            _localizer = localizer;
        }


        [HttpGet("/Rotev2/Get/AllProduct")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await _productService.GetAllProducts();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = 0, message = _localizer[ex.Message].Value });
            }
        }

        [HttpGet("/Rotev2/Get/ProductsByCategory/{categoryId}")]
        public async Task<IActionResult> GetByCategoryId(int categoryId)
        {
            try
            {
                var data = await _productService.GetProductsByCategoryId(categoryId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = 0, message = _localizer[ex.Message].Value });
            }
        }

        [HttpPost("/Rotev2/Create/Product")]
        public async Task<IActionResult> Create([FromBody] Product_DTO productDto)
        {
            var result = await _productService.Create(productDto);

            if (result.Success)
            {
                return Ok(new { Success = 1, message = result.Message });
            }
            else
            {
                return BadRequest(new { Success = 0, message = result.Message });
            }
        }

        [HttpPost("/Rotev2/Update/Product/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product_DTO productDto)
        {
            var result = await _productService.Update(id, productDto);

            if (result.Success)
            {
                return Ok(new { Success = 1, message = result.Message });
            }
            else
            {
                return BadRequest(new { Success = 0, message = result.Message });
            }
        }

        [HttpPost("/Rotev2/Delete/Product/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.Delete(id);

            if (result.Success)
            {
                return Ok(new { Success = 1, message = result.Message });
            }
            else
            {
                return NotFound(new { Success = 0, message = result.Message });
            }
        }
    }
}
