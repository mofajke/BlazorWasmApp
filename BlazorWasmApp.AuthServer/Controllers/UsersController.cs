using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorWasmApp.AuthServer.Dao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWasmApp.AuthServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersDao _usersDao;

        public UsersController(IUsersDao usersDao)
        {
            _usersDao = usersDao;
        }

        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            var result = await _usersDao.GetUsersAsync();
            return new JsonResult(result);
        }
    }
}