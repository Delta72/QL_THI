
// chi tiet nhom
function ChiTietNhom(id) {
    $.ajax({
        url: '/Nhom/ChiTietNhomThiCaNhanJson',
        type: 'post',
        data: {
            id: id,
        },
        success: function (data) {
            if (data.nhom.taiKhoan.id == data.tk) {
                window.location.href = '/Nhom/ChiTietNhomThiCaNhan?id=' + id
            } else {
                window.location.href = '/Nhom/ChiTietNhom?id=' + id
            }
        }
    })
}

// huy chinh sua
function HuyBoDS() {
    document.getElementById('_showDS').innerHTML = document.getElementById('_mainDS').innerHTML;
}

// hien chinh sua
function HienChinhSuaDS() {
    document.getElementById('_showDS').innerHTML = document.getElementById('_editDS').innerHTML;
    $('#tableEditDS tbody').sortable({
        helper: function (e, row) {
            var oCell = row.children();
            var cRow = row.clone();
            cRow.children().each(function (i) {
                $(this).width(oCell.eq(i).width());
            })
            return cRow;
        }
    })
}

// xoa nhom
function editXoaNhom(i) {
    var row = i.closest('tr');
    // console.log(row.id)
    document.getElementById('pXNXoaNhom').innerHTML = row.rowIndex;
}
function xnXoaNhom() {
    var r = document.getElementById('pXNXoaNhom').innerHTML
    document.getElementById('tableEditDS').deleteRow(parseInt(r));
}

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
                    str += '<tr onclick="ChonGV(\'' + data[i].id + '\',\'' + data[i].dn + '\',\'' + data[i].hoTen + '\')" class="trGV">'
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

// chon gv
function ChonGV(id, dn, hoten) {
    document.getElementById('textMGV').value = id
    document.getElementById('textTGV').value = hoten
    document.getElementById('textDN').value = dn
    document.getElementById('textGV').value = dn + ' - ' + hoten
}

// them gv
function ThemGV() {
    var id = document.getElementById('textMGV').value
    var dn = document.getElementById('textDN').value
    var ht = document.getElementById('textTGV').value
    if (id != null && id != "") {
        var hoten = document.getElementById('textTGV').value
        var img = GetUserAvatar(id);

        var table = document.getElementById("tableEditDS");
        var count = table.rows.length;
        var row = table.insertRow(count);
        row.id = id;

        var cell1 = row.insertCell(0);
        var cell2 = row.insertCell(1);
        var cell3 = row.insertCell(2);
        var cell4 = row.insertCell(3);

        cell1.className = 'count'

        var str = ''
        str += '<ul class="list-unstyled users-list m-0 avatar-group d-flex ulAvatar">'
        str += '<li data-bs-toggle="tooltip" data-popup="tooltip-custom" data-bs-placement="top" class="avatar avatar-xs pull-up" title=' + hoten + '">'
        str += '<img src="' + img + '" alt="Avatar" class="rounded-circle" />'
        cell2.innerHTML = str;

        cell3.innerHTML = dn + ' - ' + ht

        var str2 = '<i class="fa fa-trash" data-bs-toggle="modal" data-bs-target="#modalXNXoaNhom" onclick="editXoaNhom(this)" ></i>'
        cell4.innerHTML = str2;

        document.getElementById('modalDongThemNhomDS').click()
    }
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

// reset modal
function resetModal() {
    document.getElementById('tableTK').innerHTML = ""
    document.getElementById('textGV').value = ""
    document.getElementById('textMGV').value = ""
}

// xac nhan luu nhom
function xnLuuNhom(id) {
    var maHocPhan = id;
    var table = document.getElementById('tableEditDS')
    var rLength = table.rows.length;

    var ds = []
    for (i = 1; i < rLength; i++) {
        var row = table.rows[i];
        var id = row.id;
        var stt = row.rowIndex;
        ds.push({ "id": id, "stt": stt });
    }
    // console.log(ds)

    TaskChinhSuaNhom(maHocPhan, ds);
}
function TaskChinhSuaNhom(maHocPhan, ds) {
    $.ajax({
        url: '/HocPhan/ChinhSuaDanhSachNhom',
        type: 'post',
        data: {
            maHocPhan: maHocPhan,
            danhSach: JSON.stringify(ds),
            soNhom: ds.length,
        },
        beforeSend: function () {
            LoadingButton('btnxnXNLuuNhom');
        },
        success: function (data) {
            if (data == "error") {
                HienLoi("Có lỗi xảy ra, vui lòng thử lại sau!")
            }
            else if (data == "nhomtrong") {
                HienLoi("Không có dữ liệu nhóm trong học phần!")
            }
            else {
                window.location.reload()
            }
        }
    })
}

$(document).ready(function () {
    document.getElementById('_showDS').innerHTML = document.getElementById('_mainDS').innerHTML;
})