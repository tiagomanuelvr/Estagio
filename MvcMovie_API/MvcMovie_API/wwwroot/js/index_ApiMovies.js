$(document).ready(function () {
    //$.get("/api/MoviesApi", function (data, status) {
    //    if (data == null || data.length == 0) {
    //        alert("Is empty");
    //    } else {
    //        for (var i = 0; i < data.length; i++) {
    //            var item = data[i];
    //            var tr = '<tr><td>' + item.title + '</td><td>' + item.releaseDate + '</td><td>' + item.genre + '</td><td>' + item.price + '</td><td>' + item.rating + '</td>';

    //            var actions =
    //                `
    //                <td>
    //                    <a href="/Movies/Edit/${item.id}">Edit</a> |
    //                    <a href="/Movies/Details/${item.id}">Details</a> |
    //                    <a href="/Movies/Delete/${item.id}">Delete</a>
    //                </td><tr>
    //                `

    //            var showTableBody = tr + actions;
    //            //     console.log(tr);
    //            $("#index_API tbody").append(showTableBody);
    //        }
    //    }
    //});

    //$('#filterButton').click(function (e) {
    //    e.preventDefault();
    //    //e.stopPropagation();

    //    var parameter = $("#inputText").val();
    //    var movieGenreParameter = $("#Genre").val();
    //    //console.log(parameter, movieGenreParameter);

    //    $.get("/api/MoviesApi", { searchString: parameter, movieGenre: movieGenreParameter }, function (data, status) {
    //        if (data == null || data.length == 0) {
    //            alert("Is empty");
    //        } else {
    //            $("#index_API tbody").html("");
    //            for (var i = 0; i < data.length; i++) {
    //                var item = data[i];
    //                var tr = $(".cloneTr tr").clone();
    //                $(tr).find(".title").html(item.title);
    //                //$(tr).find(".title").html(item.releaseDate);

    //                console.log(tr[0]);

    //                var tr = '<tr><td>' + item.title + '</td><td>' + item.releaseDate + '</td><td>' + item.genre + '</td><td>' + item.price + '</td><td>' + item.rating + '</td>';
    //                //     console.log(tr);
    //                var actions =
    //                    `
    //                        <td>
    //                            <a href="/Movies/Edit/${item.id}">Edit</a> |
    //                            <a href="/Movies/Details/${item.id}">Details</a> |
    //                            <a href="/Movies/Delete/${item.id}">Delete</a>
    //                        </td><tr>
    //                        `

    //                var showTableBody = tr + actions;
    //                $("#index_API tbody").append(showTableBody);
    //            }
    //        }
    //    });
    //});

    //$('#index_API').DataTable({
    //    ajax: {
    //        url: '/api/MoviesApi',
    //        dataSrc: ''
    //    },
    //    columns: [
    //        { data: "title" },
    //        { data: "releaseDate" },
    //        { data: "genre" },
    //        { data: "price" },
    //        { data: "rating" },
    //        {
    //            data: 'id',
    //            render: function (id) {
    //                return `<a href="/Movies/Edit/${id}">Edit</a> |
    //                        <a href="/Movies/Details/${id}">Details</a> |
    //                        <a href="/Movies/Delete/${id}">Delete</a>`
    //            }
    //        }
    //    ]
    //});

    $('#index_API').DataTable({
        serverSide: true,
        //processing: true,
        ajax: {
            url: '/api/MoviesApi/getMovies',
            type: 'POST',
            headers: {
                "X-API-KEY": "QXBpS2V5TWlkZGxld2FyZQ==",
            },
            contentType: 'application/json',
            data: function (d) {
                return JSON.stringify(d);
            },
            dataSrc: 'data'
        },
        columns: [
            { data: "title", "name": "Title" },
            { data: "releaseDate", "name": "ReleaseDate" },
            { data: "genre", "name": "Genre" },
            { data: "price", "name": "Price" },
            { data: "rating", "name": "Rating" },
            {
                data: 'id',
                render: function (id) {
                    return `<a href="/Movies/Edit/${id}">Edit</a> |
                            <a href="/Movies/Details/${id}">Details</a> |
                            <a href="/Movies/Delete/${id}">Delete</a>`
                }
            }
        ]
    });

});