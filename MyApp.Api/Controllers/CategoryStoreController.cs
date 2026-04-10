using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Model_DTO;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces_store;

namespace MyApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryStoreController : ControllerBase
    {
        private readonly ICategoryRepository_store _repo;
        private readonly IMapper _mapper;

        public CategoryStoreController(ICategoryRepository_store repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("/Rotev2/Get/Category")]
        public async Task<IActionResult> Get()
        {
            var data = await _repo.GetAllCategoriesAsync();
            return Ok(data);
        }

        [HttpGet("/Rotev2/Get/Category/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await _repo.GetByIdAsync(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPost("/Rotev2/Update/Category/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category_DTO categoryDto)
        {
            var existingCategory = await _repo.GetByIdAsync(id);
            if (existingCategory == null)
                return NotFound(new { message = "Không tìm thấy danh mục có ID = " + id });

            _mapper.Map(categoryDto, existingCategory);

            var result = await _repo.UpdateAsync(existingCategory);

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
            var newCategory = _mapper.Map<Category>(categoryDto);
            var result = await _repo.AddAsync(newCategory);

            if (result.Success)
            {
                return Ok(new { message = result.Message, id = result.NewId });
            }
            else
            {
                return BadRequest(new { message = result.Message });
            }
        }

        [HttpPost("/Rotev2/Delete/Category/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repo.DeleteAsync(id);

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
