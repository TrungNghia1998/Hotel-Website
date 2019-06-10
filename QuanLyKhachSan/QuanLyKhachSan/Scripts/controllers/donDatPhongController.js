var donDatPhongConfig = {
    pageSize: 10,
    pageIndex: 1,
}

var donDatPhongController = {

    init: function () {
        donDatPhongController.registerEvent();
    },
    registerEvent: function () {

        donDatPhongController.loadData();

        $("#fileImage").change(function () {
            var File = this.files;
            if (File && File[0]) {
                donDatPhongController.ReadImage(File[0]);
            }
        });

        $(document).on("click", ".btnStatus", function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            donDatPhongController.changeStatus(id);
        });

        $(document).on("click", ".btnDelete", function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            bootbox.confirm("Are you sure?", function (result) {
                if (result) {
                    donDatPhongController.deleteTypeRoom(id);
                }
            });
        });

        $(document).on("click", ".btnDetail", function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var url = "/Admin/DonDatPhong/LoadDetailDonDatPhong?id=" + id;
            window.location.href = url;
        });

        $('#clearPreview').on('click', function () {
            donDatPhongController.ClearPreview();
        })

        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            donDatPhongController.resetForm();
        });

        $('#frmSaveData').validate({
            rules: {
                txtNameService: {
                    required: true,
                    minlength: 5
                },
                txtPriceService: {
                    required: true,
                    number: true,
                    min: 0
                },
                txtTypeService: {
                    required: true,
                    minlength: 2
                },
                txtQuantityService: {
                    required: true,
                    number: true,
                    min: 0
                }
            },
            messages: {
                txtNameService: {
                    required: "Bạn phải nhập tên",
                    minlength: "Tên phải lớn hơn hoặc bằng 5 ký tự"
                },
                txtPriceService: {
                    required: "Bạn phải nhập lương",
                    number: "Lương không hợp lệ",
                    min: "Giá tiền phải lớn hơn hoặc bằng 0"
                },
                txtTypeService: {
                    required: "Đơn vị tính không được để trống",
                    minlength: "Độ dài phải lớn hơn hoặc bằng 2 ký tự"
                },
                txtQuantityService: {
                    required: "Số lượng tồn kho không được để trống",
                    number: "Số lượng tồn kho không hợp lệ",
                    min: "Số lượng tồn kho phải lớn hơn hoặc bằng 0"
                }
            }
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSaveData').valid()) {
                donDatPhongController.saveData();
            }
        });

        $('#btnSearch').off('click').on('click', function () {
            donDatPhongController.loadData(true);
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtNameSearch').val('');
            $('#ddlStatusSearch').val('');
            donDatPhongController.loadData(true);
        });

        $('#txtNameSearch').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                donDatPhongController.loadData(true);
            }
        });

        $(document).on("keypress", ".txtPrice", function (e) {
            if (e.which == 13) {
                var idPrice = $(this).data('id');
                var valuePrice = $(this).val();
                donDatPhongController.updatePrice(idPrice, valuePrice);
            }          
        });

        $(document).on("keypress", ".txtQuantity", function (e) {
            if (e.which == 13) {
                var idQuantity = $(this).data('id');
                var valueQuantity = $(this).val();
                donDatPhongController.updateQuantity(idQuantity, valueQuantity);
            }        
        });
    },

    //Các hàm khởi tạo
    saveData: function () {
        var name = $('#txtNameService').val();
        var price = parseFloat($('#txtPriceService').val());
        var type = $('#txtTypeService').val();
        var quantity = parseInt($('#txtQuantityService').val());
        var file = $("#fileImage").get(0).files;

        var id = parseInt($('#hiddenID').val());

        var service = {
            Name: name,
            Price: price,
            Quantity: quantity,
            Type: type,
            ID: id
        }

        var data = new FormData;
        data.append("ImageFile", file[0]);

        //Update ảnh vào trong thư mục 
        $.ajax({
            type: 'POST',
            url: '/Admin/donDatPhong/SaveDataImage',
            data: data,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.status == true) {
                    var fileName = '/Content/Images/donDatPhong/' + response.fileName;
                    service["Image"] = fileName;
                    //Lưu dịch vụ vào database
                    $.ajax({
                        type: 'POST',
                        url: '/Admin/donDatPhong/SaveDataService',
                        data: service,
                        success: function (response) {
                            if (response.status == true) {
                                bootbox.alert("Lưu lại thành công.", function () {
                                    donDatPhongController.ClearPreview();
                                    $('#modalAddUpdate').modal('hide');
                                    donDatPhongController.loadData(true);
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
                else {
                    $.ajax({
                        type: 'POST',
                        url: '/Admin/donDatPhong/SaveDataServiceNotImage',
                        data: service,
                        success: function (response) {
                            if (response.status == true) {
                                bootbox.alert("Lưu lại thành công.", function () {
                                    donDatPhongController.ClearPreview();
                                    $('#modalAddUpdate').modal('hide');
                                    donDatPhongController.loadData(true);
                                });
                            }
                            else {
                                bootbox.alert(response.message);
                            }
                        },
                        error: function (err) {
                            console.log(err.responseText);
                        }
                    })
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    loadData: function (changePageSize) {
        var name = $('#txtNameSearch').val();
        $.ajax({
            url: "/Admin/DonDatPhong/LoadData",
            type: "GET",
            data: {
                name: name,
                page: donDatPhongConfig.pageIndex,
                pageSize: donDatPhongConfig.pageSize
            },
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#dondatphong-template').html();

                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            KhachHang: item.TenKhachHang,
                            NgayDen: item.NgayDen,
                            NgayDi: item.NgayDi,
                            NgayDat: item.NgayDat,
                            TrangThai: item.TinhTrang,
                            ID: item.MaPhieuDatPhong
                        });
                    });
                    $('#tblDataDonDatPhong').html(html);
                    donDatPhongController.paging(response.total, function () {
                        donDatPhongController.loadData();
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
        $('#txtNameService').val('');
        $('#txtPriceService').val(0);
        $('#txtTypeService').val('');
        $('#txtQuantityService').val(0);
        $('#fileImage').val('');
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
    loadDetailService: function (id) {
        $.ajax({
            url: '/Admin/donDatPhong/GetDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    $('#hiddenID').val(data.ID);
                    $('#txtNameService').val(data.Name);
                    $('#txtPriceService').val(data.Price);
                    $('#txtTypeService').val(data.Type);
                    $('#txtQuantityService').val(data.Quantity);
                    //$('#fileImage').val(data.Image);
                    $("#targetImage").attr('src', data.Image);
                    $("#imgPreview").show();
                    $('#clearPreview').hide();
                    $('#description').hide();
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
    deleteTypeRoom: function (id) {
        $.ajax({
            url: '/Admin/donDatPhong/DeleteTypeRoom',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Xóa thành công", function () {
                        donDatPhongController.loadData(true);
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
            url: '/Admin/donDatPhong/ChangeStatus',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    donDatPhongController.loadData();
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
        var totalPage = Math.ceil(totalRow / donDatPhongConfig.pageSize);

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
                donDatPhongConfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    },
    updatePrice: function (id, value) {
        $.ajax({
            url: '/Admin/donDatPhong/UpdatePrice',
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
            url: '/Admin/donDatPhong/UpdateQuantity',
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

donDatPhongController.init();