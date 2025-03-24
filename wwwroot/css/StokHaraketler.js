$(document).ready(function () {
    const table = $('#example5').DataTable({
        scrollY: false,
        scrollX: true,
        paging: true,
        searching: true,
        pageLength: 5,
        language: {
            "lengthMenu": "Sayfa başına _MENU_ kayıt göster",
            "zeroRecords": "Kayıt bulunamadı",
            "info": "_PAGE_ / _PAGES_",
            "infoEmpty": "Kayıt bulunamadı",
            "infoFiltered": "(toplam _MAX_ kayıttan filtrelendi)",
            "search": "Ara:",
            "paginate": {
                "next": "Sonraki",
                "previous": "Önceki"
            }
        }
    });

    // Uygula butonuna basıldığında çalışacak olan form submit olayı
    $('form').on('submit', function (event) {
        event.preventDefault(); // Sayfanın yenilenmesini engeller

        // Tarih alanlarını flatpickr ile oluştur
        let minDate = flatpickr('#min', {
            dateFormat: 'Y-m-d' // ISO formatı, 'MMMM Do YYYY' kullanabilirsiniz
        });
        let maxDate = flatpickr('#max', {
            dateFormat: 'Y-m-d'
        });

        // Custom filtering function for date range
        $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
            let min = $('#min').val() ? new Date($('#min').val()) : null;
            let max = $('#max').val() ? new Date($('#max').val()) : null;
            let date = new Date(data[4]); // Tablodaki tarih sütunu

            if (
                (min === null && max === null) ||
                (min === null && date <= max) ||
                (min <= date && max === null) ||
                (min <= date && date <= max)
            ) {
                return true;
            }
            return false;
        });

        // Tabloyu filtrele
        table.draw();
    });
});



    $("#tarihSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example5 tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(0).text().toLowerCase().indexOf(value) > -1);
        });
    });

    $("#stokKoduSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example5 tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(1).text().toLowerCase().indexOf(value) > -1);
        });
    });

    $("#stokAdiSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example5 tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(2).text().toLowerCase().indexOf(value) > -1);
        });
    });

    $("#birimSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example5 tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(3).text().toLowerCase().indexOf(value) > -1);
        });
    });

    $("#islemTipiSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example5 tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(4).text().toLowerCase().indexOf(value) > -1);
        });
    });

    $("#miktarSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example5 tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(5).text().toLowerCase().indexOf(value) > -1);
        });
    });

    $("#depoSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example5 tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(6).text().toLowerCase().indexOf(value) > -1);
        });
    });

    $("#islemKaynağıSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example5 tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(7).text().toLowerCase().indexOf(value) > -1);
        });
    });

