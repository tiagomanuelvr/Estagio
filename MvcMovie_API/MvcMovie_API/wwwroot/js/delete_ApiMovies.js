//$(document).ready(function () {
$(function () {
    var id = $("#id").val();
    $.get("/api/MoviesApi/" + id, function (data, status) {
        if (data == null) {
            alert("Is empty");
        } else {
            $(".title").html(data.title);
            $(".releaseDate").html(data.releaseDate);
            $(".genre").html(data.genre);
            $(".price").html(data.price);
            $(".rating").html(data.rating);
        }
    });

    $('form').submit(function (e) {

        e.preventDefault();
        e.stopPropagation();

        var form = $(this);

        console.log(form.serialize());

        $.ajax({
            url: "/api/MoviesApi/" + id,
            type: "DELETE",
            data: form.serialize(),
            success: function (data) {
                console.log(data);
                window.location.href = "/Movies";
            }
        });

    });

});