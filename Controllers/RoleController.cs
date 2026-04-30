using PayRoleSystem.DTOs.Request;
using PayRoleSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PayRoleSystem.Http;
using PayRoleSystem.DTOs.Response;

namespace PayRoleSystem.Controllers
{ 
    public class RoleController : BaseApiController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        //[HttpPost("Create")]
        //public async Task<IActionResult> SaveOrUpdateRole([FromBody] RoleRequest roleRequest)
        //{
        //    if (roleRequest == null)
        //    {
        //        var response = new ResponseModel<string>
        //        {
        //            MessageType = 0, 
        //            Message = "Role data is required.",
        //            HttpStatusCode = 400, 
        //            Errors = new List<string> { "Role data cannot be null." }
        //        };
        //        return BadRequest(response);
        //    }

        //    var responseModel = await _roleService.SaveRoleAsync(roleRequest);

        //    if (responseModel.HttpStatusCode == 200) 
        //    {
        //        return Ok(responseModel);
        //    }

        //    return BadRequest(responseModel); 
        //}

        //[HttpGet("GetBy-Id")]

        //public async Task<IActionResult> GetRoleWisePermissions(int roleId)
        //{
        //    var response = await _roleService.GetByRoleIdPermissionsAsync(roleId);

        //    return StatusCode(response.HttpStatusCode, response);
        //}

        //[HttpGet("GetByAssigneeToken")]
        //public async Task<IActionResult> assigneePermissions()
        //{
        //    var response = await _roleService.GetRoleByTokenAsync();

        //    return StatusCode(response.HttpStatusCode, response);
        //}
        //// New Get All Roles Endpoint
        //[HttpGet("GetAll")]
        //public async Task<IActionResult> GetAllRoles()
        //{
        //    var response = await _roleService.GetAllRolesAsync();

        //    return StatusCode(response.HttpStatusCode, response);
        //}

        //[HttpGet("GetAllWithUsers")]
        //public async Task<IActionResult> GetAllRolesWithUsers()
        //{
        //    var response = await _roleService.GetAllRolesWithUsersAsync();

        //    return StatusCode(response.HttpStatusCode, response);
        //}
        [HttpPost("Create")]
        public async Task<IActionResult> SaveOrUpdateRole([FromBody] RoleRequest roleRequest)
        {
            if (roleRequest == null)
            {
                var response = new ResponseModel<string>
                {
                    MessageType = 0,
                    Message = "Role data is required.",
                    HttpStatusCode = 400,
                    Errors = new List<string> { "Role data cannot be null." }
                };
                return BadRequest(response);
            }

            var responseModel = await _roleService.SaveRoleAsync(roleRequest);

            if (responseModel.HttpStatusCode == 200)
            {
                return Ok(responseModel);
            }

            return BadRequest(responseModel);
        }

        [HttpGet("GetBy-Id")]
        public async Task<IActionResult> GetRoleWisePermissions(int roleId)
        {
            var response = await _roleService.GetByRoleIdPermissionsAsync(roleId);

            return StatusCode(response.HttpStatusCode, response);
        }

        [HttpGet("GetByAssigneeToken")]
        public async Task<IActionResult> assigneePermissions()
        {
            var response = await _roleService.GetRoleByTokenAsync();

            return StatusCode(response.HttpStatusCode, response);
        }
        // New Get All Roles Endpoint
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllRoles()
        {
            var response = await _roleService.GetAllRolesAsync();

            return StatusCode(response.HttpStatusCode, response);
        }

        [HttpGet("GetAllWithUsers")]
        public async Task<IActionResult> GetAllRolesWithUsers()
        {
            var response = await _roleService.GetAllRolesWithUsersAsync();

            return StatusCode(response.HttpStatusCode, response);
        }

    }
}
