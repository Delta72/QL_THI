

// chi tiet nhom
function ChiTietNhom(id) {
    window.location.href = '/Nhom/ChiTietNhom?id=' + id
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