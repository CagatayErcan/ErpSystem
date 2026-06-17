$(document).ready(function () {
    var table = $('#stokTable').DataTable({
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

    $('#excelBtn').click(function () {
        var data = [], headers = [];
        $('#stokTable thead tr:first th').each(function () { headers.push($(this).text()); });
        data.push(headers);
        $('#stokTable tbody tr').each(function () {
            var row = [];
            $(this).find('td').each(function () { row.push($(this).text()); });
            data.push(row);
        });
        var ws = XLSX.utils.aoa_to_sheet(data);
        var wb = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, 'StokListesi');
        XLSX.writeFile(wb, 'StokListesi.xlsx');
    });

    $('#pdfBtn').click(function () { alert('PDF için ek kütüphane gerekir. Excel kullanın.'); });

    $('#printBtn').click(function () {
        var w = window.open('', '_blank');
        w.document.write('<html><head><title>Stok Listesi</title><link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"></head><body>');
        w.document.write('<h3>Stok Kartları</h3>' + document.getElementById('stokTable').outerHTML + '</body></html>');
        w.document.close(); w.print();
    });

    $('.column-filter').on('keyup change', function () {
        var idx = $(this).data('column');
        table.column(idx).search($(this).val()).draw();
        var c = $('.column-filter').filter(function () { return $(this).val() !== ''; }).length;
        $('#filterCount').text(c + ' filtre aktif');
    });

    $('#clearAllFilters').click(function () {
        $('.column-filter').val('');
        table.columns().search('').draw();
        $('#filterCount').text('0 filtre aktif');
    });
});