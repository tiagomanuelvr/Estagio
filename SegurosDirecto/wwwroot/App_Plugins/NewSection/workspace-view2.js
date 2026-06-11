import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import { css, html, customElement, property } from '@umbraco-cms/backoffice/external/lit';

export class MySectionViewElement2 extends UmbLitElement {

    fetchData(e) {
        e.preventDefault();
        var url = "https://localhost:7240/api/MoviesApi/sendCsv";

        const inicio = this.shadowRoot.querySelector('#DataInicio').value;
        const fim = this.shadowRoot.querySelector('#DataFim').value;

        $.ajax({
            url: url,
            type: "GET",
            data: {
                "dataInicio": inicio,
                "dataFim": fim
            },
            headers: {
                "X-API-KEY": "QXBpS2V5TWlkZGxld2FyZQ=="
            },
            contentType: "application/octet-stream",
            success: function (data, status, xhr) {
                var fileName = xhr.getResponseHeader('content-disposition').split('filename=')[1].split(';')[0];
                console.log(fileName);

                var blob = new Blob([data], { type: 'application/octet-stream' });
                var a = document.createElement('a');
                a.href = window.URL.createObjectURL(blob);
                a.download = fileName;
                a.click();
                window.URL.revokeObjectURL(url);
            }
        });
    }

    render() {
        return html`
            <uui-box headline="Exportar" style = "--uui-box-default-padding: 30px; margin:20px">
                <uui-form>
                    <form id="MyForm" name="myForm" @submit=${this.fetchData}>
                        <uui-form-layout-item>
                            <uui-label for="DataInicio" slot="label" required="">Data de início</uui-label>
                            <uui-input id="DataInicio" required label="Data de início" placeholder="Placeholder" type="datetime-local" value="0001-01-01T00:00" min="0001-01-01T00:00" max="2026-12-31T00:00" style=""></uui-input>
                        </uui-form-layout-item>
                        <uui-form-layout-item>
                            <uui-label for="DataFim" slot="label" required>Data de Fim</uui-label>              
                            <uui-input id="DataFim" required label="Data de Fim" placeholder="Placeholder" type="datetime-local" value="2026-04-02T00:00" min="0001-01-01T00:00" max="2026-12-31T00:00" style=""></uui-input>
                        </uui-form-layout-item>
                        <uui-button type="reset" label="Reset" look="secondary" color="default">Reset</uui-button>
                        <uui-button id="submitForm" type="submit" label="Submit" look="primary" color="default" pristine="">Export</uui-button>
                    </uui-form>
                </form>
            </uui-box>`            
            ;
    }
}

customElements.define('my-sectionview-element2', MySectionViewElement2);

export default MySectionViewElement2;