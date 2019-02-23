var OpenRoomTypeOption = function () {
    $('.modal-dialog').css('max-width', '65%');
    var options = {
        "backdrop": "static",
        keyboard: true
    };
    $('#myModalContent').empty();
    $.ajax({
        type: "GET",
        url: "/RoomTypeOptions/Index",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            
            $('#myModalContent').html(data);
            $('#myModal').modal(options);
            $('#headerText').text("Room Type Options");
            $('#myModal').modal('show');
        },
        error: function () {
            alert("Content load failed.");
        }
    });
    return false;
};

var CreateRoomTypeOption = function () {
    $('.modal-dialog').css('max-width', '45%');
    var options = {
        "backdrop": "static",
        keyboard: true
    };
    var url = "@Url.Action('Create','RoomTypeOptions')";
    $('#myModalContent').empty();
    $.post("/RoomTypeOptions/Create", function (data) {
        
        $('#myModalContent').html(data);
        $('#myModal').modal(options);
        $('#headerText').text("Room Type Option");
        $('#myModal').modal('show');
        //$("#SomeDivToShowTheResult").html(res);
    });
    //$.ajax({
    //    type: "GET",
    //    url: "/RoomTypeOptions/Create",
    //    contentType: "application/json; charset=utf-8",
    //    datatype: "json",
    //    success: function (data) {
    //        $('#myModalContent').empty();
    //        $('#myModalContent').html(data);
    //        $('#myModal').modal(options);
    //        $('#headerText').text("Room Type Option");
    //        $('#myModal').modal('show');
    //    },
    //    error: function () {
    //        alert("Content load failed.");
    //    }
    //});
    return false;
};

var saveRoomTypeOption = function () {
    ShowModalLoader();
    var data = $('#formSaveRoomTypeOption').serialize();
    $.ajax({
        type: 'GET',
        cache: false,
        url: "/RoomTypeOptions/CreateRoomTypeOption",
        data: data,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.StatusCode == 201) {
                OpenRoomTypeOption();
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

var DetailRoomTypeOptions = function (id) {
    $('.modal-dialog').css('max-width', '45%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/RoomTypeOptions/Details",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 204) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                $('#headerText').text("Details");
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

var EditRoomTypeOption = function (id) {
    $('.modal-dialog').css('max-width', '45%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/RoomTypeOptions/Edit",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 204) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                $('#headerText').text("Edit Room Type");
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

var updateRoomTypeOption = function () {
    ShowModalLoader();
    var data = $('#formEditRoomTypeOption').serialize();
    $.ajax({
        type: 'GET',
        cache: false,
        url: "/RoomTypeOptions/UpdateRoomTypeOptions",
        data: data,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.StatusCode == 201) {
                OpenRoomTypeOption();
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

var DeleteRoomTypeOption = function (id) {
    $('.modal-dialog').css('max-width', '40%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/RoomTypeOptions/Delete",
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

var DeleteConfirmedRoomType = function (officeTypeID) {
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': officeTypeID },
        url: "/RoomTypeOptions/DeleteConfirmed",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 405) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                OpenRoomTypeOption();
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