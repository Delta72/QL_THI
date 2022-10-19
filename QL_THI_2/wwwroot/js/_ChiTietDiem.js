// Luu diem
function LuuDiem() {
    var table = document.getElementById('tableDiemThi')
    var rCount = table.rows.length

    var data = []
    var errorTrong = []
    var errorTrung = []
    var mssv = []
    for (i = 1; i < rCount - 1; i++) {
        var diem = []
        var row = table.rows[i];
        var cCount = row.cells.length;
        for (x = 2; x < cCount - 1; x++) {
            var cell = row.cells[x].innerText;
            diem.push(cell)
        }
        for (x = 1; x < cCount - 1; x++) {
            var cell = row.cells[x].innerText;
            if (cell == "") { errorTrong.push(row.rowIndex) }
        }
        data.push({ "mssv": row.cells[1].innerText, diem })

        if (mssv.includes(row.cells[1].innerText)) {
            errorTrung.push(row.rowIndex)
        }
        mssv.push(row.cells[1].innerText)
    }
    errorTrong = [... new Set(errorTrong)]

    if (errorTrong.length > 0) {
        var str = ''
        for (i = 0; i < errorTrong.length; i++) {
            str += errorTrong[i] + ', '
        }
        str = str.slice(0, -2)
        HienLoi("Dữ liệu trống: Hàng " + str)
    }
    else if (errorTrung.length > 0) {
        var str = ''
        for (i = 0; i < errorTrung.length; i++) {
            str += errorTrung[i] + ', '
        }
        str = str.slice(0, -2)
        HienLoi("Dữ liệu trùng: Hàng " + str)
    }
    else {
        $('#btnDiem').click();
    }
}

function LuuChiTietDiem(id) {
    var table = document.getElementById('tableDiemThi')
    var rCount = table.rows.length

    var data = []
    for (i = 1; i < rCount - 1; i++) {
        var diem = ''
        var row = table.rows[i];
        var cCount = row.cells.length;
        for (x = 2; x < cCount - 1; x++) {
            var cell = row.cells[x].innerText;
            diem += cell + ' '
        }
        for (x = 1; x < cCount - 1; x++) {
            var cell = row.cells[x].innerText;
        }
        diem = diem.slice(0, -1)
        data.push({"id": id, "mssv": row.cells[1].innerText, diem })
    }

    $.ajax({
        url: '/ChiTietDiem/LuuChiTietDiem',
        type: 'post',
        data: {
            str: JSON.stringify(data)
        },
        beforeSend: function () {
            var str = '<div class="spinner-border text-light" style="height: 15px;width: 15px"><span class="visually-hidden"></span></div>'
            document.getElementById('btnXNSuaDiem').innerHTML = str;
        },
        complete: function () {
            document.getElementById('btnXNSuaDiem').innerHTML = "Lưu";
        },
        success: function (data) {
            if (data == "error") {
                HienLoi("Dữ liệu không hợp lệ!");
                ReloadChiTiet();
            }
            else {
                window.location.reload()
            }
        }
    })
    console.log(data)
}

function ThemHangDiem(c, t) {
    var table = document.getElementById('tableDiemThi')

    var trong = false;
    var kt = table.rows[t.rowIndex - 1]
    for (i = 1; i < kt.cells.length - 1; i++) {
        if (kt.cells[i].innerText == "") {
            //console.log(kt.cells[i])
            trong = true;
        }
    }
    if (!trong) {
        var count = table.rows.length;
        var row = table.insertRow(count - 1);

        document.getElementById('btnLuuDiem').style.display = 'unset'
        var cell1 = row.insertCell(0);
        cell1.style.textAlign = "center"
        cell1.className = "count"

        var cell2 = row.insertCell(1);
        cell2.contentEditable = "true"
        cell2.focus()
        cell2.innerHTML = "";

        for (i = 3; i <= parseInt(c) + 2; i++) {
            var cellx = row.insertCell(i - 1);
            cellx.contentEditable = "true"
            cellx.className = 'editDiem'
        }

        cell3 = row.insertCell(parseInt(c) + 2);
        cell3.innerHTML = '<i class="fa fa-times" onclick="XoaHang(this)"></i>'
        cell3.className = "cursorPointer"
        ChiNhapSo()
    }
    else {
        kt.cells[1].focus()
    }
}

function XoaHang(i) {
    var row = i.closest('tr')
    var table = document.getElementById('tableDiemThi')
    table.deleteRow(row.rowIndex)
    document.getElementById('btnLuuDiem').style.display = 'unset'

    if (table.rows.length == 2) {
        document.getElementById('btnLuuDiem').style.display = 'none'
    }
}

function ChiNhapSo() {
    var editDiem = document.getElementsByClassName('editDiem')
    for (var i = 0; i < editDiem.length; i++) {
        editDiem[i].onkeypress = function (e) {
            var x = event.charCode || event.keyCode;
            if (isNaN(String.fromCharCode(e.which)) && x != 46) e.preventDefault();
        }
    }
}
ChiNhapSo()

function ReloadChiTiet() {
    document.getElementById('bodyCard').innerHTML = document.getElementById('bodyCardTemp').innerHTML;
}