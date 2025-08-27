(function ($) {
    // تابع مبدل انگلیسی به فارسی (بدون تغییر)
    function toPersianNumber(str) {
        if (!str) return str;
        var persianDigits = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];
        return str.toString().replace(/\d/g, function (digit) {
            return persianDigits[digit];
        });
    }

    /**
     * این تابع تمام محتوای استاتیک و مقادیر اولیه فیلدها را در کل صفحه تبدیل می‌کند.
     */
    function convertAllStaticNumbers() {
        // ۱. تبدیل تمام گره‌های متنی (محتوای تگ‌های p, span, div, h1 و ...)
        $("body *:not(script):not(style):not(input):not(textarea)").contents().each(function () {
            if (this.nodeType === 3) { // فقط گره‌های متنی
                var text = this.nodeValue;
                var newText = toPersianNumber(text);
                if (text !== newText) {
                    this.nodeValue = newText;
                }
            }
        });

        // ۲. تبدیل مقادیر attribute های رایج (مانند placeholder)
        $("[title], [placeholder], [alt]").each(function () {
            var $this = $(this);
            ["title", "placeholder", "alt"].forEach(function (attr) {
                var oldVal = $this.attr(attr);
                if (oldVal) {
                    var newVal = toPersianNumber(oldVal);
                    if (oldVal !== newVal) {
                        $this.attr(attr, newVal);
                    }
                }
            });
        });

        // ۳. تبدیل مقدار اولیه (value) همه فیلدهای فرم در زمان بارگذاری
        // این بخش به تایپ کاربر کاری ندارد و فقط یک بار اجرا می‌شود.
        $("input[type='text'], input[type='number'], input[type='tel'], textarea").each(function () {
            var $this = $(this);
            var oldVal = $this.val();
            if (oldVal) {
                var newVal = toPersianNumber(oldVal);
                if (oldVal !== newVal) {
                    $this.val(newVal);
                }
            }
        });
    }

    // --- اجرای اسکریپت ---

    $(document).ready(function () {
        // یک بار در زمان بارگذاری، تمام محتوای استاتیک و مقادیر اولیه را تبدیل کن
        convertAllStaticNumbers();

        // برای محتوای Ajax نیز این تابع را اجرا کن
        $(document).ajaxComplete(function () {
            setTimeout(convertAllStaticNumbers, 100);
        });

        //
        // ====>>   تبدیل زنده اعداد هنگام تایپ، فقط برای فیلدهای با کلاس .persian-number   <<====
        //
        $(document).on("input", "input.persian-number, textarea.persian-number", function (e) {
            var cursorPos = this.selectionStart;
            var oldVal = $(this).val();
            var newVal = toPersianNumber(oldVal);

            if (oldVal !== newVal) {
                $(this).val(newVal);
                this.setSelectionRange(cursorPos, cursorPos);
            }
        });
    });

})(jQuery);









//(function ($) {
//    // مبدل انگلیسی به فارسی
//    function toPersianNumber(str) {
//        if (!str) return str;
//        var persianDigits = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];
//        return str.toString().replace(/\d/g, function (digit) {
//            return persianDigits[digit];
//        });
//    }

//    // تبدیل متن در کل سایت
//    function convertAllNumbers() {
//        // تبدیل متن‌ها
//        $("body *:not(script):not(style):not(noscript)").contents().each(function () {
//            if (this.nodeType === 3) { // فقط text node
//                var text = this.nodeValue;
//                var newText = toPersianNumber(text);
//                if (text !== newText) {
//                    this.nodeValue = newText;
//                }
//            }
//        });

//        // فقط attribute های موردنیاز
//        $("[title], [placeholder], [alt]").each(function () {
//            ["title", "placeholder", "alt"].forEach(attr => {
//                if ($(this).attr(attr)) {
//                    var oldVal = $(this).attr(attr);
//                    var newVal = toPersianNumber(oldVal);
//                    if (oldVal !== newVal) {
//                        $(this).attr(attr, newVal);
//                    }
//                }
//            });
//        });
//    }

//    $(document).ready(function () {
//        // بار اول
//        convertAllNumbers();

//        // بعد از هر Ajax
//        $(document).ajaxComplete(function () {
//            convertAllNumbers();
//        });

//        // برای input و textarea هنگام تایپ
//        $(document).on("input", "input[type='text'], input[type='number'], textarea", function () {
//            var cursorPos = this.selectionStart;
//            var oldVal = $(this).val();
//            var newVal = toPersianNumber(oldVal);
//            if (oldVal !== newVal) {
//                $(this).val(newVal);
//                this.setSelectionRange(cursorPos, cursorPos);
//            }
//        });
//    });
//})(jQuery);
