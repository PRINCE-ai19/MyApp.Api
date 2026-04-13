using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Model_DTO;
using MyApp.Application.Services;
using MediatR;
using MyApp.Application.Features.Categories.Queries.GetListCategory;
using MyApp.Application.Features.Categories.Commands;
using Microsoft.VisualBasic;

namespace MyApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ISender _sender;

        public CategoriesController(ICategoryService categoryService, ISender sender)
        {
            _categoryService = categoryService;
            _sender = sender;
        }

        [HttpGet("lay/category")]
        public async Task<IActionResult> Get()
        {
            var data = await _sender.Send(new GetListCategoryQuery());
            return Ok(data);
        }

        [HttpPost("them/category")]
        public async Task<IActionResult> Create([FromBody] Category_DTO dto)
        {

            var result = await _sender.Send(new CreateCategoryCommand(dto));
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }

        [HttpPost("sua/category{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category_DTO dto)
        {

            var result = await _sender.Send(new UpdateProductCommand(id, dto));

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }

        [HttpPost("delete/category/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sender.Send(new DeleteCategoryCommad(id));
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }
    }
}
