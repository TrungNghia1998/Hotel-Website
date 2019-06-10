var goiDichVu = {
    init: function () {
        goiDichVu.registerEvent();
    },
    registerEvent: function () {
        $('#btnGoBack').off('click').on('click', function () {
            var url = "/Admin/Home/TatCaPhong/";
            window.location.href = url;
        });
    }
}

goiDichVu.init();