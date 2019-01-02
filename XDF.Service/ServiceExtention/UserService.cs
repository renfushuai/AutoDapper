using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XDF.Core.Attribute;
using XDF.Core.Dto;
using XDF.Data;

namespace XDF.Service
{
   public partial class UserService
    {
        public static List<UserDto> UserDtos = new List<UserDto>
        {
            new UserDto{ Id = Guid.NewGuid(), UserName = "Paul", Password = "Paul123", Roles = new List<string>{ "administrator", "api_access" }, Urls = new List<string>{ "/api/values/getadminvalue", "/api/values/getguestvalue" }},
            new UserDto{ Id = Guid.NewGuid(), UserName = "Young", Password = "Young123", Roles = new List<string>{ "api_access" }, Urls = new List<string>{ "/api/values/getguestvalue" }},
            new UserDto{ Id = Guid.NewGuid(), UserName = "Roy", Password = "Roy123", Roles = new List<string>{ "administrator" }, Urls = new List<string>{ "/api/values/getadminvalue" }},
        };
        public List<string> GetFunctionsByUserId(Guid id)
        {
            var user = UserDtos.SingleOrDefault(r => r.Id.Equals(id));
            return user?.Urls;
        }
        [Caching(AbsoluteExpiration = 10)]
        public UserDto GetUserByName(string name)
        {
            var user = UserDtos.SingleOrDefault(r => r.UserName.Equals(name));
            return user;
        }
    }
}
