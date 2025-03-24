$(document).ready(function () {
    // Gümrük Tip Kodu Seçme İşlemi
    $('.select-birim').click(function () {
        var selectedBirim = $(this).data('value');
        var selectedBirimId = $(this).data('id');
        $('#kpsft_birim').val(selectedBirim);
        $('#kpsft_birim_id').val(selectedBirimId);
    });

    $('.select-gtip').click(function () {
        var selectedGtipKodu = $(this).data('value');
        var selectedGtipKoduId = $(this).data('id');
        $('#kpsft_gtipKodu').val(selectedGtipKodu);
        $('#kpsft_gtipKodu_id').val(selectedGtipKoduId);
    });

    // Satış KDV Oranı Seçme İşlemi
    $('.select-satisKdv').click(function () {
        var selectedSatisKdvOrani = $(this).data('value');
        var selectedSatisKdvOraniId = $(this).data('id');
        $('#kpsft_satisKdvOrani').val(selectedSatisKdvOrani);
        $('#kpsft_satisKdvOrani_id').val(selectedSatisKdvOraniId);
    });

    // Alış KDV Oranı Seçme İşlemi
    $('.select-alisKdv').click(function () {
        var selectedAlisKdvOrani = $(this).data('value');
        var selectedAlisKdvOraniId = $(this).data('id');
        $('#kpsft_alisKdvOrani').val(selectedAlisKdvOrani);
        $('#kpsft_alisKdvOrani_id').val(selectedAlisKdvOraniId);
    });

    // Kalite Seçme İşlemi
    $('.select-kalite').click(function () {
        var selectedKalite = $(this).data('value');
        var selectedKaliteId = $(this).data('id');
        $('#kpsft_kalite').val(selectedKalite);
        $('#kpsft_kalite_id').val(selectedKaliteId);
    });

    // Grup Kodu Seçme İşlemi
    $('.select-grupKodu').click(function () {
        var selectedGrupKodu = $(this).data('value');
        var selectedGrupKoduId = $(this).data('id');
        $('#kpsft_grupKodu').val(selectedGrupKodu);
        $('#kpsft_grupKodu_id').val(selectedGrupKoduId);
    });

    // Stok Kart Tipi Seçme İşlemi
    $('.select-stokKartTipi').click(function () {
        var selectedStokKartTipi = $(this).data('value');
        var selectedStokKartTipiId = $(this).data('id');
        $('#kpsft_stokKartTipi').val(selectedStokKartTipi);
        $('#kpsft_stokKartTipi_id').val(selectedStokKartTipiId);
    });

    
});

// Sekme işlevselliği
function openCity(evt, cityName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}


document.addEventListener('DOMContentLoaded', function () {
    const switchInput = document.getElementById('flexSwitchCheckChecked');
    const switchLabel = document.getElementById('switchLabel');
    const hiddenInput = document.getElementById('checkboxValue');

    // Sayfa yüklendiğinde checkbox'ın durumuna göre etiket güncellenir
    updateLabel();

    switchInput.addEventListener('change', function () {
        updateLabel();
    });

    function updateLabel() {
        if (switchInput.checked) {
            switchLabel.textContent = 'Aktif';
            hiddenInput.value = 1;
        } else {
            switchLabel.textContent = 'Pasif';
            hiddenInput.value = 0;  // Gizli input'a pasif değeri atanır
        }
    }
});
