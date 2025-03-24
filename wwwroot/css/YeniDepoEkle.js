

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

document.getElementById('flexSwitchCheckChecked').addEventListener('change', function () {
    // Gizli input alanına checkbox'ın durumunu atama
    document.getElementById('checkboxValue').value = this.checked ? 'true' : 'false';
});


$(document).ready(function () {
    $('#myForm').on('submit', function () {
        var isChecked = $('#flexSwitchCheckChecked').is(':checked');
        $('#checkboxValue').val(isChecked ? '1' : '0');
    });
});

$(document).ready(function () {
    $('.select-depoSorumlu').click(function () {
        var sorumluID = $(this).data('value');
        $('#kpsft_depoSorumlusu').val(sorumluID);
        console.log(sorumluID);
    });
});

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