using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Model_DTO;
using MyApp.Application.Services;

namespace MyApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _categoryService.GetListCategoryForUI();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category_DTO dto)
        {
            if (dto == null) return BadRequest("Dữ liệu không hợp lệ");

            await _categoryService.AddCategory(dto);

            return Ok(new { message = "Thêm danh mục thành công!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest("ID không khớp");

            await _categoryService.UpdateCategory(dto);

            return Ok(new { message = "Cập nhật danh mục thành công!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategory(id);

            if (!result)
            {
                return NotFound(new { message = "Không tìm thấy danh mục để xóa" });
            }

            return Ok(new { message = "Xóa danh mục thành công!" });
        }
    }
}
