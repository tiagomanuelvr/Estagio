$(document).ready(function () {

    //$('#create').click(function () {
    //    $.post("/api/MoviesApi", {movieData}, function (data, status) {

    //        console.log(data.value);
    //    });
    //});

    $('#formId').submit(function (e) {

        e.preventDefault();
        e.stopPropagation();

        var form = $(this);

        //var formData = new FormData(form);

        //var title = $("#Title").val();
        //var releaseDate = $('#ReleaseDate').val();
        //var genre = $('#Genre').val();
        //var price = $('#Price').val();
        //var rating = $('#Rating').val();

        //const movie = {
        //    title: title,
        //    releaseDate: releaseDate,
        //    genre: genre,
        //    price: price,
        //    rating: rating
        //};

        //$.post("../api/MoviesApi", { movie: movie });

        console.log(form.serialize());

        $.ajax({
            url: "/api/MoviesApi",
            type: "POST",
        //    contentType: "application/json charset=utf-8",
            data: form.serialize(),
          //  dataType: "json",
            success: function (data) {
                console.log("Sucesso!", data.value);
                window.location.href = "/Movies";
            }
        });

    });

});