//testul 3 - vue.js cu timer

var appUrl = "https://localhost:44313";

window.onload = function () {
    var appT3 = new Vue({
        el: "#appT3",
        data: {
            intrebari: [],
            indexIntrebare: 0,
            raspunsUtilizator: [],
            lungimeIntrebari: 0,
            lungimeTest:0,
            durata: 0,
            timp:0,
            timerActiv: false,
            timer: null,
            esteFinalizat: false,
            testulAFostDat: null,
            nrIntrebari: 0,
            notaObtinuta: 0,
            punctaj: 0
        },
        created: function () {
            this.getEsteTerminat();
            if (!this.testulAFostDat)
                this.getIntrebari();
        },
        filters: {
            charIndex: function (i) {
                return String.fromCharCode(97 + i);
            }
        },
        methods: {
            getIntrebari: function () { //generare intrebari pentru testul curent
                $.ajax({
                    contentType: "application/json",
                    dataType: "json",
                    url: appUrl+"/GetTestul3",
                    method: "get",
                    success: result => {
                        var _intrebari = [];
                        for (var i = 0; i < result.length; i++) {
                            var text = result[i].text;
                            var responses = result[i].responses.split('/');
                            var raspunsCorect = result[i].raspunsCorect;
                            _intrebari.push({ text: text, responses: responses, raspunsCorect: raspunsCorect });
                        }
                        this.intrebari = _intrebari;
                        this.lungimeIntrebari = result[0].lungimeIntrebari;
                        this.lungimeTest = result[0].lungimeTest;
                        this.durata = result[0].durata;
                        userResponseSkelaton = Array(this.intrebari.length).fill(null);
                        this.raspunsUtilizator = userResponseSkelaton;
                        this.startTimer();
                    },
                    error: err => console.log('Eroare la obtinerea intrebarilor')
                });
            },
            getEsteTerminat: function () { //verificam daca testul a fost sau nu dat anterior
                $.ajax({
                    contentType: "application/json",
                    dataType: "json",
                    url: appUrl+"/GetEsteTerminat?idTest=3",
                    method: "get",
                    success: result => {
                        this.testulAFostDat = result.testulAFostDat;
                        this.nrIntrebari = result.lungimeTest == 0 ? 10 : 20;
                        this.notaObtinuta = result.notaObtinuta;
                        this.punctaj = this.nrIntrebari * this.notaObtinuta / 10;
                    },
                    error: err => console.log('Eroare la determinarea statusului testului')
                });
            },
            //restart: function () {
            //    this.indexIntrebare = 0;
            //    this.raspunsUtilizator = Array(this.intrebari.length).fill(null);
            //},
            selectareOptiune: function (index) {  //selectare raspuns de catre utilizator
                Vue.set(this.raspunsUtilizator, this.indexIntrebare, index);
            },
            next: function () { //trecerea la intrebarea urmatoare
                if (this.indexIntrebare < this.intrebari.length && this.raspunsUtilizator[this.indexIntrebare] != null) {
                    this.indexIntrebare++;
                    this.stopTimer();
                    this.startTimer();
                }
            },
            startTimer: function() { //pornire timer
                this.timerActiv = true;
                this.timp = this.durata;
                if (!this.timer) {
                    this.timer = setInterval(() => {
                        if (this.timp > 0) {
                            this.timp--;
                        } else {
                            this.indexIntrebare++;
                            this.stopTimer();
                            this.startTimer();                          
                        }
                    }, 1000)
                }
            },
            stopTimer: function () { //oprire timer
                this.timerActiv = false;
                clearInterval(this.timer);
                this.timer = null;
            },
            scor: function () { //calculare punctaj obtinut
                var scor = 0;
                for (let i = 0; i < this.raspunsUtilizator.length; i++) {
                    if (
                        typeof this.intrebari[i].responses[
                        this.raspunsUtilizator[i]
                        ] !== "undefined" &&
                        this.intrebari[i].responses[this.raspunsUtilizator[i]] == this.intrebari[i].raspunsCorect
                    ) {
                        scor = scor + 1;
                    }
                }
                return scor;
            },
            salvareRezultat: function () { //salvare rezultat in baza de date
                if (this.raspunsUtilizator[this.indexIntrebare] != null)
                    this.esteFinalizat = true;

                var durataTest = this.durata == 15 ? 0 : 1;

                //trimitere rezultat catre controller pentru a fi adaugat in baza de date
                $.ajax({
                    contentType: "application/json",
                    dataType: "json",
                    url: appUrl+"/SalvareRezultat?test=3&scor=" + this.scor() + "&NrIntrebari=" + this.intrebari.length + "&LungimeTest=0&LungimeIntrebari=" + this.lungimeIntrebari + "&Durata=" + durataTest,
                    method: "POST",
                    success: result => {
                        console.log("Rezultatul a fost adaugat in baza de date!");
                    },
                    error: err => console.log('Eroare la salvarea rezultatului!')
                });
            }
        }
    });
}

