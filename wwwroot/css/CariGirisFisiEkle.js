$(document).ready(function () {
    // Stok Kodu arama işlemi için tabloyu başlat
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
            lengthMenu: "_MENU_",
            zeroRecords: " ",
            info: "",
            infoEmpty: " ",
            infoFiltered: "",
            search: "Ara:",
            paginate: {
                next: "Sonraki",
                previous: "Önceki",
                emptyTable: " "
            }
        }
    });

    const table2 = new DataTable('#example4', {
        scrollY: false,
        scrollX: true,
        ordering: false,
        paging: false,
        searching: false,
        pageLength: 7,
        language: {
            lengthMenu: "_MENU_",
            zeroRecords: " ",
            info: "",
            infoEmpty: " ",
            infoFiltered: "",
            search: "Ara:",
            paginate: {
                next: "Sonraki",
                previous: "Önceki",
                emptyTable: " "
            }
        }
    });

    function filterTable(inputSelector, tableSelector) {
        $(inputSelector).on("keyup", function () {
            const value = $(this).val().toLowerCase();
            if (value === "") {
                $(tableSelector + " tbody tr").hide();
                $('#birimSearch, #stokAdiSearch, #miktar').val('');
            } else {
                $(tableSelector + " tbody tr").filter(function () {
                    $(this).toggle($(this).find("td").eq(0).text().toLowerCase().indexOf(value) > -1);
                });
            }
        });
    }

    // Arama fonksiyonları tanımla
    filterTable("#stokKoduSearch", "#example3");
    filterTable("#stokAdiSearch", "#example3");
    filterTable("#birimSearch", "#example3");

    // Sayfa yüklendiğinde tüm satırları gizle
    $("#example3 tbody tr").hide();

    table1.on('click', 'tbody tr', (e) => {
        let classList = e.currentTarget.classList;

        if (classList.contains('selected')) {
            classList.remove('selected');
            selectedRow = null;
        } else {
            table1.rows('.selected').nodes().each((row) => row.classList.remove('selected'));
            classList.add('selected');
            selectedRow = table1.row(e.currentTarget);
        }

        let stokkod = $(e.currentTarget).find('td').eq(0).text();
        let stokad = $(e.currentTarget).find('td').eq(1).text();
        let birim = $(e.currentTarget).find('td').eq(2).text();

        $('#stokKoduSearch').val(stokkod);
        $('#stokAdiSearch').val(stokad);
        $('#birimSearch').val(birim);
    });

    $('#addRowButton').click(function () {
        if (selectedRow) {
            let stokkod = $('#stokKoduSearch').val().trim();
            let stokad = $('#stokAdiSearch').val().trim();
            let birim = $('#birimSearch').val().trim();
            let miktar = $('#miktar').val().trim();

            miktar = Number(miktar);
            if (miktar === 0 || !Number.isFinite(miktar)) {
                const myModal = new bootstrap.Modal(document.getElementById('miktarBosModal'));
                myModal.show();
                return;
            }

            table2.row.add([stokkod, stokad, birim, miktar]).draw(false);

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
                    kpsft_Miktar: Number(miktar)  // Miktarı sayıya çeviriyoruz
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
            url: "http://localhost:5129/Admin/CariGirisFisiEkle",
            contentType: "application/json",
            data: JSON.stringify(jsonVeri)
        }).then(function (response) {
            window.location.href = "http://localhost:5129/Admin/GirisFisiTan%C4%B1m";
            }).catch(function (xhr) {
                console.log('Veri gönderimi sırasında bir hata oluştu:', xhr.status, xhr.statusText);
        });

    });


    $('.select-cari, .select-depo').click(function () {
        const selectedAd = $(this).data('value');
        const selectedKod = $(this).data('id');

        if ($(this).hasClass('select-cari')) {
            $('#kpsft_islemKaynagiCari').val(selectedAd);
            $('#kpsft_islemKaynagiCari_id').val(selectedKod);
        } else {
            $('#kpsft_depo').val(selectedAd);
            $('#kpsft_depo_id').val(selectedKod);
        }
        console.log(selectedAd);
    });

    $(document).keydown((e) => {
        if (e.key === 'Enter' && selectedRow) {
            $('#addRowButton').click();
            e.preventDefault();
        }
    });
});
