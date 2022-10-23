
function HienXoaFile(type) {
    document.getElementById('fileType').innerHTML = type;
}

function XoaFile(id) {
    var type = document.getElementById('fileType').innerHTML;
    // console.log(type)
    $.ajax({
        url: '/Nhom/XoaFile',
        type: 'post',
        data: {
            id: id,
            type: type,
        },
        beforeSend: function () {
            LoadingButton('btnXoaFile')
        },
        success: function (data) {
            if (data == "error") {
                HienLoi();
            }
            else {
                window.location.reload()
            }
        }
    })
}