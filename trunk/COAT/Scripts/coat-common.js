/*
*   Add JQuery Extend Method "popWindow". Same class href will pop in same window;
*/
(function ($, document) {
    var popWindows = {};
    var windowStyle = "width=850,height=600,scrollbars=yes, toolbar=no, menubar=no, location=no, status=no";

    $.fn.popWindow = function () {
        $(this).click(function (e) {
            var url = $(this).attr("href");
            var className = $(this).attr("class");
            if (!popWindows[className]) {
                popWindows[className] = window.open(url, className, windowStyle);
            } else if (popWindows[className].closed) {
                popWindows[className] = null;
                popWindows[className] = window.open(url, className, windowStyle);
            } else {
                popWindows[className].open(url, className, windowStyle);
            }

            e.preventDefault();
            return false;
        });
    };
})(jQuery, document);