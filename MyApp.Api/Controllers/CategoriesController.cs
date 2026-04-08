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

        [HttpGet("lay/category")]
        public async Task<IActionResult> Get()
        {
            var data = await _categoryService.GetListCategoryForUI();
            return Ok(data);
        }

        [HttpPost("them/category")]
        public async Task<IActionResult> Create([FromBody] Category_DTO dto)
        {

            await _categoryService.AddCategory(dto);

            return Ok(new { message = "Thêm danh mục thành công!" });
        }

        [HttpPost("sua/category{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto dto)
        {

            await _categoryService.UpdateCategory(id, dto);

            return Ok(new { message = "Cập nhật danh mục thành công!" });
        }

        [HttpPost("delete/category/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteCategory(id);
            return Ok(new { message = "Đã xóa mềm thành công danh mục này!" });
        }
    }
}
