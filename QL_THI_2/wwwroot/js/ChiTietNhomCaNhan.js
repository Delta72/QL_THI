

// Ve do thi
function VeDoThi() {
    var xValues = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    var yValues = [1, 2, 3, 4, 5, 6.5, 7, 8, 9, 10];
    var barColors = ["blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue"];

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
            legend: { display: false },
            title: {
                display: true,
                text: "Bảng phân bố điểm"
            }
        }
    });
}

// Chinh sua
function ChinhSua(u, id) {
    $.ajax({
        url: '/Nhom/ChiTietNhomThiCaNhanJson',
        type: 'post',
        data: {
            id: id,
        },
        success: function (data) {
            //console.log(data);
            if (data.nhom.taiKhoan.id != u) {
                HienLoi()
            }
            else {
                HienChinhSua(data)
            }
        }
    })
}
function HienChinhSua(data) {
    var hthuc = '<select class="form-control form-select" id="selectHinhThuc">'
    for (var i in data.hinhThuc) {
        if (data.nhom.hinhThuc.tenHinhThuc == data.hinhThuc[i]) {
            hthuc += '<option value="' + (parseInt(i)+1) + '" selected>' + data.hinhThuc[i] + '</option>'
        }
        else {
            hthuc += '<option value="' + (parseInt(i)+1) + '">' + data.hinhThuc[i] + '</option>'
        }
    }
    hthuc += '</select>'
    document.getElementById('hinhThuc').innerHTML = hthuc;

    var [d, m, y] = data.nhom.ngayThi.split('/')
    var date = y + '-' + m + '-' + d
    var nThi = '<input type="date" class="form-control" id="textNgayThi" value="' + date + '">'
    document.getElementById('ngayThi').innerHTML = nThi;

    var siSo = '<input type="number" min="0" class="form-control" id="textSiSo">'
    document.getElementById('siSo').innerHTML = siSo;

    var tDu = '<input type="number" min="0" class="form-control" id="textThamDu">'
    document.getElementById('thamDu').innerHTML = tDu;

    var slDe = '<input type="number" min="0" class="form-control" id="textSoDe">'
    document.getElementById('soDe').innerHTML = slDe;

    var slDA = '<input type="number" min="0" class="form-control" id="textSoDA">'
    document.getElementById('soDapAn').innerHTML = slDA;

    var q = 0, w = 0, e = 0, r = 0;
    var zip = ''
    if (data.nhom.zipBaiThi == null) {
        zip += '<button type="button" class="btn btn-xs btn-primary" onclick="TaiLenZip()">Tải lên file .Zip</button>'
    }
    else {
        q = 1;
        zip += '<button type="button" class="btn btn-xs btn-warning" onclick="TaiLenZip()">Tải file khác</button>'
    }
    zip += '<input type="file" id="fileZip" style="display: none" accept=".zip">'
    document.getElementById('zip').innerHTML = zip

    var pdfDe = ''
    if (data.nhom.pdfDe == null) {
        pdfDe += '<button type="button" class="btn btn-xs btn-primary" onclick="TaiLenPDFDe()">Tải lên file .pdf</button>'
    }
    else {
        w = 1
        pdfDe += '<button type="button" class="btn btn-xs btn-warning" onclick="TaiLenPDFDe()">Tải file khác</button>'
    }
    pdfDe += '<input type="file" id="filePDFDe" style="display: none" accept=".pdf">'
    document.getElementById('pdfDe').innerHTML = pdfDe

    var pdfDiem = ''
    if (data.nhom.pdfDiem == null) {
        pdfDiem += '<button type="button" class="btn btn-xs btn-primary" onclick="TaiLenPDFDiem()">Tải lên file .pdf</button>'
    }
    else {
        e = 1
        pdfDiem += '<button type="button" class="btn btn-xs btn-warning" onclick="TaiLenPDFDiem()">Tải file khác</button>'
    }
    pdfDiem += '<input type="file" id="filePDFDiem" style="display: none" accept=".pdf">'
    document.getElementById('pdfDiem').innerHTML = pdfDiem

    var excel = ''
    if (data.nhom.excelDiem == null) {
        excel += '<button type="button" class="btn btn-xs btn-primary" onclick="TaiLenExcel()">Tải lên file .xlsx</button>'
    }
    else {
        r = 1
        excel += '<button type="button" class="btn btn-xs btn-warning" onclick="TaiLenExcel()">Tải file khác</button>'
    }
    excel += '<input type="file" id="fileExcel" style="display: none" accept=".xlsx,.csv">'
    document.getElementById('excel').innerHTML = excel

    var elearning = '<input type="text" class="form-control" id="textElearning" value="'
    if (data.nhom.elearning != null && data.nhom.elearning != "") {
        elearning += data.nhom.elearning
    }
    elearning += '" style="max-width: none" placeholder="Đường dẫn ELearning...">'
    document.getElementById('elearning').innerHTML = elearning;

    HienThongTin(q, w, e, r)

    var btn = '<button onclick="HuyBo(\'' + data.nhom.id + '\')" class="btn btn-secondary">Hủy bỏ</button>'
    btn += '<button data-bs-toggle="modal" data-bs-target="#modalXacNhan" class="btn btn-success">Lưu</button > '
    document.getElementById('divBtn').innerHTML = btn;
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
    frm.append('ngayThi', ngayThi)
    frm.append('hinhThuc.id', hinhThuc)
    frm.append('siSo', siSo)
    frm.append('thamDu', thamDu)
    frm.append('soDe', soDe)
    frm.append('soDapAn', soDapAn)
    if (zip.length > 0) {
        frm.append('fileZip', zip[0])
    }

    $.ajax({
        url: '/Nhom/LuuThongTinNhom',
        type: 'post',
        async: true,
        dataType: 'json',
        processData: false,
        contentType: false,
        data: frm,
        success: function (data) {
            $('#modalDong').click()
            HuyBo(id)
        }
    })
}


