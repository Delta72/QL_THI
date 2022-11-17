document.getElementById('_showN').innerHTML = document.getElementById('_mainN').innerHTML;

// Ve do thi
function VeDoThi(data) {
    var xValues = [];
    var yValues = [];
    var barColors = [];

    for (var i in data.chiTietDiem) {
        xValues.push(data.chiTietDiem[i])
        barColors.push("#007bff")
    }
    for (var i in data.soLuong) {
        yValues.push(data.soLuong[i])
    }

    new Chart("canvasDoThi", {
        type: "bar",
        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: barColors,
                data: yValues
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        min: 0,
                        stepSize: 1
                    }
                }]
            },
            legend: { display: false },
            title: {
                display: true,
                text: "Bảng phân bố điểm"
            }
        }
    });
}

// Chinh sua
function ChinhSua(id, tk) {
    $.ajax({
        url: '/Nhom/ChiTietNhomThiCaNhanJson',
        type: 'post',
        data: {
            id: id,
        },
        success: function (data) {
            var d1 = new Date()
            var d2 = new Date()
            var han = []
            if (data.nhom.hanNop != "---") {
                han = data.nhom.hanNop.split('/')
                d2 = new Date(han[2] + '-' + han[1] + '-' + han[0])
            }

            if (d1 >= d2 && data.nhom.hanNop != "---") {
                HienLoi("Đã quá thời hạn chỉnh sửa!")
            }
            else if (tk != data.nhom.taiKhoan.id) {
                HienLoi("Bạn không có quyền chỉnh sửa!")
            }
            else {
                HienChinhSua(data)
                document.getElementById('_showN').innerHTML = document.getElementById('_editN').innerHTML;

                $('#fileZip').change(function () {
                    document.getElementById('nameZip').innerHTML = $(this).val().split(/(\\|\/)/g).pop()
                })
                $('#filePDFDe').change(function () {
                    document.getElementById('nameDe').innerHTML = $(this).val().split(/(\\|\/)/g).pop()
                })
                $('#filePDFDiem').change(function () {
                    document.getElementById('nameDiem').innerHTML = $(this).val().split(/(\\|\/)/g).pop()
                })
                $('#fileExcel').change(function () {
                    document.getElementById('nameExcel').innerHTML = $(this).val().split(/(\\|\/)/g).pop()
                })
            }
        }
    })
}
function HienChinhSua(data) {
    var select = document.getElementById('selectHinhThuc')
    for (var i in data.hinhThuc) {
        if (data.nhom.hinhThuc.id == parseInt(i) + 1) {
            select.innerHTML += '<option value="' + (parseInt(i) + 1) + '" selected>' + data.hinhThuc[i] + '</option>'
        }
        else {
            select.innerHTML += '<option value="' + (parseInt(i) + 1) + '">' + data.hinhThuc[i] + '</option>'
        }
    }
}
function TaiLenZip() { $('#fileZip').click(); }
function TaiLenPDFDe() { $('#filePDFDe').click(); }
function TaiLenPDFDiem() { $('#filePDFDiem').click(); }
function TaiLenExcel() { $('#fileExcel').click(); }


// Luu
function Luu(id) {
    var ngayThi = document.getElementById('textNgayThi').value;
    var hinhThuc = document.getElementById('selectHinhThuc').value;
    var siSo = document.getElementById('textSiSo').value;
    var thamDu = document.getElementById('textThamDu').value;
    var soDe = document.getElementById('textSoDe').value;
    var soDapAn = document.getElementById('textSoDA').value;

    var zip = document.getElementById('fileZip').files;
    var pdfDe = document.getElementById('filePDFDe').files;
    var pdfDiem = document.getElementById('filePDFDiem').files;
    var excel = document.getElementById('fileExcel').files;
    var elearning = document.getElementById('textElearning').value;

    var frm = new FormData();
    frm.append('id', id)
    frm.append('ngayThi', ngayThi)
    frm.append('hinhThuc.id', hinhThuc)
    frm.append('siSo', siSo)
    frm.append('thamDu', thamDu)
    frm.append('soDe', soDe)
    frm.append('soDapAn', soDapAn)
    if (zip.length > 0) frm.append('fileZip', zip[0])
    if (pdfDe.length > 0) frm.append('filePDFDe', pdfDe[0])
    if (pdfDiem.length > 0) frm.append('filePDFDiem', pdfDiem[0])
    if (excel.length > 0) frm.append('fileExcel', excel[0])
    frm.append('elearning', elearning)

    $.ajax({
        url: '/Nhom/LuuThongTinNhom',
        type: 'post',
        async: true,
        dataType: 'json',
        processData: false,
        contentType: false,
        data: frm,
        beforeSend: function () {
            LoadingButton('btnXacNhanChinhSuaNhom');
        },
        success: function (data) {
            if (data == "error") {
                HienLoi("Có lỗi xảy ra, vui lòng thử lại")
            }
            else if (data == "excel") {
                alert("Tập tin excel không hợp lệ! Dữ liệu không thay đổi")
                window.location.reload()
            }
            else {
                window.location.reload()
            }
        }
    })
}

// huy bo
function HuyBo(id) {
    document.getElementById('_showN').innerHTML = document.getElementById('_mainN').innerHTML;
}

// Ready
$(document).ready(function () {

})