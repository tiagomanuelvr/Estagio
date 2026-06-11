$.get("/api/MoviesApi", function (data, status) {
    if (data == null || data.length == 0) {
        alert("Is empty");
    } else {
        for (var i = 0; i < data.length; i++) {
            var item = data[i];
            var tr = '<tr><td>' + item.title + '</td><td>' + item.releaseDate + '</td><td>' + item.genre + '</td><td>' + item.price + '</td><td>' + item.rating + '</td>';

            var actions =
                `
                <td>
                    <a href="/Movies/Edit/${item.id}">Edit</a> |
                    <a href="/Movies/Details/${item.id}">Details</a> |
                    <a href="/Movies/Delete/${item.id}">Delete</a>
                </td><tr>
                `

            var showTableBody = tr + actions;
            //     console.log(tr);
            $("#index_API tbody").append(showTableBody);
        }
    }
});