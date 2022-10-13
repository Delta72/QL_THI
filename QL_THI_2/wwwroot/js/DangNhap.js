


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
        cache: false,
        data: {
            taiKhoan: taiKhoan,
            matKhau: matKhau
        },
        beforeSend: function () {
            var str = '<div class="spinner-border text-light" style="height: 15px;width: 15px"><span class="visually-hidden"></span></div>'
            document.getElementById('buttonDangNhap').innerHTML = str;
        },
        complete: function () {
            document.getElementById('buttonDangNhap').innerHTML = 'Đăng nhập';
        },
        success: function (data) {
            if (data == 'error') {
                ThongBaoSai()
            }
            else if (data == 'admin') {
                window.location.href = '/HocPhan/DanhSachHocPhan'
            }
            else if (data == 'user') {
                window.location.href = '/ThongBao/DanhSachThongBao_GV'
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
