

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
    var hthuc = '<select class="form-control form-select" id="ipHThuc">'
    for (var i in data.hinhThuc) {
        if (data.nhom.hinhThuc.tenHinhThuc == data.hinhThuc[i]) {
            hthuc += '<option value="' + i + '" selected>' + data.hinhThuc[i] + '</option>'
        }
        else {
            hthuc += '<option value="' + i + '">' + data.hinhThuc[i] + '</option>'
        }
    }
    hthuc += '</select>'
    document.getElementById('hinhThuc').innerHTML = hthuc;

    var [d, m, y] = data.nhom.ngayThi.split('/')
    var date = y + '-' + m + '-' + d
    var nThi = '<input type="date" class="form-control" id="ipNThi" value="' + date + '">'
    document.getElementById('ngayThi').innerHTML = nThi;

    var siSo = '<input type="number" min="0" class="form-control" id="ipSSo">'
    document.getElementById('siSo').innerHTML = siSo;

    var tDu = '<input type="number" min="0" class="form-control" id="ipTDu">'
    document.getElementById('thamDu').innerHTML = tDu;

    var slDe = '<input type="number" min="0" class="form-control" id="ipSLDe">'
    document.getElementById('soDe').innerHTML = slDe;

    var slDA = '<input type="number" min="0" class="form-control" id="ipSLDA">'
    document.getElementById('soDapAn').innerHTML = slDA;

    var zip = ''
    if (data.nhom.zipBaiThi == null) {
        zip += '<button type="button" class="btn btn-xs btn-primary" onclick="TaiLenZip()">Tải lên .Zip</button>'
    }
    else {
        zip += '<button type="button" class="btn btn-xs btn-warning" onclick="TaiLenZip()">Tải tệp thay thế</button>'
    }
    zip += '<input type="file" id="zipFile" style="display: none">'
    document.getElementById('zip').innerHTML = zip

    var pdfDe = ''
    if (data.nhom.pdfDe == null) {
        pdfDe += '<button type="button" class="btn btn-xs btn-primary" onclick="TaiLenPDFDe()">Tải lên .pdf</button>'
    }
    else {
        pdfDe += '<button type="button" class="btn btn-xs btn-warning" onclick="TaiLenPDFDe()">Tải tệp thay thế</button>'
    }
    pdfDe += '<input type="file" id="pdfDeFile" style="display: none">'
    document.getElementById('pdfDe').innerHTML = pdfDe
}
function TaiLenZip() { $('#zipFile').click(); }
function TaiLenPDFDe() { $('#pdfDeFile').click(); }

// Ready
$(document).ready(function () {
    if (document.getElementById('canvasDoThi') != null) {
        VeDoThi()
    }
})