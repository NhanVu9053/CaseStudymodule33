using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TiemSach.Models.UserModel;

namespace TiemSach.ViewModel.UserViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nhập vào email!")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nhập vào mật khẩu!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp!")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Upload)]
    
        public IFormFile ImageFile { get; set; }

        public string ProfilePicture { get; set; }

        [Required(ErrorMessage = "Nhập vào tên!")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Độ dài của tên trong khoảng từ 5 đến 30 ký tự!")]
        public string Name { get; set; }

        public string Address { get; set; }

        [RegularExpression(@"^\(?(0|[3|5|7|8|9])+([0-9]{8})$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }

        public Gender Gender { get; set; }
        public string RoleId { get; set; }
        public string ProductId { get; set; }
    }
}
