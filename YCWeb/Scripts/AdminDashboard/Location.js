var OpenLocation = function () {
    $('.modal-dialog').css('max-width', '65%');
    ShowLoader();
    var options = {
        "backdrop": "static",
        keyboard: true
    };
    $.ajax({
        type: "GET",
        url: "/Locations/Index",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            $('#myModalContent').empty();
            $('#myModalContent').html(data);
            $('#myModal').modal(options);
            $('#headerText').text("Locations");
            $('#myModal').modal('show');
            HideLoader();
        },
        error: function () {
            alert("Content load failed.");
        }
    });
    return false;
};

var CreateLocation = function () {
    $('.modal-dialog').css('max-width', '40%');
    ShowModalLoader();
    var options = {
        "backdrop": "static",
        keyboard: true
    };
    $.ajax({
        type: "GET",
        url: "/Locations/Create",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            $('#myModalContent').empty();
            $('#myModalContent').html(data);
            $('#myModal').modal(options);
            $('#headerText').text("Location");
            $('#myModal').modal('show');
            $("#txtlongitude").inputFilter(function (value) {
                return /^-?\d*[.,]?\d*$/.test(value);
            });
            $("#txtlatitude").inputFilter(function (value) {
                return /^-?\d*[.,]?\d*$/.test(value);
            });
            HideModalLoader();
        },
        error: function () {
            alert("Content load failed.");
        }
    });
    return false;
};

var saveLocation = function () {
    ShowModalLoader();
    var data = $('#formSaveLocation').serialize();
    $.ajax({
        type: 'GET',
        cache: false,
        url: "/Locations/CreateLocation",
        data: data,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.StatusCode == 201) {
                OpenLocation();
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

var DetailLocation = function (id) {
    $('.modal-dialog').css('max-width', '40%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/Locations/Details",
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
                $('#headerText').html("Location - " + "<span style='color: red;'>" + $("#Header_Text").val() + "</span>");
            }
            HideModalLoader();
        },
        error: function () {
            alert("Content load failed.");
        }
    });
    return false;
};

var EditLocation = function (id) {
    $('.modal-dialog').css('max-width', '40%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/Locations/Edit",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 204) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                $('#headerText').text("Edit Location");
                $('#myModalContent').empty();
                $('#myModalContent').html(data);
                $("#txtlongitude").inputFilter(function (value) {
                    return /^-?\d*[.,]?\d*$/.test(value);
                });
                $("#txtlatitude").inputFilter(function (value) {
                    return /^-?\d*[.,]?\d*$/.test(value);
                });
            }
            HideModalLoader();
        },
        error: function () {
            alert("Content load failed.");
        }
    });
    return false;
};

var updateLocation = function () {
    ShowModalLoader();
    var data = $('#formEditLocation').serialize();
    $.ajax({
        type: 'GET',
        cache: false,
        url: "/Locations/UpdateLocation",
        data: data,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.StatusCode == 201) {
                OpenLocation();
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

var DeleteLocation = function (id) {
    $('.modal-dialog').css('max-width', '40%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/Locations/Delete",
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

var DeleteConfirmed = function (locationID) {
    $('.modal-dialog').css('max-width', '90%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': locationID },
        url: "/Locations/DeleteConfirmed",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 405) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                OpenLocation();
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