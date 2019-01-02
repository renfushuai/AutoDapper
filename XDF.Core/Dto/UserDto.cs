using System;
using System.Collections.Generic;

namespace XDF.Core.Dto
{
    public class UserDto
    {
        /// <summary>
        /// guid
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Urls { get; set; }
    }
}