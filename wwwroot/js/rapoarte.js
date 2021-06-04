var appUrl = "https://localhost:44313";

var rapoarteApp = angular.module('rapoarteApp', ['ngSanitize']
).config(function () {

}).run(function () {
});

var factor = "", valoriFactor = [];
var medieNote = [];
var nrNoteMaxime = [0, 0];
var notaMaxima = 0;
var raportSelectat = "lt1";

rapoarteApp.controller('rapoarteController', function ($scope, $http) {
    var vm = this;
    vm.afisareRapoarte = false;
    vm.raportSelectat = "lt1";
    vm.notaMaxima = 0;
    vm.generareRapoarte = function () {
        vm.afisareRapoarte = true;
        factor = "";
        valoriFactor = [];
        medieNote = [];
        nrNoteMaxime = [0, 0];
        notaMaxima = 0;
        raportSelectat = vm.raportSelectat;
        $http(
        {
            method: 'GET',
                url: appUrl+'/GetRapoarte?test=' + vm.raportSelectat.charAt(2)+'&tipRaport='+vm.raportSelectat,
            }).then(function (response) {
                factor = response.data[0].factor;
                var valoriDuplicate = [];
                for (var i = 0; i < response.data.length; i++) {
                    valoriDuplicate.push({ valoriFactor: response.data[i].valoare, nota: response.data[i].rezultat });             
                }
                for (var i = 0; i < valoriDuplicate.length; i++)
                    if (valoriDuplicate[i].nota > notaMaxima)
                        notaMaxima = valoriDuplicate[i].nota;
                var suma1=0, suma2=0, lungime1=0, lungime2=0;
                for (var i = 0; i < valoriDuplicate.length; i++) {
                    if (valoriDuplicate[i].valoriFactor == 0) {
                        valoriFactor.push(0);
                        suma1 += valoriDuplicate[i].nota;
                        lungime1++;
                        if (valoriDuplicate[i].nota == 10)
                            nrNoteMaxime[0]++;
                    }
                    else if (valoriDuplicate[i].valoriFactor == 1){
                        valoriFactor.push(1);
                        suma2 += valoriDuplicate[i].nota;
                        lungime2++;
                        if (valoriDuplicate[i].nota == notaMaxima)
                            nrNoteMaxime[1]++;
                    }
                }
                vm.notaMaxima = notaMaxima;

                medieNote.push(suma1 / lungime1);
                medieNote.push(suma2 / lungime2);

                desenareChart();
            })
    }
    
});

var chartGeneral, chartNotaMaxima;
var ctx1, ctx2;

function desenareChart() {
    ctx1 = document.getElementById('chartGeneral').getContext('2d');
    if (chartGeneral != null) {
        chartGeneral.destroy();
    }
    chartGeneral = new Chart(ctx1, {
        type: 'bar',
        data: {
            labels: raportSelectat.indexOf("lt") != -1 ? ["Lungime standard (10 intrebari)", "Lungime extinsa (20 intrebari)"] :
                raportSelectat.indexOf("li") != -1 ?
                    (raportSelectat.indexOf("3") != -1 || raportSelectat.indexOf("4") != -1 ?
                        ["Intrebari scurte, dar clare", "Intrebari lungi, dar detaliate"] : ["Succesiune intrebari scurte - lungi", "Succesiune intrebari lungi - scurte"]):
                    ["Succesiune intrebari usoare - grele", "Succesiune intrebari grele - usoare"],
            datasets: [{
                label: 'Media notelor',
                data: medieNote,
                fill: false,
                backgroundColor: [
                    'rgb(255, 99, 132)',
                    'rgb(54, 162, 235)'
                ],
                tension: 0.1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
    ctx2 = document.getElementById('chartNotaMaxima').getContext('2d');
    if (chartNotaMaxima != null) {
        chartNotaMaxima.destroy();
    }
    chartNotaMaxima = new Chart(ctx2, {
        type: 'doughnut',
        data: {
            labels: raportSelectat.indexOf("lt") != -1 ? ["Lungime standard (10 intrebari)", "Lungime extinsa (20 intrebari)"] :
                raportSelectat.indexOf("li") != -1 ?
                    (raportSelectat.indexOf("3") != -1 || raportSelectat.indexOf("4") != -1 ?
                        ["Intrebari scurte, dar clare", "Intrebari lungi, dar detaliate"] : ["Succesiune intrebari scurte - lungi", "Succesiune intrebari lungi - scurte"] ) :
                    ["Succesiune intrebari usoare - grele", "Succesiune intrebari grele - usoare"],
            datasets: [{
                label: 'Numar de note maxime - '+notaMaxima,
                data: nrNoteMaxime,
                fill: false,
                backgroundColor: [
                    'rgb(255, 99, 132)',
                    'rgb(54, 162, 235)'
                    ],
                tension: 0.1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}

$(document).ready(function () {
   


});