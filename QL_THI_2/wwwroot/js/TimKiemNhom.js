
Active(3, 6)

document.getElementById('DuongDan').innerHTML = '<strong>Tìm kiếm</strong> / Nhóm'

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
                window.location.href = '/Nhom/ChiTietNhomThiCaNhan?id=' + data.nhom.id
            } else {
                window.location.href = '/Nhom/ChiTietNhom?id=' + data.nhom.id
            }
        }
    })
}

function HienBoLoc() {
    $.ajax({
        url: '/TimKiem/HienBoLocNhom',
        type: 'get',
        success: function (data) {
            document.getElementById('locDSGV').style.display = 'none'
            // console.log(data)
            // hien ma hoc phan
            var selectMHP = document.getElementById('selectMHP')
            for (var i in data.mhp) {
                var opt = document.createElement('option')
                opt.value = data.mhp[i].id
                opt.innerHTML = data.mhp[i].ma + ' - ' + data.mhp[i].tenHocPhan
                selectMHP.appendChild(opt)
            }

            // hien hinh thuc thi
            var selectHT = document.getElementById('selectHT')
            for (var i in data.ht) {
                var opt = document.createElement('option')
                opt.value = (parseInt(i) + 1)
                opt.innerHTML = data.ht[i].tenHinhThuc
                selectHT.appendChild(opt)
            }

            // hien nam hoc
            var selectNH = document.getElementById('selectNH')
            for (var i in data.nh) {
                var opt = document.createElement('option')
                opt.value = data.nh[i].split(' - ')[0]
                opt.innerHTML = data.nh[i]
                selectNH.appendChild(opt)
            }
        }
    })
}
HienBoLoc()
document.getElementById('locDSGV').style.display = 'none'
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
                str += '<table class="table table-hover table-sm">'
                str += '<thead></thead>'
                str += '<tbody class="table-border-bottom-0">'
                for (var i in data) {
                    str += '<tr onclick="ChonGV(\'' + data[i].id + '\',\'' + data[i].dn + '\',\'' + data[i].hoTen + '\')" class="trGV">'
                    str += '<td><small>' + data[i].dn + " - " + data[i].hoTen + '</small></td>'
                    str += '<td>'
                    str += '<img src="' + data[i].avatar + '" class="rounded-circle avatar avatar-xs" data-bs-toggle="tooltip" data-popup="tooltip-custom" data-bs-placement="top" title="' + data[i].dn + ' - ' + data[i].hoTen + '"/></td>'
                    str += '</tr>'
                }
                str += '</tbody>'
                str += '</table>'

                document.getElementById('tableTK').innerHTML = str;
            }
        })
    }
    else {
        document.getElementById('tableTK').innerHTML = "";
    }
})

// reset modal
function resetModal() {
    document.getElementById('tableTK').innerHTML = ""
    document.getElementById('textMGV').value = ""
    $('#textGV').val("")
}

// chon gv
function ChonGV(id, dn, hoten) {
    document.getElementById('textMGV').value = id
    document.getElementById('textGV').value = dn + ' - ' + hoten
}

function ThemGV() {
    var id = $('#textMGV').val();
    if (TonTaiGV(id)) {
        if (id != null && id != "") {
            $.ajax({
                url: '/TaiKhoan/LayThongTinTaiKhoanJson',
                type: 'post',
                data: { id: id },
                success: function (data) {
                    document.getElementById('locDSGV').style.display = 'unset'
                    var List = document.getElementById('ListGV')
                    var li = document.createElement('li')
                    li.setAttribute('data-bs-toggle', 'tooltip');
                    li.setAttribute('data-popup', 'tooltip-custom');
                    li.setAttribute('data-bs-placement', 'top');
                    li.setAttribute('title', data.dn + ' - ' + data.hoTen)
                    li.classList.add('avatar');
                    li.classList.add('avatar-xs');
                    li.classList.add('pull-up');
                    var img = '<img src="' + data.avatar + '" class="rounded-circle" alt="' + data.id + '"/>'
                    li.innerHTML = img;
                    List.appendChild(li);
                }
            })
        }
    }
    $('#modalDongThemNhomDS').click();
}

function TonTaiGV(id) {
    var List = document.getElementById('divImg').getElementsByTagName("img");
    var bool = true;
    if (List[0]) {
        for (var i in List) {
            if (List[i].alt == id) {
                bool = false;
            }
        }
    }
    return bool;
}

function XoaGV() {
    document.getElementById('ListGV').innerHTML = ""
    document.getElementById('locDSGV').style.display = 'none'
}

function TimKiemNhom() {
    var txtSearch = $('#txtSearch').val();
    var mhp = $('#selectMHP').val();
    var ht = $('#selectHT').val();
    var hk = $('#selectHK').val();
    var nh = $('#selectNH').val();
    var date = $('#selectNT').val();
    var daThi = ''
    var daNop = ''
    var gv = []
    if (document.getElementById('inlineRadio1').checked) {
        daThi = '0'
    }
    else if (document.getElementById('inlineRadio2').checked) {
        daThi = '1'
    }
    else { daThi = '2' }
    if (document.getElementById('inlineRadio4').checked) {
        daNop = '0'
    }
    else if (document.getElementById('inlineRadio5').checked) {
        daNop = '1'
    }
    else { daNop = '2' }

    var List = document.getElementById('divImg').getElementsByTagName("img");
    if (List.length > 0) {
        for (var i in List) {
            gv.push(List[i].alt)
        }
    }

    $.ajax({
        url: '/TimKiem/LocNhom',
        type: 'post',
        data: {
            txtSearch: txtSearch,
            mhp: mhp,
            ht: ht,
            hk: hk,
            nh: nh,
            date: date,
            daThi: daThi,
            daNop: daNop,
            gv: JSON.stringify(gv),
        },
        success: function (data) {
            document.getElementById('divTimKiem').innerHTML = data;
            $("#collapseExample").collapse('hide')
        }
    })
}