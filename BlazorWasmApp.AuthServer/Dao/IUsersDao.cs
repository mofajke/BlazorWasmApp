using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorWasmApp.AuthServer.Models;

namespace BlazorWasmApp.AuthServer.Dao
{
    public interface IUsersDao
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserByLoginAndPasswordAsync(string login, string password);
    }
}
