$(document).ready(function () {
    ReloadPage();
    console.log("ready!");
    $("#AddActionButton").click(function () {
        AddElement();
    });
    GetElements();
});


function ReloadPage() {
    var window = $('#myWindow');
    window.append('oui');
}

function AddElement() {
    var action = document.getElementById("Action");
    var reaction = document.getElementById("Reaction");

    var obj = new Object();
    obj.action = $("#Action :selected").val();
    obj.reaction = $("#Reaction :selected").val();
    $.ajax({
        type: "POST",
        url: "/myservices/addservice",
        async: true,
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.success)
                console.log("success");
            else
                console.log("error");
        },
        error: function (data) {
            console.log("error");
        },
    });
    GetElements();
}

function GetElements() {
    var Services = [];
    $.ajax({
        type: "POST",
        url: "/myservices/getservices",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data != "error") {
                Services = JSON.parse(JSON.stringify(data));
                DisplayServices(Services);
                console.log("success");
            }
            else if (data == "error")
                console.log("error");
        },
        error: function (data) {
            console.log("error");
        },
    });
}

function DisplayServices(Services) {
    var elements = ""
    $(Services).each(function (index, value) {
        elements += `<button type="button" class="services div-inline btn btn-default">` + value.Action1 +`</button>
        <button type="button" class="services div-inline btn btn-default">` + value.Reaction +`</button>
        <button type="button" id="`+ value.Id + `" class="services btn btn-danger" onclick="DeleteService(`+ value.Id +`)">Delete</button> <br />`;
    });
    $("#Elements").html(elements);
}

function DeleteService(id) {
    var obj = new Object();
    obj.id = id;
    $.ajax({
        type: "POST",
        url: "/myservices/deleteservice",
        async: true,
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data == "success") {
                console.log("success");
                $("#Elements").html("");
                GetElements();
            }
            else
                console.log("error");
        },
        error: function (data) {
            console.log(data);
        },
    });
}