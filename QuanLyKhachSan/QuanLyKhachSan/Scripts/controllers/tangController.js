var tangConfig = {
    pageSize: 10,
    pageIndex: 1,
}

var tangController = {

    init: function () {
        tangController.registerEvent();
    },
    registerEvent: function () {

        tangController.loadData();

        $(document).on("click", ".btnStatus", function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            tangController.changeStatus(id);
        });

        $(document).on("click", ".btnEdit", function (e) {
            e.preventDefault();
            $('#modalAddUpdate').modal('show');
            var id = $(this).data('id');
            tangController.loadDetailLevel(id);
        });

        $(document).on("click", ".btnDelete", function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            bootbox.confirm("Are you sure?", function (result) {
                if (result) {
                    tangController.deleteLevel(id);
                }
            });
        });

        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');           
            tangController.resetForm();
        });

        $('#frmSaveData').validate({
            rules: {
                txtLevelRoom: {
                    required: true,
                    minlength: 5
                }
            },
            messages: {
                txtNameRoom: {
                    required: "Tên tầng không được để trống.",
                    minlength: "Tên tầng tối thiểu phải có 5 ký tự"
                }
            }
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSaveData').valid()) {
                tangController.saveData();
            }
        });

        $('#btnSearch').off('click').on('click', function () {
            tangController.loadData(true);
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtNameSearch').val('');
            $('#ddlStatusSearch').val('');
            tangController.loadData(true);
        });

        $('#txtNameSearch').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                tangController.loadData(true);
            }
        });
    },

    //Các hàm khởi tạo
    saveData: function () {
        var name = $('#txtLevelRoom').val();      
        var id = parseInt($('#hiddenID').val());

        var room = {
            Name: name,
            ID: id
        }
        $.ajax({
            type: 'POST',
            url: '/Admin/Tang/SaveDataLevel',
            data: room,
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Lưu lại thành công.", function () {                      
                        $('#modalAddUpdate').modal('hide');
                        tangController.loadData(true);
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
            url: "/Admin/Tang/LoadDataLevel",
            type: "GET",
            data: {
                name: name,               
                page: tangConfig.pageIndex,
                pageSize: tangConfig.pageSize
            },
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#level-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {                        
                            Name: item.Name,                          
                            ID: item.ID
                        });
                    });
                    $('#tblLevelRoom').html(html);
                    tangController.paging(response.total, function () {
                        tangController.loadData();
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
        $('#txtLevelRoom').val('');
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
    loadDetailLevel: function (id) {
        $.ajax({
            url: '/Admin/Tang/GetDetailLevel',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    $('#hiddenID').val(data.ID);
                    $('#txtLevelRoom').val(data.Name);                                   
                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (error) {
                //bootbox.alert("Đã có lỗi xảy ra. Vui lòng thử lại sau");
                bootbox.alert(error.responseText);
            }
        });
    },
    deleteLevel: function (id) {
        $.ajax({
            url: '/Admin/Tang/DeleteLevel',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Xóa thành công", function () {
                        tangController.loadData(true);
                    });
                }
                else {
                    bootbox.alert("Không thể xóa tầng này (hiện đang tồn tại trong các hóa đơn thống kê)");
                }
            },
            error: function (err) {
                console.log(err.responseText);
            }
        });
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / tangConfig.pageSize);

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
                tangConfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    },
    loadDataForDropDownList: function () {
        $.ajax({
            url: '/Admin/tang/GetTypeAndLevelRoom',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var level = response.level;
                    var type = response.type;

                    $('#ddlLevelRoom').empty();

                    $.each(level, function (i, value) {
                        $("#ddlLevelRoom").append($("<option></option>").val(value.ID).html(value.Name));
                    });

                    //$("#ddlLevelRoom").find("option").eq(0).remove();

                    //

                    $('#ddlTypeRoom').empty();

                    $.each(type, function (i, value) {
                        $("#ddlTypeRoom").append($("<option></option>").val(value.ID).html(value.Name));
                    });

                    //$("#ddlTypeRoom").find("option").eq(0).remove();
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

tangController.init();