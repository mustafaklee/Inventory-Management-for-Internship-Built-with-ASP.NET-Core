$(document).ready(function () {
    const table1 = new DataTable('#example3', {
        scrollY: false,
        scrollX: true,
        paging: false,
        ordering: false,
        keys: true,
        hover: true,
        searching: false,
        pageLength: 7,
        language: {
            "lengthMenu": "_MENU_",
            "zeroRecords": "Kayıt bulunamadı",
            "info": "",
            "infoEmpty": "Kayıt bulunamadı",
            "infoFiltered": "",
            "search": "Ara:",
            "paginate": {
                "next": "Sonraki",
                "previous": "Önceki",
                "emptyTable": " ",
            }
        }
    });

    const table2 = new DataTable('#example4', {
        scrollY: false,
        scrollX: true,
        paging: false,
        ordering: false,
        keys: true,
        hover: true,
        searching: false,
        pageLength: 7,
        language: {
            "lengthMenu": "_MENU_",
            "zeroRecords": "Kayıt bulunamadı",
            "info": "",
            "infoEmpty": "Kayıt bulunamadı",
            "infoFiltered": "",
            "search": "Ara:",
            "paginate": {
                "next": "Sonraki",
                "previous": "Önceki",
                "emptyTable": " ",
            }
        }
    });

    // Stok kodu arama işlemi
    $("#stokKoduSearch").on("keyup", function () {
        const value = $(this).val().toLowerCase();
        if (value === "") {
            $("#example3 tbody tr").hide();
            $('#birimSearch, #stokAdiSearch, #miktar').val('');
        } else {
            $("#example3 tbody tr").filter(function () {
                $(this).toggle($(this).find("td").eq(0).text().toLowerCase().indexOf(value) > -1);
            });
        }
    });

    $("#example3 tbody tr").hide();

    // Tablo 1'de satır seçme işlemi
    let selectedRow;
    table1.on('click', 'tbody tr', function (e) {
        const classList = e.currentTarget.classList;
        if (classList.contains('selected')) {
            classList.remove('selected');
            selectedRow = null;
        } else {
            table1.rows('.selected').nodes().each(row => row.classList.remove('selected'));
            classList.add('selected');
            selectedRow = table1.row(e.currentTarget);
        }

        const stokkod = $(e.currentTarget).find('td').eq(0).text();
        const stokad = $(e.currentTarget).find('td').eq(1).text();
        const birim = $(e.currentTarget).find('td').eq(2).text();

        $('#stokKoduSearch').val(stokkod);
        $('#stokAdiSearch').val(stokad);
        $('#birimSearch').val(birim);
    });

    let selectedRow2;
    table2.on('click', 'tbody tr', function (e) {
        const classList = e.currentTarget.classList;
        if (classList.contains('selected')) {
            classList.remove('selected');
            selectedRow2 = null;
        } else {
            table2.rows('.selected').nodes().each(row => row.classList.remove('selected'));
            classList.add('selected');
            selectedRow2 = table2.row(e.currentTarget);
        }
    });

    $('#addRowButton').click(function () {
        if (selectedRow) {
            let stokkod = $('#stokKoduSearch').val().trim();
            let stokad = $('#stokAdiSearch').val().trim();
            let birim = $('#birimSearch').val().trim();
            let miktar = $('#miktar').val().trim();

            miktar = Number(miktar);
            if (miktar === 0 || !Number.isFinite(miktar) || $('#miktar').val().trim() === "") {
                const myModal = new bootstrap.Modal(document.getElementById('miktarBosModal'));
                myModal.show();
                return;
            }

            table2.row.add([
                stokkod,
                stokad,
                birim,
                miktar,
            ]).draw(false);

            $("#example3 tbody tr").hide();
            $('#stokKoduSearch, #stokAdiSearch, #birimSearch, #miktar').val('');
        }
    });

    $('#saveButton').click(function (e) {
        e.preventDefault();
        let tableData = [];

        $('#example4 tbody tr').each(function () {
            let row = $(this);
            let stokkod = row.find('td').eq(0).text().trim();
            let miktar = row.find('td').eq(3).text().trim();

            if (stokkod && miktar) {
                tableData.push({
                    kpsft_StokKod: stokkod,
                    kpsft_Miktar: Number(miktar)
                });
            }
        });

        let fisno = $('#kpsft_FisNo').val().trim();
        let Tarih = $('#kpsft_tarih').val().trim();
        let cari = $('#kpsft_islemKaynagiCari_id').val().trim();
        let depo = $('#kpsft_depo_id').val().trim();
        let jsonVeri = {
            FisNo: fisno,
            Tarih: Tarih,
            cari: cari,
            depo: depo,
            Data: tableData
        };
        console.log(jsonVeri);
        $.ajax({
            type: "POST",
            url: `http://localhost:5129/Admin/CariCikisFisiDuzenle/${fisno}`,
            contentType: "application/json",
            data: JSON.stringify(jsonVeri)
        }).then(function (response) {
            window.location.href = "http://localhost:5129/Admin/CikisFisiTan%C4%B1m";
        }).catch(function (xhr) {
            console.log('Veri gönderimi sırasında bir hata oluştu:', xhr.status, xhr.statusText);
        });

    });

    $('.select-cari').click(function () {
        $('#kpsft_islemKaynagiCari').val($(this).data('value'));
        $('#kpsft_islemKaynagiCari_id').val($(this).data('id'));
    });

    $('.select-depo').click(function () {
        $('#kpsft_depo').val($(this).data('value'));
        $('#kpsft_depo_id').val($(this).data('id'));
    });

    $(document).keydown((e) => {
        if (e.key === 'Enter' && selectedRow) {
            $('#addRowButton').click();
            e.preventDefault();
        }
    });

    $('#deleteRow').on('click', function () {
        if (selectedRow2) {
            selectedRow2.remove().draw();
            selectedRow2 = null;
        } else {
            alert("Lütfen silmek için bir satır seçin!");
        }
    });
});
