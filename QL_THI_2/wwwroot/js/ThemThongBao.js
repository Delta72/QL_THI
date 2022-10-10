

// dem
$('#txtNoiDung').on('input change', function () {
    var c = $('#txtNoiDung').val().length
    document.getElementById('dem').innerHTML = c + '/4000'

    if (c > 3000) {
        document.getElementById('dem').classList.remove('bg-dark')
        document.getElementById('dem').classList.add('bg-warning')
    }
    if (c == 4000) {
        document.getElementById('dem').classList.remove('bg-warning')
        document.getElementById('dem').classList.add('bg-danger')
    }
})

// Kiem tra
function KiemTra() {
    var tuaDe = $('#txtTuaDe').val()
    var noiDung = $('#txtNoiDung').val()

    if (tuaDe.length > 4000 || noiDung.length > 4000) {
        $('#buttonError').click()
    }
    else if (tuaDe.length > 0 && noiDung.length > 0) {
        $('#buttonSubmit').click()
    }
}

// them thong bao
function ThemThongBaoMoi() {
    var tuaDe = $('#txtTuaDe').val()
    var noiDung = $('#txtNoiDung').val()

    $.ajax({
        url: '/ThongBao/TaskThemThongBao',
        type: 'post',
        data: {
            tuaDe: tuaDe,
            noiDung: noiDung,
        },
        success: function (data) {
            window.location.href = '/ThongBao/DanhSachThongBao';
        }
    })
}

// sua duong dan
function ThemDuongDan() {
    str = '<strong>Thông báo</strong> / Thêm thông báo mới'
    document.getElementById('DuongDan').innerHTML = str;
}

//ready
$(document).ready(function () {
    Active(2, 3)
    ThemDuongDan()
})