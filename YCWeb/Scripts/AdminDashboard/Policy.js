var OpenPolicies = function () {
    $('.modal-dialog').css('max-width', '50%');
    ShowModalLoader();
    var options = {
        "backdrop": "static",
        keyboard: true
    };
    $.ajax({
        type: "GET",
        url: "/Policies/Index",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            $('#myModalContent').empty();
            $('#myModalContent').html(data);
            $('#myModal').modal(options);
            $('#headerText').text("Policies");
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

var DetailPolicy = function (id) {
    $('.modal-dialog').css('max-width', '45%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/Policies/Details",
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
                $('#headerText').html("Policy - " + "<span style='color: red;'>" + $("#Header_Text").val() + "</span>");
            }
            HideModalLoader();
        },
        error: function () {
            alert("Content load failed.");
        }
    });
    return false;
};

var CreatePolicy = function () {
    $('.modal-dialog').css('max-width', '45%');
    ShowModalLoader();
    var options = {
        "backdrop": "static",
        keyboard: true
    };
    $.ajax({
        type: "GET",
        url: "/Policies/Create",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            $('#myModalContent').empty();
            $('#myModalContent').html(data);
            $('#myModal').modal(options);
            $('#headerText').text("Policy");
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

var savePolicy = function () {
    ShowModalLoader();
    var data = $('#formSavePolicy').serialize();
    $.ajax({
        type: 'GET',
        cache: false,
        url: "/Policies/CreatePolicy",
        data: data,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.StatusCode == 201) {
                OpenPolicies();
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

var EditPolicy = function (id) {
    $('.modal-dialog').css('max-width', '40%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/Policies/Edit",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 204) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                $('#headerText').text("Edit Policy");
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

var updatePolicy = function () {
    ShowModalLoader();
    var data = $('#formEditOfficeFacility').serialize();
    $.ajax({
        type: 'GET',
        cache: false,
        url: "/Policies/UpdatePolicy",
        data: data,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.StatusCode == 201) {
                OpenPolicies();
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

var DeletePolicy = function (id) {
    $('.modal-dialog').css('max-width', '40%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/Policies/Delete",
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

var DeleteConfirmedPolicy = function (officeFaclilityID) {
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': officeFaclilityID },
        url: "/Policies/DeleteConfirmed",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 405) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                OpenPolicies();
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