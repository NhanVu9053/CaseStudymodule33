using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TiemSach.ViewModel.OrderViewModel
{
    public class CustomerViewModel
    {
        [Required(ErrorMessage = "Nhập vào tên người nhận hàng!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Nhập vào số điện thoại người nhận hàng!")]
        [RegularExpression(@"^\(?(0|[3|5|7|8|9])+([0-9]{8})$", ErrorMessage = "Số điện thoại không hợp lệ!")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Nhập vào địa chỉ giao hàng!")]
        public string Address { get; set; }
        public int Quantity { get; set; }

        public string ProductId { get; set; }
    }
}
