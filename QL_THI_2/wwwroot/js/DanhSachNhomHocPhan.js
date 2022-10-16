

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

// sua duong dan
function ThemDuongDan() {
    str = '<strong>Học phần</strong> / Danh sách học phần / Nhóm học phần'
    document.getElementById('DuongDan').innerHTML = str;
}

//ready
$(document).ready(function () {
    ThemDuongDan()
})