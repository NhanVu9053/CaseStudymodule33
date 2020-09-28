using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TiemSach.Models.UserModel
{
    public class User : IdentityUser
    {

        [Required(ErrorMessage = "Nhập vào tên của bạn!")]
        [StringLength(30, MinimumLength = 10)]
        public string Name { get; set; }

        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsDeleted { get; set; }
    }
}
