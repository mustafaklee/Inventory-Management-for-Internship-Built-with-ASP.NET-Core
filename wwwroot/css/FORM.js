$(document).ready(function () {
    const table1 = new DataTable('#example3', {
        scrollY: false,
        scrollX: true,
        paging: true,
        ordering: false,
        searching: true,
        pageLength: 7,
        language: {
            "lengthMenu": "_MENU_",
            "zeroRecords": "Kayıt bulunamadı",
            "info": "_TOTAL_ kayıttan _START_ - _END_ arası gösteriliyor",
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
    
    $("#stokKoduSearch").on("keyup", function () {
        const value = $(this).val().toLowerCase();
        if (value === "") {
            table1.search('').draw(); // Tablodaki aramayı sıfırla
            $('#birimSearch, #stokAdiSearch, #miktar').val('');
        } else {
            table1.search(value).draw(); // DataTable'ın kendi arama fonksiyonunu kullan
        }
    });

    

    
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
        const birim = $(e.currentTarget).find('td').eq(3).text();

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
            let miktar = $('#miktarSearch').val().trim();
            let birimFiyat = $('#birimFiyatSearch').val().trim();
            let gemi = $('#gemiSearch').val().trim();
            let kdv = $('#kdvSearch').val().trim();
            let tutar = $('#tutarSearch').val().trim();

            miktar = Number(miktar);
            if (miktar === 0 || !Number.isFinite(miktar) || $('#miktarSearch').val().trim() === "") {
                const myModal = new bootstrap.Modal(document.getElementById('miktarBosModal'));
                myModal.show();
                return;
            }
            
            table2.row.add([
                stokkod,
                stokad,
                miktar,
                birim,
                birimFiyat,
                gemi,
                kdv,
                tutar
            ]).draw(false);
            
            updateTotals();
            
            $('#stokKoduSearch, #stokAdiSearch, #miktarSearch, #birimSearch, #birimFiyatSearch, #gemiSearch, #kdvSearch, #tutarSearch').val('');
        }
    });

    

    $('#miktarSearch, #birimFiyatSearch, #kdvSearch').on('keyup change', function () {
        let miktar = parseFloat($('#miktarSearch').val().trim()) || 0;
        let birimFiyat = parseFloat($('#birimFiyatSearch').val().trim()) || 0;
        let kdv = parseFloat($('#kdvSearch').val().trim()) || 0;

        let tutar = miktar * birimFiyat * (1 + kdv / 100);

        $('#tutarSearch').val(tutar.toFixed(2));
    });



    function updateTotals() {
        let total = 0;
        let kdvTotal = 0;
        
        table2.rows().data().each(function (row) {
            const tutar = parseFloat(row[7]) || 0; 
            const kdvOrani = parseFloat(row[6]) || 0; 

            total += tutar;
            kdvTotal += tutar * (kdvOrani / 100);
        });

        const grandTotal = total + kdvTotal;
        
        $('#total').val(total.toFixed(2));
        $('#kdv').val(kdvTotal.toFixed(2));
        $('#grandTotal').val(grandTotal.toFixed(2));
    }



    $('#saveButton').click(function (e) {
        e.preventDefault();
        let tableData = [];

        $('#example4 tbody tr').each(function () {
            let row = $(this);
            let stokkod = row.find('td').eq(0).text().trim();
            let kpsft_Miktar = row.find('td').eq(2).text().trim();
            let kpsft_birimFiyat = row.find('td').eq(4).text().trim();
            let kpsft_kdv = row.find('td').eq(6).text().trim();
            let kpsft_tutar = row.find('td').eq(7).text().trim();
            let kpsft_gemi = row.find('td').eq(5).text().trim();
            if (stokkod && kpsft_Miktar) {
                tableData.push({
                    kpsft_stokKod: stokkod,
                    kpsft_birimFiyat: Number(kpsft_birimFiyat),
                    kpsft_kdv: Number(kpsft_kdv),
                    kpsft_tutar: Number(kpsft_tutar),
                    kpsft_gemi: kpsft_gemi,
                    kpsft_Miktar: Number(kpsft_Miktar)
                });
            }
        });



        let kpsft_evrakNo = $('#kpsft_evrakNo').val().trim();
        let kpsft_evrak_tarih = $('#kpsft_tarih_evrak').val();
        let kpsft_teslim_tarih = $('#kpsft_tarih_teslim').val();
        let kpsft_cari = $('#kpsft_cari').val().trim();
        let kpsft_aciklama = $('#kpsft_aciklama').val().trim();
        let kpsft_satici_notu = $('#kpsft_satici_notu').val().trim();
        let kpsft_paraBirim = $('#paraBirimi').val().trim();

        if (!kpsft_evrakNo || !kpsft_evrak_tarih || !kpsft_teslim_tarih || !kpsft_cari || !kpsft_paraBirim || tableData.length === 0) {
            alert('Lütfen tüm alanları doldurun ve tabloya en az bir veri girin.');
            return; 
        }

        let jsonVeri = {
            kpsft_evrakNo: kpsft_evrakNo,
            kpsft_evrak_tarih: kpsft_evrak_tarih,
            kpsft_teslim_tarih: kpsft_teslim_tarih,
            kpsft_cari: kpsft_cari,
            kpsft_aciklama: kpsft_aciklama,
            kpsft_satici_notu: kpsft_satici_notu,
            kpsft_paraBirim: kpsft_paraBirim,
            datas: tableData
        };
        $.ajax({
            type: "POST",
            url: "http://localhost:5129/Admin/FORMM",
            contentType: "application/json",
            data: JSON.stringify(jsonVeri)
        }).then(function (response) {
            window.location.href = "http://localhost:5129/Admin/KutukSiparisListesi";
        }).catch(function (xhr) {
            console.log('Veri gönderimi sırasında bir hata oluştu:', xhr.status, xhr.statusText);
        });

    });

    $('.select-cari').click(function () {
        var selectedData = $(this).data();
        $('#kpsft_cari').val(selectedData.kod);
        $('#kpsft_cariAd').val(selectedData.ad);
        $('#kpsft_vergiNo').val(selectedData.vno);
        $('#kpsft_vergiDairesi').val(selectedData.vdaire);
        $('#kpsft_adres1').val(selectedData.adres);
        $('#kpsft_telefon1').val(selectedData.telefon);
        $('#kpsft_email').val(selectedData.email);
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


