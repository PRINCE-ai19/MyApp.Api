using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Model_DTO;
using MyApp.Application.Store_Interface;
using MyApp.Domain.Interfaces_store;

namespace MyApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserStoreService _userStore;
        public UserController(IUserStoreService userStore)
        {
            _userStore = userStore;
        }


        [HttpGet("/Get/AllUser")]
        public async Task<IActionResult> Get()
        {
            var data = await _userStore.GetListUserForUI();
            return Ok(data);
        }

        [HttpGet("/Get/UserById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _userStore.GetUserByIdForUI(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost("/Post/AddUser")]
        public async Task<IActionResult> Add([FromBody] UserADD_DTO userDto)
        {
            var response = await _userStore.AddUserAsync(userDto);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("/User/update/{id}")]
        public async Task<IActionResult> Update(int id ,[FromBody] User_DTO userDto)
        {
            var response = await _userStore.UpdateUserAsync(id , userDto);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("/User/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userStore.DeleteUserAsync(id);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("roles/add")]
        public async Task<IActionResult> AddRole(int userId, int roleId)
        {
            var response = await _userStore.AddRoleToUserAsync(userId, roleId);
            return Ok(response);
        }

        [HttpPost("roles/delete")]
        public async Task<IActionResult> DeleteRole(int userId, int roleId)
        {
            var response = await _userStore.DeleteRoleFromUserAsync(userId, roleId);
            return Ok(response);
        }

        [HttpGet("roles/{userId}")]
        public async Task<IActionResult> GetRoles(int userId)
        {
            var data = await _userStore.GetUserRolesDetailForUI(userId);
            return Ok(data);
        }
    }
}
