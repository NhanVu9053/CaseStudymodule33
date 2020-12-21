var carts = carts || {};
carts.delete = function (id) {
    console.log('hole');
    bootbox.confirm({
        title: "Cảnh báo",
        message: "Bạn có muốn xóa sản phẩm này khỏi giỏ hàng không?",
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Không'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Có'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    url: `/Cart/DeleteCart/${id}`,
                    method: "GET",
                    contentType: 'json',
                    success: function (data) {
                        if (data) {
                            window.location.href = "/Cart/ListCart";
                        }
                    }
                });
            }
        }
    });
}