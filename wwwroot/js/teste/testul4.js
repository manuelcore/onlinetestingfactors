//testul 4 - angular.js cu timer

var appUrl = "https://apptestare.azurewebsites.net/";

var appT4 = angular.module('ngAppTestul4', ['ngSanitize']
).config(function () {

}).run(function () {
});

appT4.filter('charIndex', function () {
    return function (x) {
        return String.fromCharCode(97 + x);
    };
});

appT4.controller('controllerTestul4', function ($scope, $http, $interval) {
    var vm = this;
    var timerInterval;
    vm.raspunsUtilizator = [];
    vm.indexIntrebare = 0;
    vm.lungimeIntrebari = 0;
    vm.lungimeTest = 0;
    vm.intrebari = [];
    vm.durata = 0;
    vm.timp = 0;
    vm.timerActiv = false;
    vm.timer = null;
    vm.esteFinalizat = false;
    vm.testulAFostDat = null;
    vm.nrIntrebari = 0;
    vm.notaObtinuta = 0;
    vm.punctaj = 0;
    //verificam daca testul a fost sau nu dat anterior
    $http(
        {
            method: 'GET',
            url: appUrl+'/GetEsteTerminat?idTest=4',
        }).then(function (result) {
            vm.testulAFostDat = result.data.testulAFostDat;
            vm.nrIntrebari = result.data.lungimeTest == 0 ? 10 : 20;
            vm.notaObtinuta = result.data.notaObtinuta;
            vm.punctaj = vm.nrIntrebari * vm.notaObtinuta / 10;
        });
    if (!vm.testulAFostDat) { //generare intrebari pentru testul curent
        $http(
            {
                method: 'GET',
                url: appUrl+'/GetTestul4',
            }).then(function (result) {
                var _intrebari = [];
                for (var i = 0; i < result.data.length; i++) {
                    var text = result.data[i].text;
                    var responses = result.data[i].responses.split('/');
                    var raspunsCorect = result.data[i].raspunsCorect;
                    _intrebari.push({ text: text, responses: responses, raspunsCorect: raspunsCorect });
                }
                vm.intrebari = _intrebari;
                vm.lungimeIntrebari = result.data[0].lungimeIntrebari;
                vm.lungimeTest = result.data[0].lungimeTest;
                vm.durata = result.data[0].durata;
                var userResponseSkelaton = Array(vm.intrebari.length).fill(null);
                vm.raspunsUtilizator = userResponseSkelaton;
                vm.startTimer();
            });
    }
    vm.selecteaza = function (nrIntrebare, nrRaspuns, raspunsUtilizator) { //selectare raspuns de catre utilizator
        vm.raspunsUtilizator[nrIntrebare] = nrRaspuns;
        vm.allowNav = true;
    };
    vm.next = function () { //trecerea la intrebarea urmatoare
        vm.allowNav = false;
        if (vm.raspunsUtilizator[vm.indexIntrebare] !== null && vm.indexIntrebare < vm.intrebari.length) {
            vm.indexIntrebare++;
            vm.stopTimer();
            vm.startTimer();
        }
    };
    vm.startTimer= function() { //pornire timer
        vm.timerActiv = true;
        vm.timp = vm.durata;
        if (!vm.timer) {
                vm.timer = function () {
                    if (vm.timp > 0) {
                        vm.timp--;
                    } else {
                        vm.indexIntrebare++;
                        vm.stopTimer();
                        vm.startTimer();
                    }
            }
            timerInterval=$interval(vm.timer, 1000);  
        }
    };
    vm.stopTimer= function () { //oprire timer
        vm.timerActiv = false;
        $interval.cancel(timerInterval);
        vm.timer = null;
    };
    vm.scor = function () { //calculare punctaj obtinut
        var scor = 0;
        for (let i = 0; i < vm.raspunsUtilizator.length; i++) {
            if (
                typeof vm.intrebari[i].responses[
                vm.raspunsUtilizator[i]
                ] !== "undefined" &&
                vm.intrebari[i].responses[vm.raspunsUtilizator[i]] == vm.intrebari[i].raspunsCorect
            ) {
                scor = scor + 1;
            }
        }
        return scor;
    };
    vm.salvareRezultat = function () { //salvare rezultat in baza de date
        vm.allowNav = false;
        if (vm.raspunsUtilizator[vm.indexIntrebare] !== null) {
            allowNav = true;
            vm.esteFinalizat = true;
        }

        var durataTest = vm.durata == 15 ? 0 : 1;

        //trimitere rezultat catre controller pentru a fi adaugat in baza de date
        $.ajax({
            contentType: "application/json",
            dataType: "json",
            url: appUrl+"/SalvareRezultat?test=4&scor=" + vm.scor() + "&NrIntrebari=" + vm.intrebari.length + "&LungimeTest=0&LungimeIntrebari=" + vm.lungimeIntrebari + "&Durata=" + durataTest,
            method: "POST",
            success: result => {
                console.log("Rezultatul a fost adaugat in baza de date!");
            },
            error: err => console.log('Eroare la salvarea rezultatului!')
        });
    };
});