

function XNDoiThongTin() {
    var f = document.getElementById('upload').files
    var dn = $('#TK_DN').val()
    var ht = $('#TK_HT').val()
    var email = $('#TK_Email').val()

    var span = document.getElementById('spanThongTin')
    if (dn == '') {
        span.innerHTML = '*Tên đăng nhập trống'
    }
    else if (ht == '') {
        span.innerHTML = '*Họ tên trống'
    }
    else if (email == '') {
        span.innerHTML = '*Email trống'
    }
    else {
        span.innerHTML = ''
        var frm = new FormData();
        if (f.length > 0) {
            frm.append('img', f[0])
        }
        frm.append('dn', dn)
        frm.append('hoTen', ht)
        frm.append('email', email)
        $.ajax({
            url: '/TaiKhoan/SuaThongTin',
            type: 'post',
            dataType: 'json',
            processData: false,
            contentType: false,
            data: frm,
            beforeSend: function () {
                LoadingButton('btnThongTin')
            },
            complete: function () {
                document.getElementById('btnThongTin').innerHTML = 'Lưu thay đổi'
            },
            success: function (data) {
                if (data == 'error') {
                    HienLoi()
                }
                else if (data == 'dn') {
                    HienLoi('Tên đăng nhập đã tồn tại')
                }
                else {
                    window.location.reload();
                }
            }
        })
    }
}

function XNDoiMK() {
    var p1 = $('#oldPass').val()
    var p2 = $('#newPass').val()
    var p3 = $('#cNewPass').val()

    if (p1 == '') {
        document.getElementById('spanMK').innerHTML = '*Mật khẩu trống'
    }
    else if (p2 != p3) {
        document.getElementById('spanMK').innerHTML = '*Mật khẩu không trùng khớp'
    }
    else {
        $.ajax({
            url: '/TaiKhoan/DoiMatKhau',
            type: 'post',
            data: {
                p1: p1,
                p2: p2,
            },
            beforeSend: function () {
                LoadingButton('btnMK')
            },
            complete: function () {
                document.getElementById('btnMK').innerHTML = 'Lưu thay đổi'
            },
            success: function (data) {
                if (data == 'error') {
                    HienLoi()
                }
                else if (data == 'mk') {
                    HienLoi("Mật khẩu cũ không trùng khớp")
                }
                else {
                    window.location.reload();
                }
            }
        })
    }
}