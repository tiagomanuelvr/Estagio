import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import { css, html, customElement, property } from '@umbraco-cms/backoffice/external/lit';

export class SectionViewUuiTableElement extends UmbLitElement {

    static get properties() {
        return {
            movies: { type: Array },
            skip: { type: Number },          
            itemsPerPage: { type: Number },
            totalRecords: { type: Number },
            totalPages: { type: Number },
            _currentPage: { type: Number },
            PAGE_SIZE_OPTIONS: { type: Number },
            DEFAULT_PAGE_SIZE: {type: Number}
        };
    }
    constructor() {
        super();
        this.movies = [];
        this.skip = 0;
        this.itemsPerPage = 10;
        this.totalRecords = 10;
        this.totalPages = 1;
        this._currentPage = 1;
        this.PAGE_SIZE_OPTIONS=  [10, 25, 50];
        this.DEFAULT_PAGE_SIZE= 10;
    }

    firstUpdated() {
        this.fetchData(this.skip, this.itemsPerPage);
    }

    fetchData(skip, itemsPerPage, search = "") {        
        var url = `https://localhost:7240/api/MoviesApi/datatable?skip=${skip}&length=${itemsPerPage}&search=${search}`;

        //var self = this;
        $.ajax({
            url: url,
            type: "GET",
            headers: {
                "X-API-KEY": "QXBpS2V5TWlkZGxld2FyZQ=="
            },           
            success: (data) => {
                this.movies = data.data;
                this.totalRecords = data.recordsFiltered;
                //console.log(this._currentPage)
                //console.log(this.movies);
                //console.log(this.totalRecords);
                //console.log(this.totalPages);
                //console.log(data.recordsFiltered / this.itemsPerPage);
                //console.log();
            }
            //success: function (data) {
            //    self.movies = data.data;
            //    console.log(this.movies);
            //}
        });
    }

    get totalPages() {
        //console.log(Math.ceil(this.totalRecords / this.itemsPerPage))
        return Math.ceil(this.totalRecords / this.itemsPerPage);
    }

    #onPageChange(event) {
        this._currentPage = event.target.current;
        //console.log(this._currentPage + " asd");
        this.fetchData((this._currentPage - 1) * this.itemsPerPage, this.itemsPerPage);
    }

    #onPageSizeChange(newSize) {        
        this._pageSize = newSize;

        const newTotalPages = Math.ceil(this.totalRecords / newSize);
        this.itemsPerPage = newSize;
        //console.log("pages:   " + this.totalPages + "   " + newTotalPages);
        this._currentPage = Math.min(this._currentPage, Math.max(1, newTotalPages));
        //console.log(this._currentPage);
        this.fetchData((this._currentPage - 1) * newSize, newSize);
    }

    #onSearchChange(event) {
        console.log(" asdklahdjvgas");
        const search = event.target.value;
        console.log(search + " asdklahdjvgas");
        this.fetchData((this._currentPage - 1) * this.itemsPerPage, this.itemsPerPage, search);
    }


    render() {
        return html`
            <uui-box headline="Datatable" style="--uui-box-default-padding: 30px; margin:50px;">
                <div style="display:flex; justify-content: space-between">
                    <div class="page-size">
                        Results per page:
                        ${this.PAGE_SIZE_OPTIONS.map((size, index) => html`
                            ${index > 0 ? html`<span>|</span>` : ''}
                            <uui-button
                                label= "Size"
                                compact 
                                .look="${this._pageSize === size ? 'primary' : 'default'}"
                                @click="${() => this.#onPageSizeChange(size)}">
                                ${size}
                            </uui-button>
                        `)}
                    </div>
                    <div style="display:flex; align-items:center; justify-content:end">
                        <uui-label for="MyInput">Search:</uui-label>
                        <uui-input 
                            id="MyInput" 
                            label="My A11Y Label"
                            @input="${this.#onSearchChange}">
                        </uui-input>
                    </div>
                </div>
                <uui-table role="table">
                    <uui-table-head>
                        <uui-table-head-cell>Title</uui-table-head-cell>
                        <uui-table-head-cell>releaseDate</uui-table-head-cell>
                        <uui-table-head-cell>genre</uui-table-head-cell>
                        <uui-table-head-cell>price</uui-table-head-cell>
                        <uui-table-head-cell>Rating</uui-table-head-cell>
                   </uui-table-head>
                   
                   ${this.movies.map(movie => html` <uui-table-row>
                       <uui-table-cell>${movie.title}</uui-table-cell>
                       <uui-table-cell>${movie.releaseDate}</uui-table-cell>
                       <uui-table-cell>${movie.genre}</uui-table-cell>
                       <uui-table-cell>${movie.price}</uui-table-cell>
                       <uui-table-cell>${movie.rating}</uui-table-cell>
                   </uui-table-row>`)}
               </uui-table>

               <div class="pager-container">
                    <uui-pagination
                        total="${this.totalPages}" 
                        current="${this._currentPage}"
                        @change="${this.#onPageChange}">
                    </uui-pagination>
               </div>
            </uui-box>`
            ;
    }
}

customElements.define('sectionview-uuitable-element', SectionViewUuiTableElement);

export default SectionViewUuiTableElement;