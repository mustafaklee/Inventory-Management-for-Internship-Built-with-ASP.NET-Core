const table = new DataTable('#exampleUserList', {
    scrollY: false,
    scrollX: false,
    paging: true,
    searching: true,
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

document.querySelector('#sil').addEventListener('click', function () {
    if (selectedRow) {
        let userName = selectedRow.data()[0];  // İlk sütundaki veriyi al, userName olarak varsayalım
        if (userName) {
            window.location.href = '/Login/Sil?userName=' + userName;
        }
    } else {
        alert('Lütfen silmek için bir satır seçin.');
    }
});


document.querySelector('#duzenle').addEventListener('click', function () {
    if (selectedRow) {
        let userName = selectedRow.data()[1];  // İlk sütundaki veriyi al, userName olarak varsayalım
        if (userName) {
            window.location.href = '/Login/Register?userName=' + userName;
        }
    } else {
        alert('Lütfen düzenlemek için bir satır seçin.');
    }
});


document.querySelector('#incele').addEventListener('click', function () {
    if (selectedRow) {
        let stokKodu = selectedRow.data()[0];  // İlk sütundaki veriyi al, userName olarak varsayalım
        if (stokKodu) {
            window.location.href = '/Admin/StokKartiIncele?stokKodu=' + stokKodu;
        }
    } else {
        alert('Lütfen incelemek için bir satır seçin.');
    }
});


