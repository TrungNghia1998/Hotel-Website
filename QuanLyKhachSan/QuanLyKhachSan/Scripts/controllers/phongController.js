var phongConfig = {
    pageSize: 10,
    pageIndex: 1,
}

var phongController = {

    init: function () {
        phongController.registerEvent();
    },
    registerEvent: function () {

        phongController.loadData();
      
        $(document).on("click", ".btnStatus", function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            phongController.changeStatus(id);
        });
       
        $(document).on("click", ".btnEdit", function (e) {
            e.preventDefault();
            $('#modalAddUpdate').modal('show');            
            var id = $(this).data('id');           
            phongController.loadDetailRoom(id);          
        });
      
        $(document).on("click", ".btnDelete", function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            bootbox.confirm("Are you sure?", function (result) {
                if (result) {
                    phongController.deleteRoom(id);
                }
            });
        });

        $('#btnAddNewRoom').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            phongController.loadDataForDropDownList();
            phongController.resetForm();
        });

        $('#frmSaveData').validate({
            rules: {
                txtNameRoom: {
                    required: true,
                    minlength: 1
                },
                ddlTypeRoom: {
                    required: true,
                },
                ddlLevelRoom: {
                    required: true,                
                }              
            },
            messages: {
                txtNameRoom: {
                    required: "Tên phòng không được để trống.",                   
                },
                ddlTypeRoom: {
                    required: "Bạn chưa chọn loại phòng",                   
                },
                ddlLevelRoom: {
                    required: "Bạn chưa chọn tầng",                   
                }
            }
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSaveData').valid()) {
                phongController.saveData();
            }
        });

        $('#btnSearch').off('click').on('click', function () {
            phongController.loadData(true);
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtNameSearch').val('');
            $('#ddlStatusSearch').val('');
            phongController.loadData(true);
        });

        $('#txtNameSearch').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                phongController.loadData(true);
            }
        });       
    },

    //Các hàm khởi tạo
    saveData: function () {
        var name = $('#txtNameRoom').val();
        var type = $("#ddlTypeRoom option:selected").text();
        var level = $('#ddlLevelRoom option:selected').text();             
        var id = parseInt($('#hiddenID').val());     

        var room = {
            Name: name,
            Type: type,
            Level: level,
            ID: id
        }
        $.ajax({
            type: 'POST',
            url: '/Admin/Phong/SaveDataRoom',
            data: room,
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Lưu lại thành công.", function () {
                        phongController.ClearPreview();
                        $('#modalAddUpdate').modal('hide');
                        phongController.loadData(true);
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
        var status = $('#ddlStatusSearch').val();
        $.ajax({
            url: "/Admin/Phong/LoadDataRoom",
            type: "GET",
            data: {
                name: name,
                status: status,
                page: phongConfig.pageIndex,
                pageSize: phongConfig.pageSize
            },
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#room-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            Image: item.Image,
                            Name: item.Name,
                            Type: item.Type,
                            Level: item.Level,
                            ID: item.ID            
                        });
                    });
                    $('#tblDataRoom').html(html);
                    phongController.paging(response.total, function () {
                        phongController.loadData();
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
        $('#txtNameRoom').val('');
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
    loadDetailRoom: function (id) {
        $.ajax({
            url: '/Admin/Phong/GetDetailRoom',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    $('#hiddenID').val(data.ID);
                    $('#txtNameRoom').val(data.Name);
                    $("#ddlTypeRoom option:selected").text(data.Type);
                    $('#ddlLevelRoom option:selected').text(data.Level);      
                    phongController.loadDataForDropDownList();
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
    deleteRoom: function (id) {
        $.ajax({
            url: '/Admin/Phong/DeleteRoom',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Xóa thành công", function () {
                        phongController.loadData(true);
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
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / phongConfig.pageSize);

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
                phongConfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    },
    loadDataForDropDownList: function () {
        $.ajax({
            url: '/Admin/Phong/GetTypeAndLevelRoom',
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

phongController.init();