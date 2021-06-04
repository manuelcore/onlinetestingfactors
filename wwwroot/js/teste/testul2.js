//testul 2 - angular.js

var appUrl = "https://localhost:44313";

var appT2 = angular.module('ngAppTestul2', ['ngSanitize']
).config(function () {

}).run(function () {
});

appT2.filter('charIndex', function () {
    return function (x) {
        return String.fromCharCode(97 + x);
    };
});

appT2.controller('controllerTestul2', function ($scope, $http) {
    var vm = this;
    vm.raspunsUtilizator = [];
    vm.indexIntrebare = 0;
    vm.lungimeTest = 0;
    vm.lungimeIntrebari = 0;
    vm.alternareComplexitate = 0;
    vm.intrebari = [];
    vm.testulAFostDat = null;
    vm.nrIntrebari = 0;
    vm.notaObtinuta = 0;
    vm.punctaj = 0,
        vm.esteFinalizat = false;
    //verificam daca testul a fost sau nu dat anterior
    $http(
        {
            method: 'GET',
            url: appUrl+'/GetEsteTerminat?idTest=2',
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
                url: appUrl+'/GetTestul2',
            }).then(function (result) {
                var _intrebari = [];
                for (var i = 0; i < result.data.length; i++) {
                    var text = result.data[i].text;
                    var responses = result.data[i].responses.split('/');
                    var raspunsCorect = result.data[i].raspunsCorect;
                    _intrebari.push({ text: text, responses: responses, raspunsCorect: raspunsCorect });
                }
                vm.intrebari = _intrebari;
                vm.lungimeTest = result.data[0].lungimeTest;
                vm.lungimeIntrebari = result.data[0].lungimeIntrebari;
                vm.alternareComplexitate = result.data[0].alternareComplexitate;
                var userResponseSkelaton = Array(vm.intrebari.length).fill(null);
                vm.raspunsUtilizator = userResponseSkelaton;
            });
    }
    vm.selecteaza = function (nrIntrebare, nrRaspuns, raspunsUtilizator) { //selectare raspuns de catre utilizator
        vm.raspunsUtilizator[nrIntrebare]=nrRaspuns;
        vm.allowNav = true;
    };
    vm.next = function () { //trecere la urmatoarea intrebare
        vm.allowNav = false;
        if (vm.raspunsUtilizator[vm.indexIntrebare] !== null && vm.indexIntrebare<vm.intrebari.length) 
            vm.indexIntrebare++;
        //} else {
        //    vm.allowNav = false;
        //}
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
      
        //trimitere rezultat catre controller pentru a fi adaugat in baza de date
        $.ajax({
            contentType: "application/json",
            dataType: "json",
            url: appUrl+"/SalvareRezultat?test=2&scor=" + this.scor() + "&NrIntrebari=" + this.intrebari.length + "&LungimeIntrebari=" + this.lungimeIntrebari + "&LungimeTest=" + this.lungimeTest + "&AlternareComplexitate=" + this.alternareComplexitate,
            method: "POST",
            success: result => {
                console.log("Rezultatul a fost adaugat in baza de date!");
            },
            error: err => console.log('Eroare la salvarea rezultatului!')
        });
    };
});