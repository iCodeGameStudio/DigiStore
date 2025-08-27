function toggleSearch() {
    var searchBox = document.getElementById("searchBox");
    searchBox.classList.toggle("active");
    // زمانی که باکس فعال شد، روی input فوکوس می‌کنیم
    if (searchBox.classList.contains("active")) {
        searchBox.querySelector("input").focus();
    }
}

function SearchFunction() {
    var input, filter, table, tr, td, i, j, txtValue;
    input = document.getElementById("searchInput");
    filter = input.value.toUpperCase();
    table = document.getElementById("MyDataTable");
    tr = table.getElementsByTagName("tr");

    // تعداد ستون‌ها را از اولین ردیف جدول بگیرید (معمولاً هدر جدول)
    var columnCount = 0;
    if (tr.length > 0) {
        columnCount = tr[0].getElementsByTagName("th").length;
        if (columnCount === 0) {
            // اگر th نبود، احتمالاً از td استفاده شده در هدر
            columnCount = tr[0].getElementsByTagName("td").length;
        }
    }

    for (i = 1; i < tr.length; i++) { // i=1 چون ردیف اول هدر است
        let row = tr[i];
        let showRow = false;

        for (j = 0; j < columnCount; j++) {
            td = row.getElementsByTagName("td")[j];
            if (td) {
                txtValue = td.textContent || td.innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    showRow = true;
                    break;
                }
            }
        }

        row.style.display = showRow ? "" : "none";
    }
}

function SearchFunctionForUL() {
    var input, filter, ul, li, label, i, txtValue;
    input = document.getElementById("searchInput");
    // تبدیل ورودی به حروف کوچک برای جستجوی بهتر
    filter = input.value.trim().toLowerCase();
    ul = document.getElementById("myUL");
    li = ul.getElementsByTagName("li");
    for (i = 0; i < li.length; i++) {
        label = li[i].getElementsByTagName("label")[0];
        if (label) {
            // تبدیل متن برچسب به حروف کوچک
            txtValue = label.textContent.trim().toLowerCase();
            // استفاده از `includes` برای جستجوی دقیق‌تر
            if (txtValue.includes(filter)) {
                li[i].style.display = "";
            } else {
                li[i].style.display = "none";
            }
        }
    }
}