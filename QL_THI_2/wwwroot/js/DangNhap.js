


// KiemTra()
function KiemTra() {
    var taiKhoan = $('#taiKhoan').val()
    var matKhau = $('#matKhau').val()
    if (taiKhoan == '' || taiKhoan == null) {
        $('#taiKhoan').focus()
    }
    else if (matKhau == '' || matKhau == null) {
        $('#matKhau').focus()
    }
    else {
        DangNhap()
    }
}

// DangNhap()
function DangNhap() {
    var taiKhoan = $('#taiKhoan').val()
    var matKhau = $('#matKhau').val()
    $.ajax({
        url: '/TaiKhoan/TaskDangNhap',
        type: 'post',
        data: {
            taiKhoan: taiKhoan,
            matKhau: matKhau
        },
        success: function (data) {
            if (data == 'error') {
                ThongBaoSai()
            }
            else if (data == 'admin') {
                window.location.href = '/HocPhan/DanhSachHocPhan'
            }
            else if (data == 'user') {
                window.location.href = '/ThongBao/ThongBaoMoi'
            }
        }
    })
}

// Error
function ThongBaoSai() {
    $('#buttonError').click()
}

$(document).ready(function () {
    $("#formAuthentication").submit(function (e) {
        e.preventDefault();
    });
})
