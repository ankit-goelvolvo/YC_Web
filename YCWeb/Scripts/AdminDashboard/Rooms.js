var OpenRooms = function () {
    $('.modal-dialog').css('max-width', '65%');
    var options = {
        "backdrop": "static",
        keyboard: true
    };
    $('#myModalContent').empty();
    $.ajax({
        type: "GET",
        url: "/Rooms/Index",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {

            $('#myModalContent').html(data);
            $('#myModal').modal(options);
            $('#headerText').text("Rooms");
            $('#myModal').modal('show');
        },
        error: function () {
            alert("Content load failed.");
        }
    });
    return false;
};

var CreateRooms = function () {
    $('.modal-dialog').css('max-width', '45%');
    var options = {
        "backdrop": "static",
        keyboard: true
    };
    var url = "@Url.Action('Create','RoomTypeOptions')";
    $('#myModalContent').empty();
    $.post("/Rooms/Create", function (data) {

        $('#myModalContent').html(data);
        $('#myModal').modal(options);
        $('#headerText').text("Room");
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

var saveRooms = function () {
    ShowModalLoader();
    var data = $('#formSaveRooms').serialize();
    $.ajax({
        type: 'GET',
        cache: false,
        url: "/Rooms/CreateRooms",
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

var DetailRooms = function (id) {
    $('.modal-dialog').css('max-width', '45%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/Rooms/Details",
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

var EditRooms = function (id) {
    $('.modal-dialog').css('max-width', '45%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/Rooms/Edit",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 204) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                $('#headerText').text("Edit Room");
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

var updateRooms = function () {
    ShowModalLoader();
    var data = $('#formEditRooms').serialize();
    $.ajax({
        type: 'GET',
        cache: false,
        url: "/Rooms/UpdateRooms",
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

var DeleteRooms = function (id) {
    $('.modal-dialog').css('max-width', '40%');
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': id },
        url: "/Rooms/Delete",
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

var DeleteConfirmedRooms = function (RoomtypeId, officeTypeID) {
    ShowModalLoader();
    $.ajax({
        type: "GET",
        data: { 'id': officeTypeID },
        url: "/Rooms/DeleteConfirmed",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            if (data.StatusCode == 405) {
                $('#footerText').text(data.StatusMessage);
                $('#footerText').show();
                $('#footerText').delay(5000).fadeOut();
            }
            else {
                DeleteRoomTypeOption(RoomtypeId);
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