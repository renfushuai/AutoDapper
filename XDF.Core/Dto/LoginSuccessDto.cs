using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XDF.Core.Dto
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "{0} is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(128, MinimumLength = 6, ErrorMessage = "minimum Length of {0} is 6.")]
        public string Password { get; set; }
    }
}
