$(document).ready(function () {
    ReloadPage();
    console.log("ready!");

    //Makes the selected element active
    $("#Action li a").click(function () {

        $("#Action li").removeClass("active");
        $(this).parent().addClass("active");
    });
    // Makes the selected element active
    $("#Reaction li a").click(function () {

        $("#Reaction li").removeClass("active");
        $(this).parent().addClass("active");
    });

    $("#AddActionButton").click(function () {
        AddElement();
    });
});


function ReloadPage(){
    var window = $('#myWindow');
    window.append('oui');
}

function AddElement() {
    var action = $("#Action");
    var reaction = $("#Reaction");

    var obj = new Object();
    obj.action = action.option[action.selectedIndex].value;
    obj.reaction = action;
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
}