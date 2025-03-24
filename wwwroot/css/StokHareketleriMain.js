const table = new DataTable('#example5', {
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
        selectedRow = table.row(e.currentTarget);  // Seçili satırı kaydet
    }
});





document.querySelector('#incele').addEventListener('click', function () {
    if (selectedRow) {
        let stokKodu = selectedRow.data()[0];  // İlk sütundaki veriyi al, stokKodu olarak varsayalım
        if (stokKodu) {
            window.location.href = '/Admin/StokHareketleri?stokKodu=' + stokKodu;
        }
    } else {
        alert('Lütfen incelemek için bir satır seçin.');
    }
});


//search kısmı
$(document).ready(function () {
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
});

    $(document).ready(function () {
        $(".column-search").on("keyup", function () {
            // Arama yapılan input'un indeksini al (hangi sütunda olduğunu bulur)
            var columnIndex = $(this).closest("th").index();

            // Input'a girilen değeri küçük harf olarak al
            var value = $(this).val().toLowerCase();

            // Tablodaki ilgili sütunda arama yap
            $("#example tbody tr").filter(function () {
                $(this).toggle($(this).find("td").eq(columnIndex).text().toLowerCase().indexOf(value) > -1);
            });
        });
    });
