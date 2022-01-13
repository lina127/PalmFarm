// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("document").ready(function () {
    $.ajax({
        url: "/Farms/GetFarmName",
        dataType: "text",
        type: "Post",
        success: function (result) {
            $("#farmName").html(result);
            $("#farmName").css("color", "blue");
            $("#farmName").css("font-size", "23px");
        }

    })
});