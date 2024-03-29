﻿// active page
function Active(menu, li) {
    for (var i = 1; i <= 3; i++) {
        var id = 'menu' + i;
        var m1 = document.getElementById(id)
        if (m1 != null) {
            m1.classList.remove("active")
            m1.classList.remove("open")
        }
    }
    var m = document.getElementById('menu' + menu)
    if (m != null) {
        m.classList.add("active")
        m.classList.add("open")
    }
    for (var i = 1; i <= 5; i++) {
        var id = 'li' + i;
        var l1 = document.getElementById(id)
        if (l1 != null) {
            document.getElementById(id).classList.remove("active")
        }
    }
    var l = document.getElementById('li' + li)
    if (l != null) {
        l.classList.add("active")
    }
}

// sort table
function sortTable(n, id) {
    var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;
    table = document.getElementById(id);
    switching = true;
    // Set the sorting direction to ascending:
    dir = "asc";
    /* Make a loop that will continue until
    no switching has been done: */
    while (switching) {
        // Start by saying: no switching is done:
        switching = false;
        rows = table.rows;
        /* Loop through all table rows (except the
        first, which contains table headers): */
        for (i = 1; i < (rows.length - 1); i++) {
            // Start by saying there should be no switching:
            shouldSwitch = false;
            /* Get the two elements you want to compare,
            one from current row and one from the next: */
            x = rows[i].getElementsByTagName("TD")[n];
            y = rows[i + 1].getElementsByTagName("TD")[n];
            /* Check if the two rows should switch place,
            based on the direction, asc or desc: */
            if (dir == "asc") {
                if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
                    // If so, mark as a switch and break the loop:
                    shouldSwitch = true;
                    break;
                }
            } else if (dir == "desc") {
                if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
                    // If so, mark as a switch and break the loop:
                    shouldSwitch = true;
                    break;
                }
            }
        }
        if (shouldSwitch) {
            /* If a switch has been marked, make the switch
            and mark that a switch has been done: */
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
            // Each time a switch is done, increase this count by 1:
            switchcount++;
        } else {
            /* If no switching has been done AND the direction is "asc",
            set the direction to "desc" and run the while loop again. */
            if (switchcount == 0 && dir == "asc") {
                dir = "desc";
                switching = true;
            }
        }
    }
}

// Hien loi
function HienLoi(str) {
    if (str == null) {
        document.getElementById('buttonError').click()
    }
    else {
        document.getElementById('buttonError').click()
        document.getElementById('noiDungError').innerHTML = str
    }
}

// Download
function Download(link) {
    window.location.href = '/api/Download?link=' + String(link).replace(/\\/g, "%5C");
}

$("[contenteditable]").show(function () {
        var element = $(this);
        if (!element.text().trim().length) {
            element.empty();
        }
});

function LoadingButton(id) {
    var str = '<div class="spinner-border text-light" style="height: 15px;width: 15px"><span class="visually-hidden"></span></div>'
    document.getElementById(id).innerHTML = str;
}

$(document).ready(function () {

})
