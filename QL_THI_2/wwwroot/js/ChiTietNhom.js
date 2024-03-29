﻿

// ve do thi
function VeDoThi(data) {
    var xValues = [];
    var yValues = [];
    var barColors = [];
    // console.log(data)
    for (var i in data.chiTietDiem) {
        xValues.push(data.chiTietDiem[i])
        barColors.push("#007bff")
    }
    for (var i in data.soLuong) {
        yValues.push(data.soLuong[i])
    }

    new Chart("myChart", {
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
                    },
                    scaleLabel: {
                        display: true,
                        labelString: 'Số lượng'
                    }
                }],
                xAxes: [{
                    scaleLabel: {
                        display: true,
                        labelString: 'Điểm'
                    }
                }],
            },
            legend: { display: false },
            title: {
                display: true,
                text: "Bảng phân bố điểm"
            }
        }
    });
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

function HienLichSu(stt, idN) {
    $.ajax({
        url: '/Nhom/HienLichSuNhom',
        type: 'post',
        data: {
            stt: stt,
            idN: idN,
        },
        success: function (data) {
            // console.log(data)
            document.getElementById('divLichSuChinhSua').innerHTML = data;
        }
    })
}