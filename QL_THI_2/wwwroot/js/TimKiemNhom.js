
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
            // console.log(data)
            // hien ma hoc phan
            var selectMHP = document.getElementById('selectMHP')
            for (var i in data.mhp) {
                var opt = document.createElement('option')
                opt.value = data.mhp[i].id
                opt.innerHTML = data.mhp[i].id + ' - ' + data.mhp[i].tenHocPhan
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

function TimKiemNhom() {
    var txtSearch = $('#txtSearch').val();
    var mhp = $('#selectMHP').val();
    var ht = $('#selectHT').val();
    var hk = $('#selectNH').val();

}