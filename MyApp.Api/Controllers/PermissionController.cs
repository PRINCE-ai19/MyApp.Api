using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Model_DTO;
using MyApp.Application.Store_Interface;

namespace MyApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionStoreService _permissionStore;
        public PermissionController(IPermissionStoreService permissionStore)
        {
            _permissionStore = permissionStore;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _permissionStore.GetAllPermissionForUI();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _permissionStore.GetPermissionByIdForUI(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Permission_DTO permissionDto)
        {
            var result = await _permissionStore.AddPermissionAsync(permissionDto);
            return Ok(result);
        }

        [HttpPost("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Permission_DTO permissionDto)
        {
            var result = await _permissionStore.UpdatePermissionAsync(id, permissionDto);
            return Ok(result);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _permissionStore.DeletePermissionAsync(id);
            return Ok(result);
        }
    }

}
