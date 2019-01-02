using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XDF.Core.Dto;

namespace XDF.Web.Controllers
{
 
    /// <summary>
    /// 测试控制器
    /// </summary>
    //[Authorize(Policy = "Admin")]
    public class TestController : BaseController
    {

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpGet]
        public string Get(int p)
        {
            return p.ToString();
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPerson")]
        public UserDto Get(string p)
        {
            UserDto user=new UserDto();
            user.Id=Guid.NewGuid();
            return user;
        }
    }
}