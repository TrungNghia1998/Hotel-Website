
var themMoiKhachHangController = {

    init: function () {
        themMoiKhachHangController.registerEvent();
    },
    registerEvent: function () {
                   
        $('#btnCreateAccount').off('click').on('click', function () {
            $('#modalAddCustomer').modal('show');
            themMoiKhachHangController.resetForm();
        });

        $('#frmSaveData').validate({
            rules: {
                txtTenTaiKhoan: {
                    required: true,
                    minlength: 5
                },
                txtMatKhau: {
                    required: true,
                    minlength: 5
                },
                txtHoTen: {
                    required: true,
                    minlength: 9
                },
                txtSoCMND: {
                    required: true,
                    number: true,
                    min: 9
                },
                txtSoDienThoai: {
                    required: true,
                    number: true,
                    min: 10
                },
                txtEmail: {
                    required: true,
                    minlength: 6
                }
            },
            messages: {
                txtTenTaiKhoan: {
                    required: "Tên tài khoản không được để trống",
                    minlength: "Tên tài khoản phải chứa ít nhất 5 ký tự"
                },
                txtMatKhau: {
                    required: "Mật khẩu không được để trống",
                    minlength: "Mật khẩu phải chứa ít nhất 5 ký tự",                   
                },
                txtHoTen: {
                    required: "Họ tên khách hàng không được để trống",
                    minlength: "Họ tên phải chứa ít nhất 9 ký tự"
                },
                txtSoCMND: {
                    required: "Số CMND không được để trống",
                    number: "Không thể nhập ký tự là chữ",
                    min: "Số CMND phải chứa ít nhất 9 ký tự"
                },
                txtSoDienThoai: {
                    required: "Số điện thoại không được để trống",
                    number: "Không thể nhập ký tự là chữ",
                    min: "Số điện thoại phải chứa ít nhất 10 số"
                },
                txtEmail: {
                    required: "Email khách hàng không được để trống",
                    min: "Email phải chứa ít nhất 6 ký tự"
                }
            }
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSaveData').valid()) {
                themMoiKhachHangController.saveData();
            }
        });
                 
    },

    //Các hàm khởi tạo
    saveData: function () {
        var TenTaiKhoan = $('#txtTenTaiKhoan').val();
        var MatKhau = $('#txtMatKhau').val();
        var HoTen = $('#txtHoTen').val();
        var SoCMND = $('#txtSoCMND').val();
        var SoDienThoai = $('#txtSoDienThoai').val();
        var Email = $('#txtEmail').val();

        var customer = {
            TenTaiKhoan: TenTaiKhoan,
            MatKhau: MatKhau,
            HoTen: HoTen,
            SoCMND: SoCMND,
            SoDienThoai: SoDienThoai,
            Email: Email
        }

        $.ajax({
            type: 'POST',
            url: '/Admin/KhachHang/SaveData',
            data: customer,
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Thêm mới thành công.", function () {
                        $('#modalAddCustomer').modal('hide');
                        var url = "/Admin/DonDatPhong/DatOffline/";
                        window.location.href = url;
                    });
                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (err) {
                console.log(err.responseText);
            }
        }); 
    },   
    resetForm: function () {
        $('#txtTenTaiKhoan').val('');
        $('#txtMatKhau').val('');
        $('#txtHoTen').val('');
        $('#txtSoCMND').val('');
        $('#txtSoDienThoai').val('');
        $('#txtEmail').val('');
    }    
}

themMoiKhachHangController.init();