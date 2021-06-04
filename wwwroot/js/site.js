var appUrl = "https://apptestare.azurewebsites.net/";

var testeApp = angular.module('testeApp', []);
    testeApp.controller('testeController', function ($scope, $http) {
        $scope.titlu = "Lista teste";
        $scope.teste = []; 
        $http(
            {
                method: 'GET',
                url: appUrl+'/Teste',
            }).then(function (response) {
                for (var i = 0; i < response.data.length;i++)
                    $scope.teste.push({ id: response.data[i].id, nume: response.data[i].nume });
            })
    });
