using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Model_DTO;
using MyApp.Application.Store_Services;

namespace MyApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryStoreController : ControllerBase
    {
        private readonly ICategoryStoreService _categoryService;

        public CategoryStoreController(ICategoryStoreService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("/Rotev2/Get/Category")]
        public async Task<IActionResult> Get()
        {
            var data = await _categoryService.GetAllCategories();
            return Ok(data);
        }

        [HttpGet("/Rotev2/Get/Category/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await _categoryService.GetById(id);
                return Ok(data);
            }
            catch (System.Exception ex)
            {
                
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("/Rotev2/Update/Category/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category_DTO categoryDto)
        {
            var result = await _categoryService.Update(id, categoryDto);

            if (result.Success)
            {
                return Ok(new { message = result.Message });
            }
            else
            {
               
                return BadRequest(new { message = result.Message });
            }
        }

        [HttpPost("/Rotev2/Create/Category")]
        public async Task<IActionResult> Create([FromBody] Category_DTO categoryDto)
        {
            var result = await _categoryService.Create(categoryDto);

            if (result.Success)
            {
                return Ok(new { message = result.Message });
            }
            else
            {
                return BadRequest(new { message = result.Message });
            }
        }

        [HttpPost("/Rotev2/Delete/Category/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.Delete(id);

            if (result.Success)
            {
                return Ok(new { message = result.Message });
            }
            else
            {
                return NotFound(new { message = result.Message });
            }
        }
    }
}
