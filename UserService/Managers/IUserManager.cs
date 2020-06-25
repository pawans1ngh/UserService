using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Managers
{
    public interface IUserManager
    {
        Task<UserInfo> GetUser(string email, string password);
        Task<UserInfo> GetUserAsync(int id);
        Task<List<UserInfo>> GetUsersAsync();
        Task<int> SaveUserAsync(UserInfo userInfo);
        Task<int> DeleteUserAsync(UserInfo userInfo);
    }
}
