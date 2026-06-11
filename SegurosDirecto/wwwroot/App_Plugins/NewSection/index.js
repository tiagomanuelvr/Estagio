export const onInit = (host, extensionsRegistry) => {
    const script = document.createElement('script');
    script.src = 'https://cdn.datatables.net/v/dt/jq-3.7.0/dt-2.3.7/datatables.min.js';
    script.type = 'text/javascript';
    document.head.appendChild(script);
}