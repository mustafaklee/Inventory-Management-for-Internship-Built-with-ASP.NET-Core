// DataTable yapılandırması
const table = new DataTable('#example', {
    scrollY: false,
    scrollX: true,
    paging: true,
    searching: true,
    pageLength: 7,
    keys: true,
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

// Satır tıklama olayı
table.on('click', 'tbody tr', (e) => {
    let classList = e.currentTarget.classList;

    if (classList.contains('selected')) {
        classList.remove('selected');
        selectedRow = null;
    } else {
        table.rows('.selected').nodes().each((row) => row.classList.remove('selected'));
        classList.add('selected');
        selectedRow = table.row(e.currentTarget);  // Seçili satırı kaydet
    }
});

//document.querySelector('#sil').addEventListener('click', function () {
//    if (selectedRow) {
//        let cariKodu = selectedRow.data()[0];
//        if (cariKodu) {
//            window.location.href = '/Admin/cariKartiSil?cariKodu=' + cariKodu;
//        }
//    } else {
//        alert('Lütfen silmek için bir satır seçin.');
//    }
//});


document.querySelector('#duzenle').addEventListener('click', function () {
    console.log("girdim");
    if (selectedRow) {
        let fisno = selectedRow.data()[0];
        if (fisno) {
            window.location.href = '/Admin/CariGirisFisiDuzenle?fisno=' + fisno;
        }
    } else {
        alert('Lütfen düzenlemek için bir satır seçin.');
    }
});

// Modal onay butonuna tıklama
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

// Arama işlemleri
$("#fisNoSearch").on("keyup", function () {
    var value = $(this).val().toLowerCase();
    $("#example tbody tr").filter(function () {
        $(this).toggle($(this).find("td").eq(0).text().toLowerCase().indexOf(value) > -1);
    });
});

$("#tarihSearch").on("keyup", function () {
    var value = $(this).val().toLowerCase();
    $("#example tbody tr").filter(function () {
        $(this).toggle($(this).find("td").eq(1).text().toLowerCase().indexOf(value) > -1);
    });
});

$("#fisturuSearch").on("keyup", function () {
    var value = $(this).val().toLowerCase();
    $("#example tbody tr").filter(function () {
        $(this).toggle($(this).find("td").eq(2).text().toLowerCase().indexOf(value) > -1);
    });
});


$("#cariSearch").on("keyup", function () {
    var value = $(this).val().toLowerCase();
    $("#example tbody tr").filter(function () {
        $(this).toggle($(this).find("td").eq(2).text().toLowerCase().indexOf(value) > -1);
    });
});


$("#depoSearch").on("keyup", function () {
    var value = $(this).val().toLowerCase();
    $("#example tbody tr").filter(function () {
        $(this).toggle($(this).find("td").eq(2).text().toLowerCase().indexOf(value) > -1);
    });
});
