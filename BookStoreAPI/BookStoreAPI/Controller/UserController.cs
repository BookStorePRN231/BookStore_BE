using AutoMapper;
using BookStoreAPI.Core.DTO;
using BookStoreAPI.Core.Interface;
using BookStoreAPI.Core.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service.IService;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreAPI.Controller
{
    [Route("api/users")]
    [ApiController]
   // [Authorize]
    public class UserController : ControllerBase
    {
        IUserService _user;
        IMapper _mapper;
        public UserController(IUserService user,IMapper mapper) 
        {
            _user = user;
            _mapper = mapper;
        }
       /// <summary>
       /// Search user by userName or get all users
       /// </summary>
       /// <param name="userName"></param>
       /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUser(string? userName)
        {
            IEnumerable<User> response=null;
            if (string.IsNullOrEmpty(userName))
            {
                response = await _user.GetAllUser();
            }
            else
            {
                response = await _user.GetUserByName(userName);
            }

            if (response != null)
            {
                var user = _mapper.Map<IEnumerable<UserDTO>>(response);
                return Ok(user);
            }
            return BadRequest("user don't exist in the system");
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var respone = await _user.GetUserById(userId);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest(userId + " don't exists");
        }
        /// <summary>
        /// Recover password by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("recover/{email}")]
        public async Task<IActionResult> RecoverPass(string email)
        {
            var respone = await _user.RecoverPassword(email);
            if (respone)
            {
                return Ok("recover password success");
            }
            return BadRequest("recover password fail");
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if (login != null)
            {
                var respone = await _user.CheckLogin(login);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, respone.User_Account),
                    new Claim(ClaimTypes.NameIdentifier, respone.User_Id.ToString()),
                    new Claim(ClaimTypes.Role, "admin")
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = false,
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);
                respone.User_Password = "";
                return StatusCode(200, respone);
            }
            return BadRequest("Accound or Pass Wrong!");
        }
        //[HttpPost("createUserMoble")]
        //public async Task<IActionResult> CreateUserMobel(CreateUserDTO userDTO)
        //{
        //    if(userDTO != null)
        //    {
        //        var user= _mapper.Map<User>(userDTO);
        //        var result= await _user.CreateUserMoble(user);
        //        if(result) return Ok("Create User Success");
        //    }
        //    return BadRequest("Create User Fail");
        //}

        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> CreateUserFE(UserDTO userDTO)
        {
            if (userDTO != null)
            {
                var user = _mapper.Map<User>(userDTO);
                var result = await _user.CreateUserFE(user);
                if (result) return Ok("Create User Success");
            }
            return BadRequest("Create User Fail");
        }
        [HttpPut()]
        public async Task<IActionResult> UpdateUser(UserDTO userDTO)
        {
            if (userDTO != null)
            {
                var user = _mapper.Map<User>(userDTO);
                var result = await _user.UpdateUser(user);
                if (result) return Ok("Update User Success");
            }
            return BadRequest("Update User Fail");
        }
        /// <summary>
        /// Update role user by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="option">1.Admin, 2.Staff, 3.User</param>
        /// <returns></returns>
        [HttpPatch("role/{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId,Enum.EnumClass.RoleOption option)
        {
            bool result = false;
            switch((int)option)
            {
                case 1:
                    result = await _user.UpdateRole(userId, (int)option);
                    break;
                case 2:
                    result = await _user.UpdateRole(userId, (int)option);
                    break;
                case 3:
                    result = await _user.UpdateRole(userId, (int)option);
                    break;
            }
            if (result) return Ok("Update Role Success");
            return BadRequest("Update Role Fail");
        }
        /// <summary>
        /// Delete or restore User by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="option">1.Delete, 2.Restore</param>
        /// <returns></returns>
        [HttpPatch("{userId}")]
        public async Task<IActionResult> DeleteBook(Guid userId, Enum.EnumClass.CommonStatusOption option)
        {
            bool result = false;
            switch ((int)option)
            {
                case 1:
                    result = await _user.DeleteUser(userId);
                    if (result) return Ok("Delete User Success");
                    break;
                case 2:
                    result = await _user.RestoreUser(userId);
                    if (result) return Ok("Restore User Success");
                    break;
            }
            return BadRequest("Delete/Restore User Failed");
        }
        [HttpDelete()]
        public async Task<IActionResult> RemoveUser(Guid userId)
        {
            var result = await _user.RemoveUser(userId);
            if (result) return Ok("Remove User Success");
            return BadRequest("Remove User Fail");
        }
    }
}
