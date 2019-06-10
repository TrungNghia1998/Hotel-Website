var dichVuConfig = {
    pageSize: 10,
    pageIndex: 1,
}

var dichVuController = {

    init: function () {
        dichVuController.registerEvent();
    },
    registerEvent: function () {

        dichVuController.loadData();

        $("#fileImage").change(function () {
            var File = this.files;
            if (File && File[0]) {
                dichVuController.ReadImage(File[0]);
            }
        });    
        
        $(document).on("click", ".btnStatus", function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            dichVuController.changeStatus(id);
        });

        $(document).on("click", ".btnDelete", function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            bootbox.confirm("Are you sure?", function (result) {
                if (result) {
                    dichVuController.deleteTypeRoom(id);
                }
            });
        });
     
        $(document).on("click", ".btnEdit", function (e) {
            e.preventDefault();
            $('#modalAddUpdate').modal('show');
            var id = $(this).data('id');
            dichVuController.loadDetailService(id);          
        });

        $('#clearPreview').on('click', function () {
            dichVuController.ClearPreview();
        })

        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            dichVuController.resetForm();
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
                dichVuController.saveData();
            }
        });

        $('#btnSearch').off('click').on('click', function () {           
            dichVuController.loadData(true);
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtNameSearch').val('');
            $('#ddlStatusSearch').val('');
            dichVuController.loadData(true);
        });

        $('#txtNameSearch').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                dichVuController.loadData(true);
            }
        });  

        $(document).on("keypress", ".txtPrice", function (e) {         
            if (e.which == 13) {
                var idPrice = $(this).data('id');
                var valuePrice = $(this).val();
                dichVuController.updatePrice(idPrice, valuePrice);
            }

            //$('.txtQuantity').off('keypress').on('keypress', function (e) {               
            //    if (e.which == 13) {
            //        var idQuantity = $(this).data('id');
            //        var valueQuantity = $(this).val();
            //        dichVuController.updateQuantity(idQuantity, valueQuantity);
            //    }
            //});           
        });

        $(document).on("keypress", ".txtQuantity", function (e) {
            if (e.which == 13) {
                var idQuantity = $(this).data('id');
                var valueQuantity = $(this).val();
                dichVuController.updateQuantity(idQuantity, valueQuantity);
            }

            //$('.txtPrice').off('keypress').on('keypress', function (e) {

            //    if (e.which == 13) {
            //        var idPrice = $(this).data('id');
            //        var valuePrice = $(this).val();
            //        dichVuController.updatePrice(idPrice, valuePrice);
            //    }
            //});
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
            url: '/Admin/DichVu/SaveDataImage',
            data: data,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.status == true) {
                    var fileName = '/Content/Images/DichVu/' + response.fileName;
                    service["Image"] = fileName;
                    //Lưu dịch vụ vào database
                    $.ajax({
                        type: 'POST',
                        url: '/Admin/DichVu/SaveDataService',
                        data: service,                      
                        success: function (response) {
                            if (response.status == true) {
                                bootbox.alert("Lưu lại thành công.", function () {
                                    dichVuController.ClearPreview();
                                    $('#modalAddUpdate').modal('hide');
                                    dichVuController.loadData(true);
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
                        url: '/Admin/DichVu/SaveDataServiceNotImage',
                        data: service,
                        success: function (response) {
                            if (response.status == true) {
                                bootbox.alert("Lưu lại thành công.", function () {
                                    dichVuController.ClearPreview();
                                    $('#modalAddUpdate').modal('hide');
                                    dichVuController.loadData(true);
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
        var status = $('#ddlStatusSearch').val();
        $.ajax({
            url: "/Admin/DichVu/LoadDataService",
            type: "GET",
            data: {
                name: name,
                status: status,
                page: dichVuConfig.pageIndex,
                pageSize: dichVuConfig.pageSize
            },
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#service-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            Image: item.Image,
                            Name: item.Name,
                            Price: item.Price,
                            Quantity: item.Quantity,
                            UnitPrice: item.UnitPrice,
                            Status: item.Status == true ? "<span class=\"badge badge-success\">Được sử dụng</span>" : "<span class=\"badge badge-danger\">Ngưng sử dụng</span>",
                            ID: item.ID
                        });
                    });
                    $('#tblDataService').html(html);
                    dichVuController.paging(response.total, function () {
                        dichVuController.loadData();
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
            url: '/Admin/DichVu/GetDetail',
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
            url: '/Admin/DichVu/DeleteTypeRoom',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Xóa thành công", function () {
                        dichVuController.loadData(true);
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
            url: '/Admin/DichVu/ChangeStatus',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    dichVuController.loadData();
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
        var totalPage = Math.ceil(totalRow / dichVuConfig.pageSize);

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
                dichVuConfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    },
    updatePrice: function (id, value) {
        $.ajax({
            url: '/Admin/DichVu/UpdatePrice',
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
            url: '/Admin/DichVu/UpdateQuantity',
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

dichVuController.init();