

// sua duong dan
function ThemDuongDan() {
    str = '<strong>Học phần</strong> / Danh sách học phần'
    document.getElementById('DuongDan').innerHTML = str;
}

//ready
$(document).ready(function () {
    ThemDuongDan()
})