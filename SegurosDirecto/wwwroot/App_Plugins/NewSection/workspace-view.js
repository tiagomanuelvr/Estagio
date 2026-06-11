import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import { css, html, customElement, property, LitElement } from '@umbraco-cms/backoffice/external/lit';

export class MySectionViewElement extends UmbLitElement {    

    firstUpdated() {
        const tableElement = this.shadowRoot.querySelector('#index_API');

        if (window.jQuery) {
            window.jQuery(tableElement).DataTable({
                serverSide: true,
                ajax: {
                    url: 'https://localhost:7240/api/MoviesApi/getMovies',
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
                    { data: "rating", "name": "Rating" } 
                ]
            });
        }
    }

    render() { 
        return html`
                <link href="https://cdn.datatables.net/v/dt/jq-3.7.0/dt-2.3.7/datatables.min.css" rel="stylesheet" integrity="sha384-el2AwdLu6iZ/JDWmbTZM6qT80ryVvbryXak11r7q8FPlxKvbfynXlYfhi9csn+9N" crossorigin="anonymous">
                <uui-box headline="Pedidos" style="--uui-box-default-padding: 30px; margin:20px;">
                    <table class="table" id="index_API">
                        <thead>
                            <tr>
                                <th>Title</th>
                                <th>ReleaseDate</th>        
                                <th>Genre</th>
                                <th>Price</th>
                                <th>Rating</th>
                            </tr>
                        </thead>
                    </table>
                </uui-box>                
                `
            ;
    }
               //<uui-button @click=${this.fetchData} label="Export csv" look="primary"></uui-button>

    
}

customElements.define('my-sectionview-element', MySectionViewElement);

export default MySectionViewElement;