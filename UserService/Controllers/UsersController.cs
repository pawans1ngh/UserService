using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Managers;
using UserService.Models;

namespace UserService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly IUserManager _userManager;

        public UsersController(
            UserDbContext context, 
            IUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetUserInfo()
        {
            return await _userManager.GetUsersAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfo>> GetUserInfo(int id)
        {
            var userInfo = await _userManager.GetUserAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return userInfo;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserInfo(int id, UserInfo userInfo)
        {
            if (id != userInfo.UserId)
            {
                return BadRequest();
            }

            var userInfoOld = await _userManager.GetUserAsync(id);
            if (userInfoOld == null)
            {
                return NotFound();
            }

            userInfoOld.FirstName = userInfo.FirstName ?? userInfoOld.FirstName; 
            userInfoOld.LastName = userInfo.LastName ?? userInfoOld.LastName;
            userInfoOld.UserName = userInfo.UserName ?? userInfoOld.UserName;
            userInfoOld.Password = userInfo.Password ?? userInfoOld.Password;

            try
            {
                await _userManager.SaveUserAsync(userInfoOld);
            }
            catch (ApplicationException ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserInfo>> PostUserInfo(UserInfo userInfo)
        {
            if (string.IsNullOrEmpty(userInfo.FirstName)
                || string.IsNullOrEmpty(userInfo.LastName)
                || string.IsNullOrEmpty(userInfo.Email)
                || string.IsNullOrEmpty(userInfo.UserName)
                || string.IsNullOrEmpty(userInfo.Password))
            {
                return BadRequest("One or more required fields are missing.");
            }

            await _userManager.SaveUserAsync(userInfo);

            return CreatedAtAction("GetUserInfo", new { id = userInfo.UserId }, userInfo);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserInfo>> DeleteUserInfo(int id)
        {
            var userInfo = await _userManager.GetUserAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }

            await _userManager.DeleteUserAsync(userInfo);    
            return userInfo;
        }

    }
}
