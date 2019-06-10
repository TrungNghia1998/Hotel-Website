var hoaDonConfig = {
    pageSize: 10,
    pageIndex: 1,
}
var hoaDonController = {
    init: function () {
        hoaDonController.registerEvent();
    },
    registerEvent: function () {
        hoaDonController.loadData();

        $('#beginDate').datepicker({
            format: "dd/mm/yyyy",
            autoclose: true,
            todayHighlight: true,
            showOtherMonths: true,
            selectOtherMonths: true,
            autoclose: true,
            changeMonth: true,
            changeYear: true,
            orientation: "button"
        });

        $('#endDate').datepicker({
            format: "dd/mm/yyyy",
            autoclose: true,
            todayHighlight: true,
            showOtherMonths: true,
            selectOtherMonths: true,
            autoclose: true,
            changeMonth: true,
            changeYear: true,
            orientation: "button"
        });
    },
    loadData: function (changePageSize) {
        let now = new Date();

        let day = ("0" + now.getDate()).slice(-2);
        let month = ("0" + (now.getMonth() + 1)).slice(-2);

        let today = (day) + "-" + (month) + "-" + now.getFullYear();

        var beginDateSelected = $('#beginDate').val(today);
        var endDateSelected = $('#endDate').val(today);

        $.ajax({
            url: "/Admin/HoaDon/LoadDataBill",
            type: "GET",
            data: {
                beginDate: beginDateSelected,
                endDate: endDateSelected,
                page: hoaDonConfig.pageIndex,
                pageSize: hoaDonConfig.pageSize
            },
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#bill-template').html();
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
                    $('#tblDataBill').html(html);
                    hoaDonController.paging(response.total, function () {
                        hoaDonController.loadData();
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
    }
}

hoaDonController.init();