
Active(3, 7)

// Ve do thi
function VeDoThi(arrDiem, arrTP) {
    var xValues = [];
    var yValues = [];
    var barColors = [];

    for (var x in arrDiem) {
        yValues.push(arrDiem[x])
    }
    for (var x in arrTP) {
        xValues.push(arrTP[x])
    }
    for (var x = 0; x < (parseInt(arrTP.length) - 1); x++) {
        barColors.push("#007bff")
    }
    barColors.push("#71dd37")

    $('#tdCanvas').html('')
    $('#tdCanvas').html('<canvas id="myModalCanvas"></canvas>')


    new Chart('myModalCanvas', {
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
                    },
                }],
                xAxes: [{
                    ticks: {
                        min: 0,
                    },
                }]
            },
            legend: { display: false },
        }
    });
}

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