using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XDF.Web.AuthHelper.OverWrite;

namespace XDF.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sub"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Login")]
        public JsonResult Login(long id = 1, string sub = "Admin")
        {           
            TokenModelJWT tokenModel = new TokenModelJWT();
            tokenModel.Uid = id;
            tokenModel.Role = sub;
            string jwtStr = JwtHelper.IssueJWT(tokenModel);
            return Json(jwtStr);
        }

    }
}