

document.getElementById('showCS').innerHTML = document.getElementById('mainCS').innerHTML

$('#slB').on('change', function (e) {
    document.getElementById('slK').value = parseInt($('#slB').val()) + 1;
})

// Use datepicker on the date inputs
$("input[type=date]").datepicker({
    dateFormat: 'yy-mm-dd',
    onSelect: function (dateText, inst) {
        $(inst).val(dateText); // Write the value in the input
    }
});

// Code below to avoid the classic date-picker
$("input[type=date]").on('click', function () {
    return false;
});

// Xoa
function XoaSpan(i) {
    var span = document.getElementById('span' + i)
    span.remove()
}

// Them thanh phan
function ThemThanhPhan() {
    var str = document.getElementById('txtTP').value;
    if (str != "") {
        var id = document.getElementById('divThanhPhanDiem').children.length + 1;
        var s = '<span class="tpdiem badge badge-primary" id="span' + id + '">' + str + ' |<i class="fa fa-times" onclick="XoaSpan(' + id + ')"></i></span>'
        document.getElementById('divThanhPhanDiem').innerHTML += s;
        document.getElementById('txtTP').value = ""
        document.getElementById('txtTP').focus()
    }
}

// chinh sua CS
function HienChinhSua(id, mhp) {
    $.ajax({
        url: '/HocPhan/KiemTraChinhSua',
        type: 'post',
        data: {
            id: id,
        },
        success: function (data) {
            LayDanhSachHocPhan(mhp)
            if (data == "outdated") {
                document.getElementById('showCS').innerHTML = document.getElementById('ChinhSuaCS').innerHTML
                document.getElementById('txtTP').disabled = true;
                document.getElementById('txtTPHPedit').innerHTML = '<span class="alert alert-danger">Không thể chỉnh sửa điểm thành phần (Quá hạn)</span>'
                document.getElementById('divThanhPhanDiem').innerHTML = 'same'
                document.getElementById('divThanhPhanDiem').style.display = 'none'
            }
            else if (data == "error") {
                HienLoi()
            }
            else if (data == "submitted") {
                document.getElementById('showCS').innerHTML = document.getElementById('ChinhSuaCS').innerHTML
                document.getElementById('txtTP').disabled = true;
                document.getElementById('txtTPHPedit').innerHTML = '<span class="alert alert-danger">Không thể chỉnh sửa điểm thành phần (Đã có nhóm nộp)</span>'
                document.getElementById('divThanhPhanDiem').innerHTML = 'same'
                document.getElementById('divThanhPhanDiem').style.display = 'none'
                document.getElementById('selectHocKy').disabled = true;
                document.getElementById('slB').disabled = true;
                document.getElementById('selectMHP').disabled = true;
                
            }
            else {
                document.getElementById('showCS').innerHTML = document.getElementById('ChinhSuaCS').innerHTML
            }
        }
    })
}

// huy bo cs
function HuyBoCS() {
    document.getElementById('showCS').innerHTML = document.getElementById('mainCS').innerHTML
}

// Lay danh sach hoc phan
function LayDanhSachHocPhan(id) {
    $.ajax({
        url: '/HocPhan/LayMaHocPhan',
        type: 'get',
        success: function (data) {
            var select = document.getElementById('selectMHP')
            for (var i in data) {
                var opt = document.createElement('option');
                opt.value = data[i].id;
                opt.innerHTML = data[i].ma + ' - ' + data[i].tenHocPhan;
                if (data[i].id == parseInt(id)) {
                    opt.selected = true;
                }
                select.appendChild(opt);
            }
        }
    })
}

// luu
function LuuCS() {
    var tPhan = document.getElementById('divThanhPhanDiem').innerText.replaceAll('\n', "").replaceAll("  ", "").slice(0, -1)
    if (tPhan.length == 0) {
        document.getElementById('txtTP').focus()
    }
    else {
        $('#btnModalLuuCS').click()
    }
}

// luu
function TaskLuu(id) {
    var hKy = document.getElementById('selectHocKy').value
    var nHoc = document.getElementById('slB').value
    var mHP = document.getElementById('selectMHP').value
    var hNop = document.getElementById('dateHanNop').value
    var tPhan = document.getElementById('divThanhPhanDiem').innerText.replaceAll('\n', "").replaceAll("  ", "")
    $.ajax({
        url: '/HocPhan/ChinhSuaHocPhan',
        type: 'post',
        data: {
            id: id,
            hocKy: hKy,
            namHoc: nHoc,
            maHocPhan: mHP,
            hanNop: hNop,
            thanhPhan: tPhan,
        },
        beforeSend: function () {
            LoadingButton('btnLuu');
        },
        complete: function () {
            document.getElementById('btnLuu').innerHTML = 'Lưu'
        },
        success: function (data) {
            if (data == "error") {
                HienLoi("Có lỗi xảy ra, vui lòng thử lại sau!")
            }
            else if (data == 'exists') {
                HienLoi("Mã học phần đã tồn tại!")
            }
            else {
                window.location.reload()
            }
        }
    })
}

function XNGuiMail() {
    $('#btnModalMail').click();
}

function GuiMail(id) {
    $.ajax({
        url: '/Mail/GuiMailThongBao',
        type: 'post',
        data: {
            id: id,
        },
        beforeSend: function () {
            LoadingButton('btnGuiMail')
        },
        success: function () {
            window.location.reload()
        }
    })
}