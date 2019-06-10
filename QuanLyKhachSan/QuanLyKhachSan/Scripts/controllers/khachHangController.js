var khachHangConfig = {
    pageSize: 5,
    pageIndex: 1,
}

var khachHangController = {

    init: function () {
        khachHangController.registerEvent();
    },
    registerEvent: function () {

        khachHangController.loadData();

        $("#fileImage").change(function () {
            var File = this.files;
            if (File && File[0]) {
                khachHangController.ReadImage(File[0]);
            }
        });

        $(document).on("click", ".btnStatus", function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            khachHangController.changeStatus(id);
        });

        $(document).on("click", ".btnDelete", function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            bootbox.confirm("Are you sure?", function (result) {
                if (result) {
                    khachHangController.deleteKhachHang(id);
                }
            });
        });

        $(document).on("click", ".btnEdit", function (e) {
            e.preventDefault();
            $('#modalAddUpdate').modal('show');
            var id = $(this).data('id');
            khachHangController.loadDetail(id);
        });

        $('#clearPreview').on('click', function () {
            khachHangController.ClearPreview();
        })

        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            khachHangController.resetForm();
        });

        $('#frmSaveData').validate({
            rules: {
                txtHoTen: {
                    required: true,
                    minlength: 6
                },
                txtDiaChi: {
                    required: true,
                    minlength: 6
                },
                txtSoDienThoai: {
                    required: true,
                    number: true,
                    min: 10
                },
                txtSoCMND: {
                    required: true,
                    number: true,
                    min: 10
                },
                txtEmail: {
                    required: true,
                    minlength: 6
                },
                txtTenTaiKhoan: {
                    required: true,
                    minlength: 6
                },
                txtMatKhau: {
                    required: true,
                    minlength: 6
                }
            },
            messages: {
                txtHoTen: {
                    required: "Họ tên không được để trống",
                    minlength: "Họ tên tối thiểu phải có 6 ký tự"
                },
                txtDiaChi: {
                    required: "Địa chỉ không được để trống",
                    minlength: "Họ tên tối tiểu phải có 6 ký tự"
                },
                txtSoDienThoai: {
                    required: "Số điện thoại không được để trống",
                    number: "Ký tự không hợp lệ",
                    min: "Tối thiểu phải có 10 số"
                },
                txtSoCMND: {
                    required: "Số CMND KHÔNG được để trống",
                    number: "Ký tự không hợp lệ",
                    min: "Tối thiểu phải có 10 số"
                },
                txtEmail: {
                    required: "Email không được để trống",
                    minlength: "Tối thiểu có 6 ký tự"
                },
                txtTenTaiKhoan: {
                    required: "Tên tài khoản không được để trống",
                    minlength: "Tối thiểu phải có 6 ký tự"
                },
                txtMatKhau: {
                    required: "Mật khẩu không được để trống",
                    minlength: "Tối thiểu phải có 6 ký tự"
                }
            }
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSaveData').valid()) {
                khachHangController.saveData();
            }
        });

        $('#btnSearch').off('click').on('click', function () {
            khachHangController.loadData(true);
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtNameSearch').val('');
            $('#ddlStatusSearch').val('');
            khachHangController.loadData(true);
        });

        $('#txtNameSearch').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                khachHangController.loadData(true);
            }
        });

        $(document).on("keypress", ".txtPrice", function (e) {
            if (e.which == 13) {
                var idPrice = $(this).data('id');
                var valuePrice = $(this).val();
                khachHangController.updatePrice(idPrice, valuePrice);
            }

            //$('.txtQuantity').off('keypress').on('keypress', function (e) {               
            //    if (e.which == 13) {
            //        var idQuantity = $(this).data('id');
            //        var valueQuantity = $(this).val();
            //        khachHangController.updateQuantity(idQuantity, valueQuantity);
            //    }
            //});           
        });

        $(document).on("keypress", ".txtQuantity", function (e) {
            if (e.which == 13) {
                var idQuantity = $(this).data('id');
                var valueQuantity = $(this).val();
                khachHangController.updateQuantity(idQuantity, valueQuantity);
            }

            //$('.txtPrice').off('keypress').on('keypress', function (e) {

            //    if (e.which == 13) {
            //        var idPrice = $(this).data('id');
            //        var valuePrice = $(this).val();
            //        khachHangController.updatePrice(idPrice, valuePrice);
            //    }
            //});
        });
    },

    //Các hàm khởi tạo
    saveData: function () {
        var hoTen = $('#txtHoTen').val();
        var diaChi = $('#txtDiaChi').val();
        var soDienThoai = $('#txtSoDienThoai').val();
        var soCMND = $('#SoCMND').val();
        var tenTaiKhoan = $("#txtTenTaiKhoan").val();
        var matKhau = $('#txtMatKhau').val();
        var email = $('#txtEmail').val();
        var id = parseInt($('#hiddenID').val());

        var khachhang = {
            HoTen: hoTen,
            DiaChi: diaChi,
            SoDienThoai: soDienThoai,
            TenTaiKhoan: tenTaiKhoan,
            MatKhau: matKhau,
            SoCMND: soCMND,
            Email: email,
            ID: id
        }

        $.ajax({
            type: 'POST',
            url: '/Admin/KhachHang/LuuKhachHang',
            data: khachhang,
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Lưu lại thành công.", function () {
                        $('#modalAddUpdate').modal('hide');
                        khachHangController.loadData(true);
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
    loadData: function (changePageSize) {
        var name = $('#txtNameSearch').val();
        $.ajax({
            url: "/Admin/KhachHang/LoadDataCustomer",
            type: "GET",
            data: {
                name: name,
                page: khachHangConfig.pageIndex,
                pageSize: khachHangConfig.pageSize
            },
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#customer-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.ID,
                            Username: item.Username,
                            Name: item.Name,
                            Email: item.Email,
                            Address: item.Address,
                            Phone: item.Phone,
                            CMND: item.CMND
                        });
                    });
                    $('#tblDataCustomer').html(html);
                    khachHangController.paging(response.total, function () {
                        khachHangController.loadData();
                    }, changePageSize);
                }
                else {
                    bootbox.alert("Load dữ liệu thất bại. Vui lòng thử lại sau.")
                }
            },
            error: function (error) {
                //bootbox.alert("Đã có lỗi xảy ra. Vui lòng thử lại sau");
                bootbox.alert(error.responseText);
            }
        })
    },
    resetForm: function () {
        $('#hiddenID').val('0');
        $('#txtHoTen').val('');
        $('#txtDiaChi').val('');
        $('#txtSoDienThoai').val('');
        $('#txtSoCMND').val(0);
        $('#txtEmail').val('');
        $('#txtTenTaiKhoan').val('');
        $('#txtMauKhau').val('');
    },
    ReadImage: function (file) {
        var reader = new FileReader;
        var image = new Image;

        reader.readAsDataURL(file);
        reader.onload = function (_file) {

            image.src = _file.target.result;
            image.onload = function () {

                var height = this.height;
                var width = this.width;
                var type = file.type;
                var size = ~~(file.size / 1024) + "KB";

                $("#targetImage").attr('src', _file.target.result);
                $("#description").text("Size: " + size + ", " + height + "X " + width + ", " + type + "");
                $("#imgPreview").show();

            }
        }
    },
    ClearPreview: function () {
        $("#fileImage").val('');
        $("#description").text('');
        $("#imgPreview").hide();
    },
    loadDetail: function (id) {
        $.ajax({
            url: '/Admin/KhachHang/LayChiTietKhachHang',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    $('#hiddenID').val(data.ID);
                    $('#txtHoTen').val(data.HoTen);
                    $('#txtDiaChi').val(data.DiaChi);
                    $('#txtSoDienThoai').val(data.SoDienThoai);
                    $('#txtSoCMND').val(data.SoCMND);                 
                    $("#txtEmail").val(data.Email);
                    $("#txtTenTaiKhoan").val(data.TenTaiKhoan);
                    $('#txtMatKhau').val(data.MatKhau);    
                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (error) {
                bootbox.alert("Đã có lỗi xảy ra. Vui lòng thử lại sau");
                //bootbox.alert(error.responseText);
            }
        });
    },
    deleteKhachHang: function (id) {
        $.ajax({
            url: '/Admin/KhachHang/DeleteKhachHang',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Xóa thành công", function () {
                        khachHangController.loadData(true);
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
    changeStatus: function (id) {
        $.ajax({
            url: '/Admin/khachHang/ChangeStatus',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    khachHangController.loadData();
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
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / khachHangConfig.pageSize);

        //Unbind pagination if it existed or click change pagesize
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: "Đầu",
            next: "Sau",
            last: "Cuối",
            prev: "Trước",
            visiblePages: 10,
            onPageClick: function (event, page) {
                khachHangConfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    },
    updatePrice: function (id, value) {
        $.ajax({
            url: '/Admin/khachHang/UpdatePrice',
            type: 'POST',
            dataType: 'json',
            data: {
                ID: id,
                Price: value
            },
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Cập nhật thành công");
                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (err) {
                bootbox.alert(err.responseText);
            }
        });
    },
    updateQuantity: function (id, value) {
        $.ajax({
            url: '/Admin/khachHang/UpdateQuantity',
            type: 'POST',
            dataType: 'json',
            data: {
                ID: id,
                Quantity: value
            },
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Cập nhật thành công");
                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (err) {
                bootbox.alert(err.responseText);
            }
        });
    }
}

khachHangController.init();