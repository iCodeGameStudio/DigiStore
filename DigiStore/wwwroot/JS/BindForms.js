function BindFormLargeModal() {
    $('#MyForm').submit(function (event) {
        event.preventDefault(); // جلوگیری از ارسال عادی فرم

        var form = $(this)[0]; // فرم اصلی
        var formData = new FormData(form); // درست کردن FormData برای ارسال فایل‌ها

        $.ajax({
            url: $(form).attr('action'),
            type: $(form).attr('method'),
            data: formData,
            contentType: false,   // خیلی مهم
            processData: false,   // خیلی مهم
            success: function (response) {
                if (response.success) {
                    $('#LargeModel').modal('hide');
                    window.location.href = response.redirectUrl;
                } else {
                    $('#LargeModelBody').html(response);
                    BindFormLargeModal();
                }
            },
            error: function (xhr, status, error) {
                console.error("An error occurred: " + error);
            }
        });
    });
}

function BindFormMiniModal() {
    $('#MyForm').submit(function (event) {
        event.preventDefault(); // جلوگیری از ارسال عادی فرم

        var form = $(this)[0]; // فرم اصلی
        var formData = new FormData(form); // درست کردن FormData برای ارسال فایل‌ها

        $.ajax({
            url: $(form).attr('action'),
            type: $(form).attr('method'),
            data: formData,
            contentType: false,   // خیلی مهم
            processData: false,   // خیلی مهم
            success: function (response) {
                if (response.success) {
                    $('#MiniModel').modal('hide');
                    window.location.href = response.redirectUrl;
                } else {
                    $('#MiniModelBody').html(response);
                    BindFormMiniModal();
                }
            },
            error: function (xhr, status, error) {
                console.error("An error occurred: " + error);
            }
        });
    });
}

$('#LargeModel').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);  // دکمه‌ای که مودال رو باز کرده
    var title = button.data('title');     // گرفتن مقدار data-title
    var modal = $(this);

    modal.find('.modal-title').text(title); // ست کردن عنوان مودال
});
$('#MiniModel').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);  // دکمه‌ای که مودال رو باز کرده
    var title = button.data('title');     // گرفتن مقدار data-title
    var modal = $(this);

    modal.find('.modal-title').text(title); // ست کردن عنوان مودال
});
