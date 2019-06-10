var detailController = {
    init: function () {
        detailController.registerEvent();
    },
    registerEvent: function () {
      
        $('#btnAccept').off('click').on('click', function () {
            detailController.xacNhanDonDatPhong();
        });

        $('#btnBack').off('click').on('click', function () {
            var url = "/Admin/DonDatPhong/";
            window.location.href = url;
        });
    },
    xacNhanDonDatPhong: function () {
        var idPhieuDatPhong = $('#btnAccept').attr('idPhieuDatPhong');
        var idPhong = $('#btnAccept').attr('idPhong');
       
        $.ajax({
            url: "/Admin/DonDatPhong/XacNhanDonDatPHong",
            type: "GET",
            data: {              
                idPhieuDatPhong: idPhieuDatPhong,
                idPhong: idPhong
            },
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    bootbox.alert("Xác nhận thành công", function () {
                        var url = "/Admin/DonDatPhong/";
                        window.location.href = url;
                    });
                }
                else {
                    bootbox.alert(response.message)
                }
            },
            error: function (error) {
                //bootbox.alert("Đã có lỗi xảy ra. Vui lòng thử lại sau");
                bootbox.alert(error.responseText);
            }
        })
    }
}

detailController.init();