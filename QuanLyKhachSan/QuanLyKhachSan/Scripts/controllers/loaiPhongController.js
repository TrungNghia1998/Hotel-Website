var loaiPhongConfig = {
    pageSize: 10,
    pageIndex: 1,
}

var loaiPhongController = {

    init: function () {
        loaiPhongController.registerEvent();
    },
    registerEvent: function () {

        loaiPhongController.loadData();

        $("#fileImage").change(function () {
            var File = this.files;
            if (File && File[0]) {
                loaiPhongController.ReadImage(File[0]);
            }
        });

        $(document).on("click", ".btnStatus", function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            loaiPhongController.changeStatus(id);
        });

        $(document).on("click", ".btnDelete", function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            bootbox.confirm("Are you sure?", function (result) {
                if (result) {
                    loaiPhongController.deleteTypeRoom(id);
                }
            });
        });

        $(document).on("click", ".btnEdit", function (e) {
            e.preventDefault();
            $('#modalAddUpdate').modal('show');
            var id = $(this).data('id');
            loaiPhongController.loadDetailTypeRoom(id);
        });

        $('#clearPreview').on('click', function () {
            loaiPhongController.ClearPreview();
        })

        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            loaiPhongController.resetForm();
        });

        $('#frmSaveData').validate({
            rules: {
                txtTypeRoom: {
                    required: true,
                    minlength: 5
                },
                txtPrice: {
                    required: true,
                    number: true,
                    min: 0
                },
                txtPercent: {
                    required: true,
                    number:true,
                    minlength: 2
                }
            },
            messages: {
                txtTypeRoom: {
                    required: "Tên loại không được bỏ trống",
                    minlength: "Tên loại phải lớn hơn hoặc bằng 5 ký tự"
                },
                txtPrice: {
                    required: "Giá mặc định của phòng không được để trống",
                    number: "Giá không hợp lệ",
                    min: "Giá tiền phải lớn hơn hoặc bằng 0"
                },
                txtPercent: {
                    required: "Phần trăm phụ thu không được để trống",
                    number:"Phần trăm phụ thu không hợp lệ",
                    minlength: "Tỉ lệ phụ thu phải lớn hơn hoặc bằng 0 ký tự"
                }
            }
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSaveData').valid()) {
                loaiPhongController.saveData();
            }
        });

        $('#btnSearch').off('click').on('click', function () {
            loaiPhongController.loadData(true);
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtNameSearch').val('');
            $('#ddlStatusSearch').val('');
            loaiPhongController.loadData(true);
        });

        $('#txtNameSearch').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                loaiPhongController.loadData(true);
            }
        });

        $(document).on("keypress", ".txtPrice", function (e) {
            if (e.which == 13) {
                var idPrice = $(this).data('id');
                var valuePrice = $(this).val();
                loaiPhongController.updatePrice(idPrice, valuePrice);
            }     
        });

        $(document).on("keypress", ".txtPercent", function (e) {
            if (e.which == 13) {
                var idPercent = $(this).data('id');
                var valuePercent = $(this).val();
                loaiPhongController.updatePercent(idPercent, valuePercent);
            }
        });
    },

    //Các hàm khởi tạo
    saveData: function () {
        var typeRoom = $('#txtTypeRoom').val();      
        var price = parseFloat($('#txtPrice').val());      
        var percent = parseInt($('#txtPercent').val());       
        var file = $("#fileImage").get(0).files;
        var id = parseInt($('#hiddenID').val());
        
        var roomType = {
            TypeRoom: typeRoom,
            Price: price,
            Percent: percent,         
            ID: id
        }

        var data = new FormData;
        data.append("ImageFile", file[0]);

        //Update ảnh vào trong thư mục 
        $.ajax({
            type: 'POST',
            url: '/Admin/LoaiPhong/SaveDataImage',
            data: data,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.status == true) {
                    var fileName = '/Content/Images/Phong/' + response.fileName;
                    roomType["Image"] = fileName;
                    //Lưu dịch vụ vào database
                    $.ajax({
                        type: 'POST',
                        url: '/Admin/LoaiPhong/SaveDataTypeRoom',
                        data: roomType,
                        success: function (response) {
                            if (response.status == true) {
                                bootbox.alert("Lưu lại thành công.", function () {
                                    loaiPhongController.ClearPreview();
                                    $('#modalAddUpdate').modal('hide');
                                    loaiPhongController.loadData(true);
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
                        url: '/Admin/LoaiPhong/SaveDataTypeRoomNotImage',
                        data: roomType,
                        success: function (response) {
                            if (response.status == true) {
                                bootbox.alert("Lưu lại thành công.", function () {
                                    loaiPhongController.ClearPreview();
                                    $('#modalAddUpdate').modal('hide');
                                    loaiPhongController.loadData(true);
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
        //var status = $('#ddlStatusSearch').val();
        $.ajax({
            url: "/Admin/LoaiPhong/LoadDataTypeRoom",
            type: "GET",
            data: {
                name: name,
                page: loaiPhongConfig.pageIndex,
                pageSize: loaiPhongConfig.pageSize
            },
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#typeRoom-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            Image: item.Image,
                            Type: item.Type,
                            Price: item.Price,
                            PhuThu: item.PhuThu,                           
                            ID: item.ID
                        });
                    });
                    $('#tblDataTypeRoom').html(html);
                    loaiPhongController.paging(response.total, function () {
                        loaiPhongController.loadData();
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
    loadDetailTypeRoom: function (id) {
        $.ajax({
            url: '/Admin/LoaiPhong/GetDetailTypeRoom',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    $('#hiddenID').val(data.ID);
                    $('#txtTypeRoom').val(data.TypeRoom);
                    $('#txtPrice').val(data.Price);
                    $('#txtPercent').val(data.Percent);
                   
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
            url: '/Admin/LoaiPhong/DeleteTypeRoom',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Xóa thành công", function () {
                        loaiPhongController.loadData(true);
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
            url: '/Admin/loaiPhong/ChangeStatus',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    loaiPhongController.loadData();
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
        var totalPage = Math.ceil(totalRow / loaiPhongConfig.pageSize);

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
                loaiPhongConfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    },
    updatePrice: function (id, value) {
        $.ajax({
            url: '/Admin/LoaiPhong/UpdatePrice',
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
    updatePercent: function (id, value) {
        $.ajax({
            url: '/Admin/LoaiPhong/UpdatePercent',
            type: 'POST',
            dataType: 'json',
            data: {
                ID: id,
                Percent: value
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

loaiPhongController.init();