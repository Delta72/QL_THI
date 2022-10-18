

// Them thanh phan
function ThemThanhPhan() {
    var str = document.getElementById('txtTP').value;
    if (str != "") {
        var id = document.getElementById('divThanhPhanDiem').children.length + 1;
        var s = '<span class="tpdiem badge badge-primary" id="span' + id + '">' + str + ' |<i class="fa fa-times" onclick="Xoa(' + id + ')"></i></span>'
        document.getElementById('divThanhPhanDiem').innerHTML += s;
        document.getElementById('txtTP').value = ""
    }
}

// Xoa
function Xoa(i) {
    var span = document.getElementById('span' + i)
    span.remove()
}

// Lay danh sach hoc phan
function LayDanhSachHocPhan() {
    $.ajax({
        url: '/HocPhan/LayMaHocPhan',
        type: 'get',
        success: function (data) {
            // console.log(data)
            for (var i in data) {
                var select = document.getElementById('selectMHP')
                var opt = document.createElement('option');
                opt.value = data[i].id;
                opt.innerHTML = data[i].id + ' - ' + data[i].tenHocPhan;
                select.appendChild(opt);
            }
        }
    })
}

// Chon nam hoc
function ChonNamHoc() {
    var date = new Date().getFullYear()
    for (i = date; i < (date + 5); i++) {
        var select = document.getElementById('selectHKB')
        var opt = document.createElement('option');
        opt.value = i;
        opt.innerHTML = i;
        select.appendChild(opt);
    }
}

$('#selectHKB').change(function () {
    document.getElementById('textHKK').value = parseInt($('#selectHKB').val()) + 1;
})

// tim nguoi
$('#textGV').on('change input copy paste', function () {
    document.getElementById('textMGV').value = ""
    // console.log("A")
    var txt = $('#textGV').val()
    if (txt.length > 0) {
        $.ajax({
            url: '/TaiKhoan/TimKiemTaiKhoan',
            type: 'post',
            data: {
                str: txt,
            },
            success: function (data) {
                str = ''
                str += '<table class="table table-hover">'
                str += '<thead></thead>'
                str += '<tbody class="table-border-bottom-0">'
                for (var i in data) {
                    str += '<tr onclick="ChonGV(\'' + data[i].id + '\',\'' + data[i].dn +'\',\'' + data[i].hoTen + '\')" class="trGV">'
                    str += '<td><small>' + data[i].dn + " - " + data[i].hoTen + '</small></td>'
                    str += '<td><ul class="list-unstyled users-list m-0 avatar-group d-flex align-items-center">'
                    str += '<li data-bs-toggle="tooltip" data-popup="tooltip-custom" data-bs-placement="top" class="avatar avatar-xs pull-up" title = "' + data[i].hoTen + '" >'
                    str += '<img src="' + data[i].avatar + '" alt="Avatar" class="rounded-circle" /></li></ul></td>'
                    str += '</tr>'
                }
                str += '</tbody>'
                str += '</table>'

                document.getElementById('tableTK').innerHTML = str;
                
            }
        })
    }
})



// Xoa nhom
function XoaNhom(c) {
    // console.log(c)
    var table = document.getElementById("tableThemNhom")
    table.deleteRow(c);
    var total = table.rows.length;
    for (var i = 0; i < total; i++) {
        if (i > 0) {
            table.rows[i].cells[0].innerHTML = (i).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });;
        }
    }
}

// reset modal
function resetModal() {
    document.getElementById('tableTK').innerHTML = ""
    document.getElementById('textGV').value = ""
    document.getElementById('textMGV').value = ""
}

// lay avatar
function GetUserAvatar(id) {
    var img = ""
    $.ajax({
        url: '/TaiKhoan/LayAvatar',
        type: 'post',
        async: false,
        data: {
            id: id,
        },
        success: function (data) {
            img = data
        }
    })
    return img;
}

// Kiem tra du lieu
function KiemTra() {
    var maHP = $('#selectMHP').val();
    var hocKy = $('#selectHK').val();
    var namHocB = $('#selectHKB').val();
    var hanNop = $('#ngayNop').val();
    var diemTP = document.getElementById('divThanhPhanDiem').innerText;

    var table = document.getElementById("tableThemNhom")
    var rowLength = table.rows.length;

    if (maHP == null || maHP == '') {
        $('#selectMHP').focus()
    }
    else if (hocKy == null || hocKy == '') {
        $('#selectHK').focus()
    }
    else if (namHocB == null || namHocB == '') {
        $('#selectHKB').focus()
    }
    else if (hanNop == null || hanNop == '') {
        $('#ngayNop').focus()
    }
    else if (diemTP.length == 0) {
        $('#txtTP').focus()
    }
    else if (rowLength < 2) {
        $('#buttonThemNhom').focus()
        document.getElementById('buttonThemNhom').classList.remove('btn-primary')
        document.getElementById('buttonThemNhom').classList.add('btn-danger')
    }
    else {
        $('#buttonSubmit').click()
    }
}

