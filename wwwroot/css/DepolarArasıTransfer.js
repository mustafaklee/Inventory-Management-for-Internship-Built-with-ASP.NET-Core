$(document).ready(function () {
    // Stok Kodu arama işlemi için tabloyu başlat
    const table1 = new DataTable('#example3', {
        scrollY: false,
        scrollX: true,
        paging: false,
        "ordering": false,
        keys: true,
        hover:true,
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
        "ordering": false,
        paging: false,
        searching: false,
        pageLength: 7,
        "emptyTable": " ",
        language: {
            "lengthMenu": "_MENU_",
            "zeroRecords": " ",
            "info": "",
            "infoEmpty": " ",
            "infoFiltered": "",
            "search": "Ara:",
            "paginate": {
                "next": "Sonraki",
                "previous": "Önceki",
                "emptyTable": " ",
            }
        }
    });


    $(document).ready(function () {
        // Arama yapıldığında satırları görüntülemek için
        $("#stokKoduSearch").on("keyup", function () {
            var value = $(this).val().toLowerCase();

            // Eğer arama alanı boşsa tüm satırları gizle
            if (value === "") {
                $("#example3 tbody tr").hide();
                $('#birimSearch').val('');
                $('#stokAdiSearch').val('');
                $('#miktar').val('');
            } else {
                // Arama ile eşleşen satırları göster
                $("#example3 tbody tr").filter(function () {
                    $(this).toggle($(this).find("td").eq(0).text().toLowerCase().indexOf(value) > -1);
                });
            }
        });

        // Sayfa yüklendiğinde tüm satırları gizle
        $("#example3 tbody tr").hide();
    });

    $(document).ready(function () {
        // Arama yapıldığında satırları görüntülemek için
        $("#stokAdiSearch").on("keyup", function () {
            var value = $(this).val().toLowerCase();

            // Eğer arama alanı boşsa tüm satırları gizle
            if (value === "") {
                $("#example3 tbody tr").hide();
                $('#birimSearch').val('');
                $('#stokAdiSearch').val('');
                $('#miktar').val('');
            } else {
                // Arama ile eşleşen satırları göster
                $("#example3 tbody tr").filter(function () {
                    $(this).toggle($(this).find("td").eq(0).text().toLowerCase().indexOf(value) > -1);
                });
            }
        });

        // Sayfa yüklendiğinde tüm satırları gizle
        $("#example3 tbody tr").hide();
    });





    

    $(document).ready(function () {
        // Arama yapıldığında satırları görüntülemek için
        $("#birimSearch").on("keyup", function () {
            var value = $(this).val().toLowerCase();

            // Eğer arama alanı boşsa tüm satırları gizle
            if (value === "") {
                $("#example3 tbody tr").hide();
                $('#birimSearch').val('');
                $('#stokAdiSearch').val('');
                $('#miktar').val('');

            } else {
                // Arama ile eşleşen satırları göster
                $("#example3 tbody tr").filter(function () {
                    $(this).toggle($(this).find("td").eq(0).text().toLowerCase().indexOf(value) > -1);
                });
            }
        });

        // Sayfa yüklendiğinde tüm satırları gizle
        $("#example3 tbody tr").hide();
    });
    


    table1.on('click', 'tbody tr', (e) => {
        let classList = e.currentTarget.classList;

        if (classList.contains('selected')) {
            classList.remove('selected');
            selectedRow = null;
        } else {
            table1.rows('.selected').nodes().each((row) => row.classList.remove('selected'));
            classList.add('selected');
            selectedRow = table1.row(e.currentTarget);  // Seçili satırı kaydet
        }

        let stokkod = $(e.currentTarget).find('td').eq(0).text();
        let stokad = $(e.currentTarget).find('td').eq(1).text();
        let birim = $(e.currentTarget).find('td').eq(2).text();

        $('#stokKoduSearch').val(stokkod);
        $('#stokAdiSearch').val(stokad);
        $('#birimSearch').val(birim);

        // Burada sadece seçme işlemi yapılıyor
    });


    $('#addRowButton').click(function () {
        if (selectedRow) {
            let stokkod = $('#stokKoduSearch').val().trim();
            let stokad = $('#stokAdiSearch').val().trim();
            let birim = $('#birimSearch').val().trim();
            let miktar = $('#miktar').val().trim();

            miktar = Number(miktar);
            if (miktar === 0 || !Number.isFinite(miktar)) {
                // Miktar alanı boşsa modal'ı aç
                var myModal = new bootstrap.Modal(document.getElementById('miktarBosModal'));
                myModal.show();
                return; // Eğer miktar boşsa, aşağıdaki kodları çalıştırmadan çık.
            }



            let rowCount = $('#example4 tbody tr').length -1; // Mevcut satır sayısını al
            let newRowNumber = rowCount; // Yeni satır numarası

            // Yeni satırı tabloya ekle
            let newRow = `
                <tr>
                    <td>
                        <input type="text" class="input-field" name="FisHaraketleriModelDepo[${newRowNumber}].kpsft_StokKod" value="${stokkod}" readonly style="background-color: #dadada; box-sizing: border-box; width: 93%; border: none;">
                    </td>
                    <td>
                        <input type="text" class="input-field" name="FisHaraketleriModelDepo[${newRowNumber}].kpsft_stokAdi" value="${stokad}" readonly style="background-color: #dadada; box-sizing: border-box; width: 100%; border: none;">
                    </td>
                    <td>
                        <input type="text" class="input-field" name="FisHaraketleriModelDepo[${newRowNumber}].StokKartı.birimModel.kpsft_birimKodu" value="${birim}" readonly style="background-color: #dadada; box-sizing: border-box; width: 100%; border: none;">
                    </td>
                    <td>
                        <input type="number" class="input-field" name="FisHaraketleriModelDepo[${newRowNumber}].kpsft_Miktar" value="${miktar}" readonly style="box-sizing: border-box; width: 100%; border: none;">
                    </td>
                    <td>
                        <i style="margin-left:38%" class="fa-solid fa-trash deleteRow"></i>
                    </td>
                </tr>
                `;

            // Tabloya yeni satırı ekle
            $('#example4 tbody').append(newRow);

            $("#example3 tbody tr").hide();
            // Input alanlarını temizle
            $('#stokKoduSearch').val('');
            $('#stokAdiSearch').val('');
            $('#birimSearch').val('');
            $('#miktar').val('');
        }

        
    });

    
        
    $('.select-depoGELEN').click(function () {
        var selectedcariAd = $(this).data('value');
        var selectedcariKod = $(this).data('id');

        $('#kpsft_depoGELEN').val(selectedcariAd);
        $('#kpsft_depo_idGELEN').val(selectedcariKod);
    });

    $('.select-depoGİDEN').click(function () {
        var selectedcariAd = $(this).data('value');
        var selectedcariKod = $(this).data('id');

        $('#kpsft_depoGİDEN').val(selectedcariAd);
        $('#kpsft_depo_idGİDEN').val(selectedcariKod);
    });



    //enter ile veri ekleme

    $(document).keydown((e) => {
        if (e.key === 'Enter') {
            // Seçili satır varsa veri al
            if (selectedRow) {
                let data = selectedRow.data();

                let stokkod = $('#stokKoduSearch').val().trim();
                let stokad = $('#stokAdiSearch').val().trim();
                let birim = $('#birimSearch').val().trim();
                let miktar = $('#miktar').val().trim();

                miktar = Number(miktar);
                if (miktar === 0 || !Number.isFinite(miktar)) {
                    // Miktar alanı boşsa modal'ı aç
                    var myModal = new bootstrap.Modal(document.getElementById('miktarBosModal'));
                    myModal.show();
                    return; // Eğer miktar boşsa, aşağıdaki kodları çalıştırmadan çık.
                }


                // Yeni satırı tabloya ekle
                let rowCount = $('#example4 tbody tr').length -1; // Mevcut satır sayısını al
                let newRowNumber = rowCount; // Yeni satır numarası
              
                // Yeni satırı tabloya ekle
                let newRow = `
                <tr>
                    <td>
                        <input type="text" class="input-field" name="FisHaraketleriModelDepo[${newRowNumber}].kpsft_StokKod" value="${stokkod}" readonly style="background-color: #dadada; box-sizing: border-box; width: 93%; border: none;">
                    </td>
                    <td>
                        <input type="text" class="input-field" name="FisHaraketleriModelDepo[${newRowNumber}].kpsft_stokAdi" value="${stokad}" readonly style="background-color: #dadada; box-sizing: border-box; width: 100%; border: none;">
                    </td>
                    <td>
                        <input type="text" class="input-field" name="FisHaraketleriModelDepo[${newRowNumber}].StokKartı.birimModel.kpsft_birimKodu" value="${birim}" readonly style="background-color: #dadada; box-sizing: border-box; width: 100%; border: none;">
                    </td>
                    <td>
                        <input type="number" class="input-field" name="FisHaraketleriModelDepo[${newRowNumber}].kpsft_Miktar" value="${miktar}" readonly style="box-sizing: border-box; width: 100%; border: none;">
                    </td>
                    <td>
                        <i style="margin-left:38%" class="fa-solid fa-trash deleteRow"></i>
                    </td>
                </tr>
        `;
            
                // Tabloya yeni satırı ekle
                $('#example4 tbody').append(newRow);


                $("#example3 tbody tr").hide();
                $('#stokKoduSearch').val('');
                $('#stokAdiSearch').val('');
                $('#birimSearch').val('');
                $('#miktar').val('');

            }
        }
    });

});