
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