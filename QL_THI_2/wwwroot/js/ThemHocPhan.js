
// active page
function Active() {
    document.getElementById('liThemHocPhan').classList.add("active")
    document.getElementById('liDanhSachHocPhan').classList.remove("active")
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

// Lay danh sach hoc ky
function LayDanhSachHocKy() {
    $.ajax({
        url: '/HocPhan/LayMaHocKy',
        type: 'get',
        success: function (data) {
            // console.log(data)
            for (var i in data) {
                var select = document.getElementById('selectHK')
                var opt = document.createElement('option');
                opt.value = data[i].id;
                opt.innerHTML = data[i].tenHocKy;
                select.appendChild(opt);
            }
        }
    })
}

// Chon nam hoc
function ChonNamHoc() {
    var date = new Date().getFullYear()
    for (i = date; i < (date + 10); i++) {
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
                    str += '<tr onclick="ChonGV(\'' + data[i].id +'\',\'' + data[i].hoTen + '\')" class="trGV">'
                    str += '<td><small>' + data[i].id + " - " + data[i].hoTen + '</small></td>'
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

// chon gv
function ChonGV(id, hoten) {
    document.getElementById('textMGV').value = id
    document.getElementById('textTGV').value = hoten
    document.getElementById('textGV').value = id + ' - ' + hoten
}
function ThemGV() {
    var id = document.getElementById('textMGV').value
    if (id != null && id != "") {
        var hoten = document.getElementById('textTGV').value
        var img = GetUserAvatar(id);

        var table = document.getElementById("tableThemNhom");
        var count = table.rows.length;
        var row = table.insertRow(count);

        var cell1 = row.insertCell(0);
        var cell2 = row.insertCell(1);
        var cell3 = row.insertCell(2);

        cell1.innerHTML = (count).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        str = ''
        str += '<ul class="list-unstyled users-list m-0 avatar-group d-flex align-items-center">'
        str += '<li data-bs-toggle="tooltip" data-popup="tooltip-custom" data-bs-placement="top" class="avatar avatar-xs pull-up" title="' + id + ' - ' + hoten + '">'
        str += '<img src="' + img + '" alt="Avatar" class="rounded-circle" /></li><li>' + hoten + '</li></ul>'
        cell2.innerHTML = str
        cell3.innerHTML = '<div class="dropdown"><button type="button" class="btn p-0 dropdown-toggle hide-arrow" onclick="XoaNhom(' + count + ')"><i class="bx bx-trash"></i></button></div>'

        document.getElementById('modalDong').click();
    }
}

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

//ready
$(document).ready(function () {
    Active()
    LayDanhSachHocPhan()
    LayDanhSachHocKy()
    ChonNamHoc()
})