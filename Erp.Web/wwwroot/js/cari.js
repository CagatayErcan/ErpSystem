$(document).ready(function () {
    var table = $('#cariTable').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        scrollX: true,
        scrollY: '65vh',
        scrollCollapse: true,
        autoWidth: true,
        language: { url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/tr.json' },
        dom: 'frtip'
    });

    $('#excelBtn').click(function () {
        var data = [];
        var headers = [];
        $('#cariTable thead tr:first th').each(function () { headers.push($(this).text()); });
        data.push(headers);
        $('#cariTable tbody tr').each(function () {
            var row = [];
            $(this).find('td').each(function () { row.push($(this).text()); });
            data.push(row);
        });
        var ws = XLSX.utils.aoa_to_sheet(data);
        var wb = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, 'CariListesi');
        XLSX.writeFile(wb, 'CariListesi.xlsx');
    });

    $('#pdfBtn').click(function () { alert('PDF için ek kütüphane gerekir. Excel kullanın.'); });

    $('#printBtn').click(function () {
        var w = window.open('', '_blank');
        w.document.write('<html><head><title>Cari Listesi</title>');
        w.document.write('<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">');
        w.document.write('</head><body>');
        w.document.write('<h3>Cari Kartları</h3>');
        w.document.write(document.getElementById('cariTable').outerHTML);
        w.document.write('</body></html>');
        w.document.close();
        w.print();
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