using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service.IService;
using static System.Net.Mime.MediaTypeNames;

namespace BookStoreAPI.Controller
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        IRoleService _role;
        public RoleController(IRoleService role)
        {
            _role = role;
        }
        [HttpGet()]
        public async Task<IActionResult> GetRole()
        {
            var respone = await _role.GetAllRole();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest("null");
        }
    }

}
