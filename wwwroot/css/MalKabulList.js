const table = new DataTable('#example', {
    scrollY: false,
    scrollX: true,
    paging: true,
    searching: false,
    pageLength: 7,
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


let selectedRow;

table.on('click', 'tbody tr', (e) => {
    let classList = e.currentTarget.classList;

    if (classList.contains('selected')) {
        classList.remove('selected');
        selectedRow = null;
    } else {
        table.rows('.selected').nodes().each((row) => row.classList.remove('selected'));
        classList.add('selected');
        selectedRow = table.row(e.currentTarget);
    }
});


document.querySelector('#teslimAl').addEventListener('click', function () {
    if (selectedRow) {
        let evrakNo = selectedRow.data()[0];
        let stokKod = selectedRow.data()[1];
        if (stokKod && evrakNo) {
            window.location.href = '/Admin/CariGirisFisiEkle?evrakNo=' + evrakNo + '&stokKod=' + stokKod;


        }
    } else {
        alert('Lütfen teslim almak için bir satır seçin.');
    }
});


$(document).ready(function () {
    $('#confirmDelete').on('click', function () {
        if (selectedRow) {
            selectedRow.remove();
            $('#deleteConfirmationModal').modal('hide');
        }
    });


    $('#close').on('click', function () {
        if (selectedRow) {
            $('#deleteConfirmationModal').modal('hide');
        }
    });

    $("#stokKoduSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(0).text().toLowerCase().indexOf(value) > -1);
        });
    });


    $("#stokAdiSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(1).text().toLowerCase().indexOf(value) > -1);
        });
    });


    $("#birimSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(2).text().toLowerCase().indexOf(value) > -1);
        });
    });


    // tipSearch Kodu Sütunu Araması
    $("#tipSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(3).text().toLowerCase().indexOf(value) > -1);
        });
    });

    // tipSearch Kodu Sütunu Araması
    $("#durumSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(4).text().toLowerCase().indexOf(value) > -1);
        });
    });

    // Stok Adı Sütunu Araması
    $("#grupKoduSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(5).text().toLowerCase().indexOf(value) > -1);
        });
    });

    // Birim Sütunu Araması
    $("#stokKartTipSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(6).text().toLowerCase().indexOf(value) > -1);
        });
    });

    // Birim Sütunu Araması
    $("#stokAilesiSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(7).text().toLowerCase().indexOf(value) > -1);
        });
    });


    // Birim Sütunu Araması
    $("#kritikMiktarSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(8).text().toLowerCase().indexOf(value) > -1);
        });
    });


    $("#alisOrtBirimSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#example tbody tr").filter(function () {
            $(this).toggle($(this).find("td").eq(9).text().toLowerCase().indexOf(value) > -1);
        });
    });

});