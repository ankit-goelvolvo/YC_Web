
(function ($) {

    "use strict";
    
    $.fn.inputFilter = function (inputFilter) {
        return this.on("input keydown keyup mousedown mouseup select contextmenu drop", function () {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            }
        });
    };

    $("#txtMobile").inputFilter(function (value) {
        return /^-?\d*[.,]?\d*$/.test(value);
    });

    $("#txtPassword").inputFilter(function (value) {
        return /^[a-zA-Z0-9\s]*$/.test(value);
    });
})(jQuery);