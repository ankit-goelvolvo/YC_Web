
(function ($) {
    "use strict";

    
    /*==================================================================
    [ Validate ]*/
    var input = $('.validate-input .input100');
    var erType = '';
    $('.validate-form').on('submit',function(){
        var check = true;
        for(var i=0; i<input.length; i++) {
            if(validate(input[i]) == false){
                showValidate(input[i], erType);
                check=false;
            }
        }
        if (check == true) {
            Login();
        }
        //return check;
    });


    $('.validate-form .input100').each(function(){
        $(this).focus(function(){
           hideValidate(this);
        });
    });

    function validate(input) {
        var emailReg = /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{1,5}|[0-9]{1,3})(\]?)$/;
        var phoneRegex = /[0-9 -()+]+$/;
        var letterNumber = /((^[0-9]+[a-z]+)|(^[0-9]+[A-Z]+)|(^[A-Z]+[0-9]+)|(^[a-z]+[0-9]+))+$/i;
        var type = '';
        if (emailReg.test($(input).val().trim()) || $(input).val().trim().match(emailReg) == null) {
            type = 'email';
        }
        if (phoneRegex.test($(input).val().trim())) {
            type = 'mobile';
        }
        if ($(input).attr('type') == 'password') {
            type = 'password';
        }
        if (type == 'email') {
            if($(input).val().trim().match(/^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{1,5}|[0-9]{1,3})(\]?)$/) == null) {
                erType = "email";
                return false;
            }
        }
        else if (type == 'mobile') {
            if ($(input).val().trim().length != 10) {
                $('#mobileError').text("Mobile should be of 10 digits");
                $('#mobileError').show();
                $('#mobileError').delay(5000).fadeOut();
                erType = "mobile";
                return false;
            }
        }
        else if (type == 'password') {
            if ($(input).val().trim() == '') {
                erType = "password";
                return false;
            }
            else if (letterNumber.test($(input).val()) == false) {
                $('#passwordError').text("Wrong password. Try again or click on Forgot Password to reset it.");
                $('#passwordError').show();
                $('#passwordError').delay(5000).fadeOut();
                return false;
            }
        }
        else {
            if($(input).val().trim() == ''){
                return false;
            }
        }
    }

    function showValidate(input,type) {
        var thisAlert = $(input).parent();
        if (type == "email" || type == "password") {
            $(thisAlert).addClass('alert-validate');
        }        
    }

    function hideValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).removeClass('alert-validate');
    }
    
    $('#modal-loader').removeClass('visible');
    
})(jQuery);

var ShowModalLoader = function () {
    $('#modal-loader').addClass('visible');
};

var HideModalLoader = function () {
    $('#modal-loader').removeClass('visible');
};

var Login = function () {
    ShowModalLoader();
    var data = $('#formLogin').serialize();
    $.ajax({
        type: 'GET',
        cache: false,
        url: "/Login/Login",
        data: data,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.StatusCode == 201) {
                window.location = "/Home/Index";
            }
            else if (data.StatusCode == 404){
                $('#errorText').text(data.StatusMessage);
                $('#errorText').show();
                $('#errorText').delay(5000).fadeOut();
            }
            HideModalLoader();
        },
        error: function () {
            alert("Content load failed.");
        }
    });
};