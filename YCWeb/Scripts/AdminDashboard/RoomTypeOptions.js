var OpenRoomTypeOption = function () {
    var options = {
        "backdrop": "static",
        keyboard: true
    };
    $.ajax({
        type: "GET",
        url: "/RoomTypeOptions/Index",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            $('#myModalContent').empty();
            $('#myModalContent').html(data);
            $('#myModal').modal(options);
            $('#headerText').text("Room Types Options");
            $('#myModal').modal('show');
        },
        error: function () {
            alert("Content load failed.");
        }
    });
    return false;
};

var CreateRoomTypeOption = function () {
    var options = {
        "backdrop": "static",
        keyboard: true
    };
    var url = "@Url.Action('Create','RoomTypeOptions')";
    $.post("/RoomTypeOptions/Create", function (data) {
        $('#myModalContent').empty();
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

var EditRoomTypeOption = function (id) {
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

var updateRoomType = function () {
    ShowModalLoader();
    var data = $('#formEditRoomType').serialize();
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