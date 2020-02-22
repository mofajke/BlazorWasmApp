using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BlazorWasmApp.AuthServer.Models;
using BlazorWasmApp.Infra.ConnectionFactory;
using BlazorWasmApp.Infra.DI;
using BlazorWasmApp.Infra.SqlLoader;
using Dapper;

namespace BlazorWasmApp.AuthServer.Dao
{
    [InjectAsSingleton(typeof(IUsersDao))]
    public class UsersDao : IUsersDao
    {
        private readonly IDbConnectionFactory connectionFactory;
        private readonly ISqlScriptLoader sqlScriptLoader;

        public UsersDao(IDbConnectionFactory connectionFactory, ISqlScriptLoader sqlScriptLoader)
        {
            this.connectionFactory = connectionFactory;
            this.sqlScriptLoader = sqlScriptLoader;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            using var dbConnection = await connectionFactory.CreateConnectionAsync();
            var result = await dbConnection.QueryAsync<User>(await sqlScriptLoader.GetSqlAsync("BlazorWasmApp.AuthServer.Dao.Scripts.GetUsers.sql"));
            return result;
        }

        public async Task<User> GetUserByLoginAndPasswordAsync(string login, string password)
        {
            using var dbConnection = await connectionFactory.CreateConnectionAsync();
            var result = await dbConnection.QueryFirstOrDefaultAsync<User>(await sqlScriptLoader.GetSqlAsync("BlazorWasmApp.AuthServer.Dao.Scripts.GetUser.sql"), new
            {
                login,
                password
            });
            return result;
        }
    }
}
