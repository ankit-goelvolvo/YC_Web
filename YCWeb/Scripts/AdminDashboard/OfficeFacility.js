var OpenOfficeFacility = function () {
    $('.modal-dialog').css('max-width', '55%');
    ShowModalLoader();
    var options = {
        "backdrop": "static",
        keyboard: true
    };
    $.ajax({
        type: "GET",
        url: "/OfficeFacilities/Index",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            $('#myModalContent').empty();
            $('#myModalContent').html(data);
            $('#myModal').modal(options);
            $('#headerText').text("Office Facilities");
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

var CreateOfficeFacility = function () {
    $('.modal-dialog').css('max-width', '40%');
    ShowModalLoader();
    var options = {
        "backdrop": "static",
        keyboard: true
    };
    $.ajax({
        type: "GET",
        url: "/OfficeFacilities/Create",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            $('#myModalContent').empty();
            $('#myModalContent').html(data);
            $('#myModal').modal(options);
            $('#headerText').text("Office Facility");
            $('#myModal').modal('show');
            $("#txtPhone").inputFilter(function (value) {
                return /^-?\d*[.,]?\d*$/.test(value);
            });
            HideModalLoader();
        },
        error: function () {
            HideModalLoader();
            alert("Content load failed.");
        }
    });
    return false;
};

var saveOfficeFacility = function () {
    ShowModalLoader();
    var data = $('#formSaveOfficeFacility').serialize();
    $.ajax({
        type: 'GET',
        cache: false,
        url: "/OfficeFacilities/CreateOfficeFacility",
        data: data,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.StatusCode == 201) {
                OpenOfficeFacility();
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

var DetailOfficeFacility = function (id) {
    $('.modal-dialog').css('max-width', '40%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/OfficeFacilities/Details",
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
                $('#headerText').html("Office Facility - " + "<span style='color: red;'>" + $("#Header_Text").val() + "</span>");
            }
            HideModalLoader();
        },
        error: function () {
            alert("Content load failed.");
        }
    });
    return false;
};

var EditOfficeFacility = function (id) {
    $('.modal-dialog').css('max-width', '40%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/OfficeFacilities/Edit",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 204) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                $('#headerText').text("Edit Office Facility");
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

var updateOfficeFacility = function () {
    ShowModalLoader();
    var data = $('#formEditOfficeFacility').serialize();
    $.ajax({
        type: 'GET',
        cache: false,
        url: "/OfficeFacilities/UpdateOfficeFacility",
        data: data,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.StatusCode == 201) {
                OpenOfficeFacility();
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

var DeleteOfficeFacility = function (id) {
    $('.modal-dialog').css('max-width', '40%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/OfficeFacilities/Delete",
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

var DeleteConfirmedOfficeFaclility = function (officeFaclilityID) {
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': officeFaclilityID },
        url: "/OfficeFacilities/DeleteConfirmed",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 405) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                OpenOfficeFacility();
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