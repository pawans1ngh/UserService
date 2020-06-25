using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Managers
{
    public class UserManager : IUserManager
    {
        private UserDbContext _context;

        public UserManager(UserDbContext context)
        {
            _context = context;
        }

        public async Task<UserInfo> GetUser(string email, string password)
        {
            return await _context.UserInfo.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        public async Task<UserInfo> GetUserAsync(int id)
        {
            return await _context.UserInfo.FindAsync(id);
;       }

        public async Task<List<UserInfo>> GetUsersAsync()
        {
            return await _context.UserInfo.ToListAsync();
        }

        private bool UserInfoExists(int id)
        {
            return _context.UserInfo.Any(e => e.UserId == id);
        }

        public async Task<int> SaveUserAsync(UserInfo userInfo)
        {
            if (userInfo.UserId <= 0)
            {
                _context.UserInfo.Add(userInfo);
            }
            else 
            {
                _context.Entry(userInfo).State = EntityState.Modified;
            }

            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserInfoExists(userInfo.UserId))
                {
                    throw new ApplicationException("User not found.");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<int> DeleteUserAsync(UserInfo userInfo)
        {
            _context.UserInfo.Remove(userInfo);
            return await _context.SaveChangesAsync();
        }
    }
}
