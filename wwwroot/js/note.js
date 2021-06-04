var appUrl = "https://localhost:44313";

var note = [];

var tabel = null;

function getNote() {
    $.ajax({
        url: appUrl + '/GetNote',
        contentType: 'application/json',
        type: 'GET',
        cache: false,
        success: function (data) {
            if (tabel != null)
                tabel.destroy()
            tabel = $('#tabelNote').DataTable({
                data: data,
                columns: [
                    { data: "student", title: "Student" },
                    { data: "idTest", title: "Id Test" },
                    { data: "nota", title: "Nota" }
                ],
                "paging": true,
                "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
                "sorting": true,
                "ordering": true,
                "order": [],
                "scrollY": true,
                "scrollCollapse": true
            });
        },
        error: function (xhr) {
            console.log("Eroare la obtinerea notelor");
        }
    });
}

$(document).ready(function () {

    getNote();

});