// huy bo
function HuyBo(id) {
    $.ajax({
        url: '/Nhom/ChiTietNhomThiCaNhanJson',
        type: 'post',
        data: {
            id: id,
        },
        success: function (data) {
            QuayLai(data);
        }
    })
}
function QuayLai(data) {
    // console.log(data)
    document.getElementById('hinhThuc').innerHTML = data.nhom.hinhThuc.tenHinhThuc;
    document.getElementById('ngayThi').innerHTML = data.nhom.ngayThi;
    document.getElementById('siSo').innerHTML = data.nhom.siSo;
    document.getElementById('thamDu').innerHTML = data.nhom.thamDu;
    document.getElementById('soDe').innerHTML = data.nhom.soDe;
    document.getElementById('soDapAn').innerHTML = data.nhom.soDapAn;
    document.getElementById('elearning').innerHTML = (data.nhom.elearning == null) ? "---" : data.nhom.elearning;
    document.getElementById('zip').innerHTML = (data.nhom.zipBaiThi == null) ? "---" : '<button type="button" class="btn btn-xs btn-primary" onclick="Download(\'' + data.nhom.zipBaiThi + '\')">Tải xuống .Zip</button>';
    document.getElementById('DoThiDiem').innerHTML = (data.nhom.excelDiem == null) ? "<h5>Chưa có thông tin chi tiết điểm</h5>" : '<canvas id="canvasDoThi" style="max-width: 350px"></canvas>';
    if (document.getElementById('canvasDoThi') != null) { VeDoThi() }
    document.getElementById('pdfDe').innerHTML = (data.nhom.pdfDe == null) ? "---" : '<button type="button" class="btn btn-xs btn-primary" onclick="Download(\'' + data.nhom.pdfDe + '\')">Tải xuống .PDF</button>';
    document.getElementById('pdfDiem').innerHTML = (data.nhom.pdfDiem == null) ? "---" : '<button type="button" class="btn btn-xs btn-primary" onclick="Download(\'' + data.nhom.pdfDiem + '\')">Tải xuống .PDF</button>';;
    document.getElementById('excel').innerHTML = (data.nhom.excelDiem == null) ? "---" : '<button type="button" class="btn btn-xs btn-primary" onclick="Download(\'' + data.nhom.excelDiem + '\')">Tải xuống .xlsx</button>';;

    var btn = '<button onclick="ChinhSua(\'' + data.tk + '\',\'' + data.nhom.id + '\')" class="btn btn-primary">Chỉnh sửa</button>'
    document.getElementById('divBtn').innerHTML = btn;
}
// Hien thong tin file upload
function HienThongTin(a, b, c, d) {
    // console.log(a ,b, c, d)

    var x1 = (a == 1) ? "Đã tải lên" : "Trống"
    var x2 = (b == 1) ? "Đã tải lên" : "Trống"
    var x3 = (c == 1) ? "Đã tải lên" : "Trống"
    var x4 = (d == 1) ? "Đã tải lên" : "Trống"

    var tt = ''
    tt += '<div class="demo-inline-spacing mt-3">'
    tt += '<ul class="list-group">'
    tt += '<li class="list-group-item d-flex align-items-left"><i class="fa fa-file-archive-o" style="margin-right: 20px"></i><span class="col-md-6">.Zip bài thi: </span><span class="col-md-3">' + x1 + '</span></li>'
    tt += '<li class="list-group-item d-flex align-items-left"><i class="fa fa-file-pdf-o" style="margin-right: 20px"></i><span class="col-md-6">.PDF đề thi: </span><span class="col-md-3">' + x2 + '</span></li>'
    tt += '<li class="list-group-item d-flex align-items-left"><i class="fa fa-file-pdf-o" style="margin-right: 20px"></i><span class="col-md-6">.PDF điểm thi: </span><span class="col-md-3">' + x3 + '</span></li>'
    tt += '<li class="list-group-item d-flex align-items-left"><i class="fa fa-file-excel-o" style="margin-right: 20px"></i><span class="col-md-6">.xlsx điểm thi: </span><span class="col-md-3">' + x4 + '</span></li>'
    tt += '</ul></div>'

    if (document.getElementById('DoThiDiem') != null) {
        document.getElementById('DoThiDiem').innerHTML = tt;
    }
}


// Ready
$(document).ready(function () {
    if (document.getElementById('canvasDoThi') != null) {
        VeDoThi()
    }

    
})