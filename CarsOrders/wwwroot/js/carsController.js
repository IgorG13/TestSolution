var carsApp = angular.module("carsApp", []);

 carsApp.service("cars", function ($http) {
    this.getCars = function ($http, scope) {
        $http.get("/cars").then(
        function (response) {
            scope.cars = response.data;
            scope.showCars = true;
            scope.loading = false;
        },
        function (response) {
            alert(getResponseText(response));
        });
    };
    this.addCar = function ($http, scope, car) {
        $http.post("/cars/add/" + scope.key, car).then(function (response) {
            car.id = response.data.id
        },
            function (response) {
                failResponse(response, getResponseText(response) + " Car " + car.model + " was not added!");
                deleteCarFromList(scope, car);
            });
    };
    this.deleteCar = function ($http, scope, car) {
        $http.get("/cars/delete/" + car.id + "?key=" + scope.key).then(function (response) {
            deleteCarFromList(scope, car);
        }, 
            function (response) {
                failResponse(response, getResponseText(response) + " Car " + car.model + " was not deleted!");
            });
    };

    function deleteCarFromList(scope, car) {
        var index = scope.cars.indexOf(car);
        if (index > -1) {
            scope.cars.splice(index, 1);
        }
    }

    function failResponse(response, defaultError) {
        switch (response.status) {
            case 401:
                alert("Security key is wrong!");
                break;
            default:
                alert(defaultError);
        }
    }

    function getResponseText(response) {
        return "Error: " + response.status + " " + response.statusText + "!";
    }
});

carsApp.controller("carsController", function ($scope, $http, cars) {
    $scope.cars = [];
    $scope.loading = true;
    $scope.showCars = false;
    cars.getCars($http, $scope);

    $scope.addCar = function (key, carModel, engine, price) {
        price = parseFloat(price);
        engine = parseFloat(engine);
        var car = { model: carModel, price: price, engineCapacity: engine, id: "" };
        if (isCarValid(car)) {
            $scope.cars.push(car);
            cars.addCar($http, $scope, car);
        }
    };

    $scope.deleteCar = function (car) {
        cars.deleteCar($http, $scope, car)
    };

    function isCarValid(car) {
        return car !== null && car.model !== "" && !isNaN(car.price) && !isNaN(car.engineCapacity);
    }
});