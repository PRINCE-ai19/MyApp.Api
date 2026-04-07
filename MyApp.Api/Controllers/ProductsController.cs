using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Model_DTO;
using MyApp.Application.Services;

namespace MyApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var data = await _productService.GetProductsByCategoryId(categoryId);

            if (data == null || !data.Any())
                return NotFound("Không có sản phẩm nào trong danh mục này");

            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _productService.GetAllProducts();
            if (data == null || !data.Any())
                return NotFound("Không có sản phẩm nào");
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product_DTO dto)
        {
            if (dto == null) return BadRequest("Dữ liệu không hợp lệ");
            await _productService.AddProduct(dto);
            return Ok(new { message = "Thêm sản phẩm thành công!" });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest("ID không khớp");
            await _productService.UpdateProduct(dto);
            return Ok(new { message = "Cập nhật sản phẩm thành công!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProduct(id);
            if (!result)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm để xóa" });
            }
            return Ok(new { message = "Xóa sản phẩm thành công!" });
        }
    }
}