// chon gv
function ChonGV(id, dn, hoten) {
    document.getElementById('textMGV').value = id
    document.getElementById('textTGV').value = hoten
    document.getElementById('textDN').value = dn
    document.getElementById('textGV').value = dn + ' - ' + hoten
}
function ThemGV() {
    var id = document.getElementById('textMGV').value
    var dn = document.getElementById('textDN').value
    if (id != null && id != "") {
        var hoten = document.getElementById('textTGV').value
        var img = GetUserAvatar(id);

        var table = document.getElementById("tableThemNhom");
        var count = table.rows.length;
        var row = table.insertRow(count);

        var cell1 = row.insertCell(0);
        var cell2 = row.insertCell(1);
        var cell3 = row.insertCell(2);
        var cell4 = row.insertCell(3);
        var cell5 = row.insertCell(4);

        cell1.innerHTML = (count).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        cell2.innerHTML = '<p style="display: none" disabled>' + id + '</p>'
        cell3.innerHTML = dn
        str = ''
        str += '<ul class="list-unstyled users-list m-0 avatar-group d-flex align-items-center">'
        str += '<li data-bs-toggle="tooltip" data-popup="tooltip-custom" data-bs-placement="top" class="avatar avatar-xs pull-up" title="' + dn + ' - ' + hoten + '">'
        str += '<img src="' + img + '" alt="Avatar" class="rounded-circle" /></li><li style="margin-left: 10px">' + hoten + '</li></ul>'
        cell4.innerHTML = str
        cell5.innerHTML = '<div class="dropdown"><button type="button" class="btn p-0 dropdown-toggle hide-arrow" onclick="XoaNhom(' + count + ')"><i class="bx bx-trash"></i></button></div>'

        document.getElementById('modalDong').click()
        document.getElementById('buttonThemNhom').classList.remove('btn-danger')
        document.getElementById('buttonThemNhom').classList.add('btn-primary')
    }
}

// Them hoc phan moi
function ThemHocPhanMoi() {
    var maHP = $('#selectMHP').val();
    var hocKy = $('#selectHK').val();
    var namHocB = $('#selectHKB').val();
    var namHocK = $('#textHKK').val();
    var hanNop = $('#ngayNop').val();
    var diemTP = document.getElementById('divThanhPhanDiem').innerText;

    var table = document.getElementById("tableThemNhom")
    var rowLength = table.rows.length;

    var arrayNhom = []
    var arrayHP = []

    arrayHP.push({
        "maHocPhan": maHP,
        "maHocKy": hocKy,
        "namHocB": namHocB,
        "namHocK": namHocK,
        "hanNop": hanNop,
        "diemTP": diemTP,
    })

    for (i = 1; i < rowLength; i++) {
        var oCells = table.rows.item(i).cells;
        var cellLength = oCells.length;
        var id = []
        for (var j = 0; j < cellLength - 3; j++) {
            var cellVal = oCells.item(j).innerHTML;
            // console.log(cellVal);
            id[j] = cellVal;
        }
        arrayNhom.push({ "idN": id[0], "idGV": id[1] })
    }
    // console.log(arrayHP, arrayNhom);
    $.ajax({
        url: '/HocPhan/TaskThemHocPhan',
        type: 'post',
        data: {
            jsonHP: JSON.stringify(arrayHP),
            jsonNhom: JSON.stringify(arrayNhom),
            soNhom: (rowLength - 1)
        },
        success: function (data) {
            if (data == "error") {
                HienLoi("Có lỗi xảy ra, vui lòng thử lại sau")
            }
            else if (data == 'exists') {
                HienLoi("Đã tồn tại mã học phần này trong niên khóa")
            }
            else {
                window.location.href = '/Nhom/DanhSachNhomHocPhan?id=' + data
            }
        }
    })
}

// sua duong dan
function ThemDuongDan() {
    str = '<strong>Học phần</strong> / Thêm học phần mới'
    document.getElementById('DuongDan').innerHTML = str;
}

//ready
$(document).ready(function () {
    Active(1, 1)
    LayDanhSachHocPhan()
    ChonNamHoc()
    $("#formThemHocPhan").submit(function (e) {
        e.preventDefault();
    });
    ThemDuongDan()
})