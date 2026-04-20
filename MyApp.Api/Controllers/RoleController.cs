using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Model_DTO;
using MyApp.Application.Store_Interface;

namespace MyApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleSoteService _roleStore;
        public RoleController(IRoleSoteService roleStore)
        {
           _roleStore = roleStore;
        }

        [HttpGet("/Get/AllRole")]
        public async Task<IActionResult> Get()
        {
            var data = await _roleStore.GetListRoleForUI();
            return Ok(data);
        }

        [HttpGet("/Get/RoleById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _roleStore.GetRoleByIdForUI(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost("/Post/AddRole")]
        public async Task<IActionResult> Add([FromBody] Role_DTO roleDto)
        {
            var response = await _roleStore.AddRoleAsync(roleDto);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("/Put/UpdateRole/{id}")]
        public async Task<IActionResult> Update( int id ,[FromBody] Role_DTO roleDto)
        {
            var response = await _roleStore.UpdateRoleAsync(id, roleDto);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("/Delete/DeleteRole/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _roleStore.DeleteRoleAsync(id);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("update-users/{roleId}")]
        public async Task<IActionResult> UpdateUsersInRole(int roleId, [FromBody] IEnumerable<int> userIds)
        {
            var response = await _roleStore.UpdateUsersInRoleAsync(roleId, userIds);
            return Ok(response);
        }
    }
}
