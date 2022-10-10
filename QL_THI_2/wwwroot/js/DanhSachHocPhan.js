


// Chon nam hoc
function ChonNamHoc() {
    $.ajax({
        url: '/HocPhan/LayNamHoc',
        type: 'get',
        success: function (data) {
            // console.log(data)
            for (var i in data) {
                var select = document.getElementById('selectHKB')
                var opt = document.createElement('option');
                opt.value = data[i];
                opt.innerHTML = data[i];
                select.appendChild(opt);
            }
        }
    })
}
$('#selectHKB').change(function () {
    document.getElementById('textHKK').value = parseInt($('#selectHKB').val()) + 1;
})

// sua duong dan
function ThemDuongDan() {
    str = '<strong>Học phần</strong> / Danh sách học phần'
    document.getElementById('DuongDan').innerHTML = str;
}

// Tim kiem
function TimKiem() {
    var hocKy = $('#selectHK').val();
    var namHocB = $('#selectHKB').val();

    if ((hocKy == null || hocKy == '') && (namHocB == null || namHocB == '')) {
        window.location.href = '/HocPhan/DanhSachHocPhan'
    }
    else if (hocKy == null || hocKy == '') {
        $('#selectHK').focus()
    }
    else if (namHocB == null || namHocB == '') {
        $('#selectHKB').focus()
    }
    else {
        window.location.href = '/HocPhan/TimKiemHocPhan?hocKy=' + hocKy + '&namHoc=' + namHocB;
    }
}

// Danh sach nhom
function DanhSachNhom(id) {
    window.location.href = '/Nhom/DanhSachNhomHocPhan?id=' + id
}

//ready
$(document).ready(function () {
    Active(1, 2)
    ChonNamHoc()
    ThemDuongDan()
})