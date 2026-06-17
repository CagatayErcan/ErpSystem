$(document).ready(function () {
    $('#bomTable').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        scrollX: true,
        scrollY: '65vh',
        scrollCollapse: true,
        autoWidth: false,
        language: { url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/tr.json' },
        dom: 'frtip'
    });
});