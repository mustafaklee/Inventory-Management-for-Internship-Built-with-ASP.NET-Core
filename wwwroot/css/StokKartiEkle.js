$(document).ready(function () {

    let table; // DataTable değişkenini tanımla
    let selectedRow = null; // Seçili satır değişkenini tanımla

    $('#klte').on('shown.bs.modal', function () {
        // Mevcut DataTable varsa yok et
        if ($.fn.DataTable.isDataTable('#kaliteTable')) {
            $('#kaliteTable').DataTable().destroy();
        }

        // DataTable'ı başlat
        table = $('#kaliteTable').DataTable({
            scrollY: false,
            scrollX: true,
            paging: true,
            searching: true,
            pageLength: 3,
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

        // Tablodaki bir satır seçildiğinde selectedRow değişkenine atama yapılır
        $('#kaliteTable tbody').on('click', 'tr', function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
                selectedRow = null;
            } else {
                table.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');
                selectedRow = table.row(this);
            }
        });
    });

    // Seç butonuna yalnızca bir kez olay dinleyici ekle
    $('#Seç').on('click', function () {
        if (selectedRow) {
            // Seçili satırdan veri al
            let kaliteno = selectedRow.data()[1] || "";
            let id = selectedRow.data()[0] || "";
            let kaliteKod = selectedRow.data()[2] || "";
            if (kaliteno && id) {
                // Kalite bilgilerini ilgili alanlara yaz
                $('#kpsft_kalite_kod').val(kaliteKod);
                $('#kpsft_kalite_no').val(kaliteno);
                $('#kpsft_kalite_id').val(id);
            }
            // Modal'ı kapat
            $('#klte').modal('hide');
        } else {
            alert('Lütfen incelemek için bir satır seçin.');
        }
    });

    // Tab içeriği ve linklerini başlat
    document.addEventListener("DOMContentLoaded", function () {
        var tabContents = document.querySelectorAll(".tabcontent");
        if (tabContents.length > 0) {
            tabContents[0].style.display = "block";
        }
        var tabLinks = document.querySelectorAll(".tablink");
        if (tabLinks.length > 0) {
            tabLinks[0].classList.add("active");
        }
    });

    $('.select-birim').click(function () {
        $('#kpsft_birim').val($(this).data('value'));
        $('#kpsft_birim_id').val($(this).data('id'));
    });

    $('.select-gtip').click(function () {
        $('#kpsft_gtipKodu').val($(this).data('value'));
        $('#kpsft_gtipKodu_id').val($(this).data('id'));
    });

    $('.select-satisKdv').click(function () {
        $('#kpsft_satisKdvOrani').val($(this).data('value'));
        $('#kpsft_satisKdvOrani_id').val($(this).data('id'));
    });

    $('.select-alisKdv').click(function () {
        $('#kpsft_alisKdvOrani').val($(this).data('value'));
        $('#kpsft_alisKdvOrani_id').val($(this).data('id'));
    });

    $('.select-grupKodu').click(function () {
        $('#kpsft_grupKodu').val($(this).data('value'));
        $('#kpsft_grupKodu_id').val($(this).data('id'));
        $('#kpsft_grupAdi').val($(this).data('ad'));
    });

    $('.select-stokKartTipi').click(function () {
        $('#kpsft_stokKartTipi').val($(this).data('value'));
        $('#kpsft_stokKartTipi_id').val($(this).data('id'));
    });

    $('#myForm').on('submit', function () {
        $('#checkboxValue').val($('#flexSwitchCheckChecked').is(':checked') ? '1' : '0');
    });

    document.getElementById('saveButton').addEventListener('click', function (event) {
        let alisKdvOrani = $('#kpsft_alisKdvOrani').val().trim();
        let satisKdvOrani = $('#kpsft_satisKdvOrani').val().trim();
        let gtipKodu = $('#kpsft_gtipKodu').val().trim();
        if (!alisKdvOrani || !satisKdvOrani || !gtipKodu) {
            event.preventDefault();
            new bootstrap.Modal(document.getElementById('uyarımodal')).show();
        }
    });

    const switchInput = $('#flexSwitchCheckChecked');
    const switchLabel = $('#switchLabel');
    const hiddenInput = $('#checkboxValue');

    updateLabel();
    switchInput.on('change', updateLabel);

    function updateLabel() {
        switchLabel.text(switchInput.is(':checked') ? 'Aktif' : 'Pasif');
        hiddenInput.val(switchInput.is(':checked') ? '1' : '0');
    }

    $('#generateCodeButton').on('click', function () {
        let boyut1 = $('#kpsft_boyut1').val().trim() || "0";
        let boyut2 = $('#kpsft_boyut2').val().trim() || "0";
        let boyut3 = $('#kpsft_boyut3').val().trim() || "0";
        let boyut4 = $('#kpsft_boyut4').val().trim() || "0";
        let boyut5 = $('#kpsft_boyut5').val().trim() || "0";
        let kaliteNO = $('#kpsft_kalite_no').val().trim();
        let kaliteAD = $('#kpsft_kalite_kod').val().trim();
        let grupKodu = $('#kpsft_grupKodu').val().trim();
        let grupAdi = $('#kpsft_grupAdi').val().trim();
        let uretimTuruId = $('select[name="kpsft_uretim_turu"]').val();
        let uretimTuruAd = $('select[name="kpsft_uretim_turu"] option:selected').text();

        if (grupAdi === "KÖŞEBENT DEMİR") {
            $('#kpsft_stokKodu').val(`${grupKodu}.${kaliteNO}.${boyut4}.${uretimTuruId}`);
            $('#kpsft_stokAdi').val(`${grupAdi} ${kaliteAD} ${boyut1}x${boyut2}x${boyut3}x${boyut4} ${uretimTuruAd}`);
        } else if (grupAdi === "KARE DEMİR") {
            $('#kpsft_stokKodu').val(`${grupKodu}.${kaliteNO}.${boyut4}.${uretimTuruId}`);
            $('#kpsft_stokAdi').val(`${grupAdi} ${kaliteAD} ${boyut1}x${boyut2}x${boyut4} ${uretimTuruAd}`);
        } else if (grupAdi === "LAMA DEMİR") {
            $('#kpsft_stokKodu').val(`${grupKodu}.${kaliteNO}.${boyut4}.${uretimTuruId}`);
            $('#kpsft_stokAdi').val(`${grupAdi} ${kaliteAD} ${boyut1}x${boyut3}x${boyut4} ${uretimTuruAd}`);
        } else if (grupAdi === "YUVARLAK DEMİR") {
            $('#kpsft_stokKodu').val(`${grupKodu}.${kaliteNO}.${boyut4}${uretimTuruId}`);
            $('#kpsft_stokAdi').val(`${grupAdi} ${kaliteAD} ${boyut4}x${boyut5} ${uretimTuruAd}`);
        } else if (grupAdi === "KÜTÜK DEMİR") {
            let boyut4Kod = boyut4 === "6000" ? "001" : "002";
            $('#kpsft_stokKodu').val(`${grupKodu}.${kaliteNO}.${boyut4Kod}`);
            $('#kpsft_stokAdi').val(`${grupAdi} ${kaliteAD} ${boyut1}x${boyut2}x${boyut4}`);
        }
    });

});

function openCity(evt, cityName) {
    var tabcontent = document.getElementsByClassName("tabcontent");
    for (let content of tabcontent) {
        content.style.display = "none";
    }
    var tablinks = document.getElementsByClassName("tablink");
    for (let link of tablinks) {
        link.className = link.className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}
