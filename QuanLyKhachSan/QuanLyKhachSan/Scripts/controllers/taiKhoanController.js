var taiKhoanConfig = {
    pageSize: 5,
    pageIndex: 1,
}

var taiKhoanController = {

    init: function () {
        taiKhoanController.registerEvent();
    },
    registerEvent: function () {

        taiKhoanController.loadData();

        $(document).on("click", ".btnStatus", function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            taiKhoanController.changeStatus(id);
        });

        $(document).on("click", ".btnDelete", function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            bootbox.confirm("Are you sure?", function (result) {
                if (result) {
                    taiKhoanController.deleteAccount(id);
                }
            });
        });

        $(document).on("click", ".btnEdit", function (e) {
            e.preventDefault();
            $('#modalAddUpdate').modal('show');
            var id = $(this).data('id');
            taiKhoanController.loadDetail(id);
        });

        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            taiKhoanController.loadDataForDropDownList();
            taiKhoanController.resetForm();
        });

        $('#frmSaveData').validate({
            rules: {
                txtHoTen: {
                    required: true,
                    minlength: 5
                },
                txtDiaChi: {
                    required: true,
                    minlength: 6
                },
                txtSoDienThoai: {
                    required: true,
                    number: true,
                    min: 9
                },
                txtEmail: {
                    required: true
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
                    minlength: "Tên phải lớn hơn hoặc bằng 5 ký tự"
                },
                txtDiaChi: {
                    required: "Địa chỉ không được để trống",                  
                    minlength: "Địa chỉ tối thiểu phải có 6 ký tự"
                },
                txtSoDienThoai: {
                    required: "Số điện thoại không được để trống",
                    number: "Ký tự không hợp lệ",
                    min: "Tối thiểu phải có 9 số"
                },
                txtTenTaiKhoan: {
                    required: "Tên tài khoản không được để trống",
                    minlength: "Tài khoản ít nhất có 6 ký tự"
                },
                txtMauKhau: {
                    required: "Mật khẩu không được để trống",
                    minlength: "Mật khẩu chứa ít nhất có 6 ký tự"
                },
                txtEmail: {
                    required: "Email không được để trống"
                }
            }
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSaveData').valid()) {
                taiKhoanController.saveData();
            }
        });

        $('#btnSearch').off('click').on('click', function () {
            taiKhoanController.loadData(true);
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtNameSearch').val('');
            $('#ddlStatusSearch').val('');
            taiKhoanController.loadData(true);
        });

        $('#txtNameSearch').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                taiKhoanController.loadData(true);
            }
        });

        $(document).on("keypress", ".txtPrice", function (e) {
            if (e.which == 13) {
                var idPrice = $(this).data('id');
                var valuePrice = $(this).val();
                taiKhoanController.updatePrice(idPrice, valuePrice);
            }

            //$('.txtQuantity').off('keypress').on('keypress', function (e) {               
            //    if (e.which == 13) {
            //        var idQuantity = $(this).data('id');
            //        var valueQuantity = $(this).val();
            //        taiKhoanController.updateQuantity(idQuantity, valueQuantity);
            //    }
            //});           
        });

        $(document).on("keypress", ".txtQuantity", function (e) {
            if (e.which == 13) {
                var idQuantity = $(this).data('id');
                var valueQuantity = $(this).val();
                taiKhoanController.updateQuantity(idQuantity, valueQuantity);
            }

            //$('.txtPrice').off('keypress').on('keypress', function (e) {

            //    if (e.which == 13) {
            //        var idPrice = $(this).data('id');
            //        var valuePrice = $(this).val();
            //        taiKhoanController.updatePrice(idPrice, valuePrice);
            //    }
            //});
        });
    },

    //Các hàm khởi tạo
    saveData: function () {
        var hoTen = $('#txtHoTen').val();
        var diaChi = $('#txtDiaChi').val();
        var soDienThoai = $('#txtSoDienThoai').val();
        var tenTaiKhoan = $("#txtTenTaiKhoan").val();
        var matKhau = $('#txtMatKhau').val();
        var chucVu = $('#ddlChucVu option:selected').text();  
        console.log(chucVu);
        var email = $('#txtEmail').val();
        var id = parseInt($('#hiddenID').val());

        var chucvu = {
            HoTen: hoTen,
            DiaChi: diaChi,
            SoDienThoai: soDienThoai,
            TenTaiKhoan: tenTaiKhoan,
            MatKhau: matKhau,
            ChucVu: chucVu,
            Email: email,
            ID: id
        }       

        $.ajax({
            type: 'POST',
            url: '/Admin/TaiKhoan/LuuTaiKhoan',
            data: chucvu,
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Lưu lại thành công.", function () {                      
                        $('#modalAddUpdate').modal('hide');
                        taiKhoanController.loadData(true);
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
            url: "/Admin/TaiKhoan/LoadDataAccount",
            type: "GET",
            data: {
                name: name,
                page: taiKhoanConfig.pageIndex,
                pageSize: taiKhoanConfig.pageSize
            },
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#account-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.ID,
                            Username: item.Username,
                            Name: item.Name,
                            Role: item.Role,
                            //DateOfBirth: item.DateOfBirth,
                            //DateString: item.DateString,
                            Phone: item.Phone,
                            Email: item.Email
                        });
                    });
                    $('#tblDataCustomer').html(html);
                    taiKhoanController.paging(response.total, function () {
                        taiKhoanController.loadData();
                    }, changePageSize);
                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (error) {
                //bootbox.alert("Đã có lỗi xảy ra. Vui lòng thử lại sau");
                bootbox.alert(error.responseText);
            }
        })
    },
    formatDate: function (date) {

        //var day = date.getDate();
        //var month = date.getMonth();
        //var year = date.getFullYear();

        let now = date.getDate();

        let day = ("0" + now.getDate()).slice(-2);
        let month = ("0" + (now.getMonth() + 1)).slice(-2);

        let today = (day) + "-" + (month) + "-" + now.getFullYear();

        return today;
    },
    resetForm: function () {
        $('#hiddenID').val('0');
        $('#txtHoTen').val('');
        $('#txtDiaChi').val('');
        $('#txtSoDienThoai').val(0);
        $('#txtTenTaiKhoan').val('');
        $('#txtMatKhau').val('');
        $('#txtEmail').val('');
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
            url: '/Admin/TaiKhoan/LayChiTietTaiKhoan',
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
                    $('#txtTenTaiKhoan').val(data.TenTaiKhoan);
                    $("#txtMatKhau").val(data.MatKhau);
                    $("#ddlChucVu option:selected").text(data.ChucVu);
                    $('#txtEmail').val(data.Email);
                    taiKhoanController.loadDataForDropDownList();
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
    deleteAccount: function (id) {
        $.ajax({
            url: '/Admin/TaiKhoan/DeleteAccount',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Xóa thành công", function () {
                        taiKhoanController.loadData(true);
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
            url: '/Admin/taiKhoan/ChangeStatus',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    taiKhoanController.loadData();
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
        var totalPage = Math.ceil(totalRow / taiKhoanConfig.pageSize);

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
                taiKhoanConfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    },
    updatePrice: function (id, value) {
        $.ajax({
            url: '/Admin/taiKhoan/UpdatePrice',
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
            url: '/Admin/taiKhoan/UpdateQuantity',
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
    },
    loadDataForDropDownList: function () {
        $.ajax({
            url: '/Admin/TaiKhoan/LayChucVu',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var chucvu = response.chucvu;                 

                    $('#ddlChucVu').empty();

                    $.each(chucvu, function (i, value) {
                        $("#ddlChucVu").append($("<option></option>").val(value.ID).html(value.Name));
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
    }
}

taiKhoanController.init();