var OpenOfficeType = function () {
    $('.modal-dialog').css('max-width', '65%');
    var options = {
        "backdrop": "static",
        keyboard: true
    };
    $.ajax({
        type: "GET",
        url: "/OfficeTypes/Index",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            $('#myModalContent').empty();
            $('#myModalContent').html(data);
            $('#myModal').modal(options);
            $('#headerText').text("Office Types");
            $('#myModal').modal('show');
        },
        error: function () {
            alert("Content load failed.");
        }
    });
    return false;
};

var CreateOfficeType = function () {
    $('.modal-dialog').css('max-width', '40%');
    var options = {
        "backdrop": "static",
        keyboard: true
    };
    $.ajax({
        type: "GET",
        url: "/OfficeTypes/Create",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            $('#myModalContent').empty();
            $('#myModalContent').html(data);
            $('#myModal').modal(options);
            $('#headerText').text("Office Type");
            $('#myModal').modal('show');
        },
        error: function () {
            alert("Content load failed.");
        }
    });
    return false;
};

var saveOfficeType = function () {
    ShowModalLoader();
    var data = $('#formSaveOfficeType').serialize();
    $.ajax({
        type: 'GET',
        cache: false,
        url: "/OfficeTypes/CreateOfficeType",
        data: data,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.StatusCode == 201) {
                OpenOfficeType();
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
            alert("Content load failed.");
        }
    });
    return false;
};

var DetailOfficeType = function (id) {
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/OfficeTypes/Details",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 204) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                $('.modal-dialog').css('max-width', '45%');               
                $('#myModalContent').empty();
                $('#myModalContent').html(data);
                $('#headerText').html("Office Type - " + "<span style='color: red;'>" + $("#Header_Text").val() + "</span>");
            }
            HideModalLoader();
        },
        error: function () {
            alert("Content load failed.");
        }
    });
    return false;
};

var EditOfficeType = function (id) {
    $('.modal-dialog').css('max-width', '40%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/OfficeTypes/Edit",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 204) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                $('#headerText').text("Edit Office Type");
                $('#myModalContent').empty();
                $('#myModalContent').html(data);
            }
            HideModalLoader();
        },
        error: function () {
            alert("Content load failed.");
        }
    });
    return false;
};

var updateOfficeType = function () {
    ShowModalLoader();
    var data = $('#formEditOfficeType').serialize();
    $.ajax({
        type: 'GET',
        cache: false,
        url: "/OfficeTypes/UpdateOfficeTypes",
        data: data,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.StatusCode == 201) {
                OpenOfficeType();
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
            alert("Content load failed.");
        }
    });
    return false;
};

var DeleteOfficeType = function (id) {
    $('.modal-dialog').css('max-width', '40%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/OfficeTypes/Delete",
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
            alert("Content load failed.");
        }
    });
    return false;
};

var DeleteConfirmedOfficeType = function (officeTypeID) {
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': officeTypeID },
        url: "/OfficeTypes/DeleteConfirmed",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 405) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                OpenOfficeType();
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            HideModalLoader();
        },
        error: function () {
            alert("Content load failed.");
        }
    });
    return false;
};