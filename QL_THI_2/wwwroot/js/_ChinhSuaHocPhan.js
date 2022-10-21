console.log(document.getElementById('divThanhPhanDiem').innerText.replaceAll('\n',"").replaceAll("  ", "").slice(0, -1))

document.getElementById('showCS').innerHTML = document.getElementById('ChinhSuaCS').innerHTML

$('#slB').on('change', function (e) {
    document.getElementById('slK').value = parseInt($('#slB').val()) + 1;
})

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
    }
}

// chinh sua CS
function HienChinhSua() {
    document.getElementById('showCS').innerHTML = document.getElementById('ChinhSuaCS').innerHTML
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
            // console.log(data)
            for (var i in data) {
                var select = document.getElementById('selectMHP')
                var opt = document.createElement('option');
                opt.value = data[i].id;
                opt.innerHTML = data[i].id + ' - ' + data[i].tenHocPhan;
                if (data[i].id == id) {
                    opt.selected = 'selected'
                }
                select.appendChild(opt);
            }
        }
    })
}