var OpenOffices = function () {
    $('.modal-dialog').css('max-width', '65%');
    ShowModalLoader();
    var options = {
        "backdrop": "static",
        keyboard: true
    };
    $.ajax({
        type: "GET",
        url: "/Offices/Index",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            $('#myModalContent').empty();
            $('#myModalContent').html(data);
            $('#myModal').modal(options);
            $('#headerText').text("Offices");
            $('#myModal').modal('show');
            HideModalLoader();
        },
        error: function () {
            HideModalLoader();
            alert("Content load failed.");
        }
    });
    return false;
};

var DetailOffice = function (id) {
    $('.modal-dialog').css('max-width', '40%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/Offices/Details",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 204) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                $('#myModalContent').empty();
                $('#myModalContent').html(data);
                $('#headerText').html("Office - " + "<span style='color: red;'>" + $("#Header_Text").val() + "</span>");
            }
            HideModalLoader();
        },
        error: function () {
            alert("Content load failed.");
        }
    });
    return false;
};

var CreateOffice = function () {
    $('.modal-dialog').css('max-width', '60%');
    ShowModalLoader();
    var options = {
        "backdrop": "static",
        keyboard: true
    };
    $.ajax({
        type: "GET",
        url: "/Offices/Create",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            $('#myModalContent').empty();
            $('#myModalContent').html(data);
            $('#myModal').modal(options);
            $('#headerText').text("Office");
            $('#myModal').modal('show');
            $("#txtPhone").inputFilter(function (value) {
                return /^-?\d*[.,]?\d*$/.test(value);
            });
            InitTimePicker();
            HideModalLoader();
        },
        error: function () {
            HideModalLoader();
            alert("Content load failed.");
        }
    });
    return false;
};

var saveOffice = function () {
    ShowModalLoader();
    var data = $('#formSaveOffice').serialize();
    $.ajax({
        type: 'GET',
        cache: false,
        url: "/Offices/CreateOffice",
        data: data,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.StatusCode == 201) {
                OpenOffices();
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            HideModalLoader();
        },
        error: function () {
            HideModalLoader();
            alert("Content load failed.");
        }
    });
    return false;
};

var EditOffice = function (id) {
    $('.modal-dialog').css('max-width', '60%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/Offices/Edit",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 204) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                $('#headerText').text("Edit Office");
                $('#myModalContent').empty();
                $('#myModalContent').html(data);
            }
            //InitTimePicker();
            HideModalLoader();
        },
        error: function () {
            HideModalLoader();
            alert("Content load failed.");
        }
    });
    return false;
};

var updateOffice = function () {
    ShowModalLoader();
    var data = $('#formEditOffice').serialize();
    $.ajax({
        type: 'GET',
        cache: false,
        url: "/Offices/UpdateOffice",
        data: data,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.StatusCode == 201) {
                OpenOffices();
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
                InitTimePicker();
            }
            HideModalLoader();
        },
        error: function () {
            HideModalLoader();
            alert("Content load failed.");
        }
    });
    return false;
};

var DeleteOffice = function (id) {
    $('.modal-dialog').css('max-width', '40%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/Offices/Delete",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 204) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                $('#headerText').text("Are you sure you want to delete this?");
                $('#myModalContent').empty();
                $('#myModalContent').html(data);
            }
            HideModalLoader();
        },
        error: function () {
            HideModalLoader();
            alert("Content load failed.");
        }
    });
    return false;
};

var DeleteConfirmedOffice = function (officeID) {
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': officeID },
        url: "/Offices/DeleteConfirmed",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 405) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                OpenOffices();
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            HideModalLoader();
        },
        error: function () {
            HideModalLoader();
            alert("Content load failed.");
        }
    });
    return false;
};

var InitTimePicker = function () {
    $('#txtCheckIn').timepicker({
        timeFormat: 'h:mm p',
        interval: 60,
        minTime: '10',
        maxTime: '6:00pm',
        defaultTime: '11',
        startTime: '10:00',
        dynamic: false,
        dropdown: true,
        scrollbar: true
    });
    $('#txtCheckOut').timepicker({
        timeFormat: 'h:mm p',
        interval: 60,
        minTime: '10',
        maxTime: '6:00pm',
        defaultTime: '10',
        startTime: '10:00',
        dynamic: false,
        dropdown: true,
        scrollbar: true
    });

}

var EditTimePicker = function (CheckIn, CheckOut) {
    CheckIn = CheckIn.split(" ")[1].substring(0, 2);
    CheckOut = CheckOut.split(" ")[1].substring(0, 2);
    $('#txtCheckIn').timepicker({
        timeFormat: 'h:mm p',
        interval: 60,
        minTime: '10',
        maxTime: '6:00pm',
        defaultTime: CheckIn,
        startTime: '10:00',
        dynamic: false,
        dropdown: true,
        scrollbar: true
    });
    $('#txtCheckOut').timepicker({
        timeFormat: 'h:mm p',
        interval: 60,
        minTime: '10',
        maxTime: '6:00pm',
        defaultTime: CheckOut,
        startTime: '10:00',
        dynamic: false,
        dropdown: true,
        scrollbar: true
    });
};