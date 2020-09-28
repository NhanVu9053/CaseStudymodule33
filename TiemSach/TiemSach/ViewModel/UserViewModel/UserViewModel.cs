using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TiemSach.Models.UserModel;

namespace TiemSach.ViewModel.UserViewModel
{
    public class UserViewModel
    {
        public string UserId { get; set; }

        [DataType(DataType.Upload)]
       
        public IFormFile ImageFile { get; set; }

        public string ProfilePicture { get; set; }

        [Required(ErrorMessage = "Nhập vào email của bạn!")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nhập vào tên của bạn!")]
        [StringLength(30, MinimumLength = 10)]
        public string Name { get; set; }

        public string Address { get; set; }

        [RegularExpression(@"^\(?(0|[3|5|7|8|9])+([0-9]{8})$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }

        public Gender Gender { get; set; }
        public string RoleName { get; set; }
    }
}
