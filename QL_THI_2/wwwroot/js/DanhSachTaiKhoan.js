function ThemDuongDan() {
    str = '<strong>Tài khoản</strong> / Danh sách tài khoản'
    document.getElementById('DuongDan').innerHTML = str;
}


// Hien xac nhan
function HienXacNhan(id, k, table) {
    document.getElementById('idKhoa').value = id
    var row = table.closest('tr')
    document.getElementById('idRow').value = row.rowIndex
    if (k == "True" || k == "true" || k == true) {
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
            if (data == "error") {
                HienLoi()
            } else { CapNhatKhoa(rowI, data.id, data.hd) }
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

// Kiem tra ton tai tai khoan
function KiemTraID(id) {
    var bool = false
    $.ajax({
        url: '/TaiKhoan/KiemTraID',
        type: 'post',
        async: false,
        data: {
            id: id
        },
        success: function (data) {
            if (data == true) {
                bool = true
            }
        }
    })
    return bool
}

// Hien xac nhan them
function HienXacNhanThem() {
    var id = document.getElementById('IDthem').value
    var mk = document.getElementById('MKthem').value
    var xnmk = document.getElementById('XNMKthem').value
    var hoten = document.getElementById('Namethem').value
    var email = document.getElementById('Emailthem').value
    var regexExp = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/gi;

    if (id == "" || id == null) {
        document.getElementById('alert').innerHTML = "*ID trống!"
    }
    else if (KiemTraID(id) == true) {
        document.getElementById('alert').innerHTML = "*ID đã tồn tại!"
    }
    else if (mk == "" || mk == null) {
        document.getElementById('alert').innerHTML = "*Mật khẩu trống!"
    }
    else if (xnmk != mk) {
        document.getElementById('alert').innerHTML = "*Mật khẩu không khớp!"
    }
    else if (hoten == "" || hoten == null) {
        document.getElementById('alert').innerHTML = "*Họ tên trống"
    }
    else if (email == "" || email == null) {
        document.getElementById('alert').innerHTML = "*Email trống"
    }
    else if (!regexExp.test(email)) {
        document.getElementById('alert').innerHTML = "*Email không hợp lệ!"
    }
    else {
        document.getElementById('alert').innerHTML = ""
        var input = document.getElementsByClassName('inputThem')
        for (var i = 0; i < input.length; i++) {
            input[i].disabled = true;
        }
        var footer = document.getElementById('footerThem')
        var str = ''
        str += '<h6 class="modal-footer" style="margin-bottom: -20px">Thêm tài khoản này?</h6>'
        str += '<div class="modal-footer">'
        str += '<button type="button" class="btn btn-outline-secondary" onclick="TroLai()">Trở lại</button>'
        str += '<button type="button" onclick="ThemTaiKhoan()" class="btn btn-success">Xác nhận</button>'
        str == '</div>'

        footer.innerHTML = str
    }
}
function TroLai() {
    var input = document.getElementsByClassName('inputThem')
    for (var i = 0; i < input.length; i++) {
        input[i].disabled = false;
    }
    var footer = document.getElementById('footerThem')
    var str = '<div class="modal-footer">'
    str += '<button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Đóng</button>'
    str += '<button type="button" onclick="HienXacNhanThem()" class="btn btn-primary">Thêm</button>'
    str == '</div>'

    footer.innerHTML = str
}

// Them tai khoan
function ThemTaiKhoan() {
    var id = document.getElementById('IDthem').value
    var mk = document.getElementById('MKthem').value
    var hoten = document.getElementById('Namethem').value
    var email = document.getElementById('Emailthem').value

    $.ajax({
        url: '/TaiKhoan/TaskThemTaiKhoan',
        type: 'post',
        data: {
            id: id,
            mk: mk,
            hoTen: hoten,
            email: email
        },
        success: function (data) {
            document.getElementById('btnThem_dong').click()
            HienTaiKhoanVuaThem(data)
        }
    })
}

// hien tai khoan vua them
function HienTaiKhoanVuaThem(data) {
    console.log(data)
    var table = document.getElementById('DSTK')
    var row = table.insertRow(1)

    var cell0 = row.insertCell(0);
    var cell1 = row.insertCell(1);
    var cell2 = row.insertCell(2);
    var cell3 = row.insertCell(3);
    var cell4 = row.insertCell(4);
    var cell5 = row.insertCell(5);
    var cell6 = row.insertCell(6);
    var cell7 = row.insertCell(7);

    cell0.innerHTML = '<span class="badge rounded-pill bg-success" style="margin-right: 10px">Mới</span><strong>' + data.id + '</strong>'

    var str1 = '<div style="text-align: center;">'
    str1 += '<ul class="list-unstyled users-list m-0 avatar-group d-flex align-items-center" style="text-align: center !important; justify-content: center">'
    str1 += '<li data-bs-toggle="tooltip" data-popup="tooltip-custom" data-bs-placement="top" class="avatar avatar-xs pull-up" title="@i.hoTen" style="text-align: center !important">'
    str1 += '<img src="' + data.avatar + '" alt="Avatar" class="rounded-circle" />'
    str1 += '</li></ul>'
    str1 += '</div>'
    cell1.innerHTML = str1

    var hoten = (data.hoTen).split(" ")
    var ho = ''
    for (var i = 0; i < hoten.length - 1; i++) {
        ho += hoten[i] + ' '
    }
    var ten = hoten[hoten.length - 1]
    cell2.innerHTML = ho
    cell3.innerHTML = ten
    cell4.innerHTML = data.email
    cell5.innerHTML = data.lanHDCuoi

    var str6 = ''
    str6 += '<div style="text-align: center;">'
    if (data.isAdmin == true) {
        str6 += '<strong>X</strong>'
    }
    str6 += '</div>'
    cell6.innerHTML = str6

    var str7 = ''
    str7 += '<div class="dropdown" style="z-index: 1 !important">'
    str7 += '<p style="display: none">@i.hoatDong</p>'
    str7 += '<button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="modal" data-bs-target="#modalXacNhanKhoa" id="buttonChiTietNhom" onclick="HienXacNhan(\'' + data.id + '\', \'' + data.hoatDong + '\', this)" >'
    if (data.hoatDong == true) {
        str7 += '<i class="bx bx-lock-open-alt"></i>'
    }
    else {
        str7 += '<i class="bx bx-lock" style="color: red"></i>'
    }
    str7 += '</button></div>'
    cell7.innerHTML = str7

}

$(".latin_letters").on("keypress", function (event) {
    var englishAlphabetAndWhiteSpace = /^[-@./#&+\w\s]*$/;
    var key = String.fromCharCode(event.which);
    if (event.keyCode == 8 || event.keyCode == 37 || event.keyCode == 39 || englishAlphabetAndWhiteSpace.test(key)) {
        return true;
    }
    return false;
});
$('.latin_letters').on("paste", function (e) {
    e.preventDefault();
});

// Tim kiem tai khoan
$('#SearchTK').on('change input copy paste', function () {
    // console.log(this.value)
    var str = this.value;
    $.ajax({
        url: '/TaiKhoan/TimKiemTaiKhoanDS',
        type: 'post',
        data: {
            str: str
        },
        success: function (data) {
            document.getElementById('tableBody').innerHTML = data
        }
    })
})

$(document).ready(function () {
    Active(3, 5)
    ThemDuongDan();
})