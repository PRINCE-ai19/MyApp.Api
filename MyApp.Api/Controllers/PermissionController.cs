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

        [HttpPost("role/add-permission/{roleId}/{permissionId}")]
        public async Task<IActionResult> AddPermissionToRole(int roleId, int permissionId)
        {
            var result = await _permissionStore.AddPermissionToRoleAsync(roleId, permissionId);
            return Ok(result);
        }

        [HttpPost("role/update-permissions/{roleId}")]
        public async Task<IActionResult> UpdateRolePermissions(int roleId, [FromBody] IEnumerable<int> permissionIds)
        {
            var result = await _permissionStore.UpdateRolePermissionsAsync(roleId, permissionIds);
            return Ok(result);
        }

        [HttpGet("role/{roleId}")]
        public async Task<IActionResult> GetPermissionsByRoleId(int roleId)
        {
            var data = await _permissionStore.GetPermissionsByRoleIdForUI(roleId);
            return Ok(data);
        }

        [HttpGet("roles-by-permission/{permissionId}")]
        public async Task<IActionResult> GetRolesByPermissionId(int permissionId)
        {
            var data = await _permissionStore.GetRolesByPermissionIdForUI(permissionId);
            return Ok(data);
        }
    }
}
