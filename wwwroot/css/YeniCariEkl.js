$(document).ready(function () {

    $('.select-vergiD').click(function () {
        var selectedVkodu = $(this).data('id');
        var selectedVAdi = $(this).data('value');

        $('#kpsft_vergiDairesi').val(selectedVkodu);
        $('#kpsft_vergiDairesi_id').val(selectedVAdi);
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




//$(document).ready(function () {
//    $('#generateCodeButton').on('click', function () {
//        let boyut1 = $('#kpsft_boyut1').val().trim();

//        let uretimTuruId = $('select[name="kpsft_cariTipi"]').val();
//        let uretimTuruId = $('select[name="kpsft_yurticiDisi"]').val();


//        if (grupAdi == "KÖŞEBENT DEMİR") {
//            let stokKodu = `${grupKodu}.${kaliteNO}.${boyut4}.${uretimTuruId}`;
//            let stokAdi = `${grupAdi} ${kaliteAD} ${boyut1}x${boyut2}x${boyut3}x${boyut4} ${uretimTuruAd}`;
//            $('#kpsft_stokKodu').val(stokKodu);
//            $('#kpsft_stokAdi').val(stokAdi);
//        }

//    });
//});