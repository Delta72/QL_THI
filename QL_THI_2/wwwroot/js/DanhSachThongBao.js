function ThemDuongDan() {
    str = '<strong>Thông báo</strong> / Danh sách thông báo'
    document.getElementById('DuongDan').innerHTML = str;
}

// Hien noi dung
function HienNoiDung(id) {
    $.ajax({
        url: '/ThongBao/ChiTietThongBao',
        type: 'post',
        data: {
            id: id
        },
        success: function (data) {
            document.getElementById('modalTB').innerHTML = data
        }
    })
}

// Xoa thong bao
function XoaThongBao(id) {
    document.getElementById('idXoa').value = id
}

// ham xoa
function TaskXoaThongBao() {
    var id = document.getElementById('idXoa').value
    $.ajax({
        url: '/ThongBao/XoaThongBao',
        type: 'post',
        data: {
            id: id,
        },
        success: function (data) {
            window.location.reload()
        }
    })
}

// Tim kiem tai khoan
$('#SearchTB').on('change input copy paste', function () {
    // console.log(this.value)
    var str = this.value;
    var p = 1
    TimKiemTB(str, p)
})
function TimKiemTB(str, p) {
    $.ajax({
        url: '/ThongBao/TimKiemThongBao',
        type: 'post',
        data: {
            str: str,
            p: p
        },
        success: function (data) {
            document.getElementById('tableBody').innerHTML = data
        }
    })
}

$(document).ready(function () {
    Active(2, 4)
    ThemDuongDan();
})