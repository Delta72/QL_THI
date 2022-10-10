function ThemDuongDan() {
    str = '<strong>Tài khoản</strong> / Danh sách tài khoản'
    document.getElementById('DuongDan').innerHTML = str;
}

// Hien xac nhan
function HienXacNhan(id, k, table) {
    document.getElementById('idKhoa').value = id
    var row = table.closest('tr')
    document.getElementById('idRow').value = row.rowIndex
    if (k == "True") {
        document.getElementById('h4XN').innerHTML = "Khóa tài khoản này?"
        document.getElementById('btnXN').innerHTML = "Khóa"
    }
    else {
        document.getElementById('h4XN').innerHTML = "Mở khóa tài khoản này?"
        document.getElementById('btnXN').innerHTML = "Mở khóa"
    }
}

// task khoa tk
function TaskKhoaTaiKhoan() {
    var id = document.getElementById('idKhoa').value
    var rowI = document.getElementById('idRow').value

    $.ajax({
        url: '/TaiKhoan/KhoaTaiKhoan',
        type: 'post',
        async: false,
        data: {
            id: id,
        },
        success: function (data) {
            //console.log(data)
            CapNhatKhoa(rowI, data.id, data.hd)
        }
    })
}

// Cap nhat khoa
function CapNhatKhoa(index, id, hd) {
    //console.log(id, hd)
    var table = document.getElementById('DSTK')
    var td = table.rows[index].cells[7]

    var str = ''
    str += '<div class="dropdown" style="z-index: 1 !important">'
    str += '<button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="modal" data-bs-target="#modalXacNhanKhoa"'
    str += 'id="buttonChiTietNhom" onclick="HienXacNhan(\'' + id + '\', \'' + hd + '\', this)">'
    if (hd == true) {
        str += '<i class="bx bx-lock-open-alt"></i>'
    }
    else {
        str += '<i class="bx bx-lock" style="color: red"></i>'
    }
    str += '</button>'
    str += '</div>'
    td.innerHTML = str

    $('#modalDong').click();
}

$(document).ready(function () {
    Active(3, 6)
    ThemDuongDan();
})