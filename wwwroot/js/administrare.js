var appUrl = "https://localhost:44313";

var idIntrebare = 0;

var tabel = null;

function getIntrebari() {
    $.ajax({
        url: appUrl + '/GetIntrebari',
        contentType: 'application/json',
        type: 'GET',
        cache: false,
        success: function (data) {
            if (tabel != null)
                tabel.destroy()
            tabel = $('#tabelIntrebari').DataTable({
                data: data,
                columns: [
                    { data: "id", title: "Id"},
                    { data: "text", title: "Intrebare"},
                    { data: "responses", title: "Variante de raspuns"},
                    { data: "raspunsCorect", title: "Raspuns Corect"},
                    { data: "idTest", title: "Id Test" },
                    { data: "lungimeIntrebari", title: "Lungime"},
                    { data: "alternareComplexitate", title: "Complexitate"}
                ],
                columnDefs: [
                    {
                        data: null,
                        render: function (data, type, row) {
                            return `<button class="btn btn-sm mb-0 btn-info fa fa-edit" data-target='#addEditModal' data-tooltip='tooltip' title='Info' onclick='editareIntrebare(${data.id},"${data.text}","${data.responses}","${data.raspunsCorect}","${data.idTest}","${data.lungimeIntrebari}","${data.alternareComplexitate}")'></button>
                                    <button class="btn btn-sm mb-0 btn-danger fa fa-trash" data-target='#deleteModal' data-tooltip='tooltip' title='Delete' onclick='stergereIntrebare(${data.id},"${data.text}","${data.responses}","${data.raspunsCorect}","${data.idTest}","${data.lungimeIntrebari}","${data.alternareComplexitate}")'></button>`
                        },
                        targets: 7
                    },
                    { width: "5%", targets: 0 },
                    { width: "30%", targets: 1 },
                    { width: "20%", targets: 2 },
                    { width: "15%", targets: 3 },
                    { width: "5%", targets: 4 },
                    { width: "5%", targets: 5 },
                    { width: "10%", targets: 6 },
                    { width: "10%", targets: 7 }
                ],
                "paging": true,
                "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
                "sorting": true,
                "ordering": true,
                "order":[],
                "scrollX": true,
                "scrollY": true,
                "scrollCollapse": true
            });
        },
        error: function (xhr) {
            console.log("Eroare la obtinerea intrebarilor");
        }
    });
}

function showModal1() {
    $('#addEditModal').show();
    $('.intrebariContainer').addClass('modalBlur');
}
function hideModal1() {
    $('#addEditModal').hide();
    $('.intrebariContainer').removeClass('modalBlur');
    idIntrebare = 0;
    getIntrebari();
}

function showModal2() {
    $('#deleteModal').show();
    $('.intrebariContainer').addClass('modalBlur');
}
function hideModal2() {
    $('#deleteModal').hide();
    $('.intrebariContainer').removeClass('modalBlur');
    idIntrebare = 0;
    getIntrebari();
}

$("#addEditModal").on('hide', function () {
    $('.intrebariContainer').removeClass('modalBlur');
});
$("#deleteModal").on('hide', function () {
    $('.intrebariContainer').removeClass('modalBlur');
});

function adaugareIntrebare() {
    showModal1();
    $('#inputIntrebare').val('');
    $('#inputVariante').val('');
    $('#inputRaspuns').val('');
    $('#idtest').val(0);
    $('#lungime').val(0);
    $('#dificultate').val(0);
    idIntrebare = 0;
}

function editareIntrebare(id, intrebare,variante,raspuns,idtest,lungime,complexitate) {
    showModal1();
    $('#inputIntrebare').val(intrebare);
    $('#inputVariante').val(variante);
    $('#inputRaspuns').val(raspuns);
    $('#idtest').val(idtest);
    $('#lungime').val(lungime);
    $('#dificultate').val(complexitate);
    idIntrebare = id;
}

function stergereIntrebare(id, intrebare, variante, raspuns, idtest, lungime, complexitate) {
    showModal2();
    idIntrebare = id;
}

function salvareIntrebare() {
    var intrebare = $('#inputIntrebare').val();
    var varianteRaspuns = $('#inputVariante').val();
    var raspunsCorect = $('#inputRaspuns').val();
    var idTest = $('#idtest').val();
    var lungime = $('#lungime').val();
    var dificultate = $('#dificultate').val();
    var adaugareEditare = idIntrebare == 0 ? 0 : 1;
    $.ajax({
        url: appUrl + '/SalvareIntrebare?adaugareEditare='+adaugareEditare+'&id='+idIntrebare+'&intrebare=' + intrebare + '&variante=' + varianteRaspuns + '&raspuns=' + raspunsCorect
            +'&idtest='+idTest+'&lungime='+lungime+'&dificultate='+dificultate,
        contentType: 'application/json',
        type: 'POST',
        cache: false,
        success: function (data) {
            hideModal1();
        },
        error: function (xhr) {
            alert("A aparut o eroare");
            console.log("Eroare la salvarea intrebarii");
        }
    });
}

function confirmareStergere() {
    $.ajax({
        url: appUrl + '/StergereIntrebare?id=' + idIntrebare,
        contentType: 'application/json',
        type: 'POST',
        cache: false,
        success: function (data) {
            hideModal2();
        },
        error: function (xhr) {
            alert("A aparut o eroare");
            console.log("Eroare la stergerea intrebarii");
        }
    });
}

$(document).ready(function () {

    getIntrebari();

});

