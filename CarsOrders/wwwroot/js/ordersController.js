var ordersApp = angular.module("ordersApp", []);

ordersApp.service("orders", function ($http) {
    this.getCarsForSale = function ($http, scope) {
        $http.get("/orders/cars").then(function (response) {
                scope.cars = response.data;
                scope.hideSelectCar = false;
            }
        );
    };

    this.getOrders = function ($http, scope) {
        $http.get("/orders").then(function (response) {
                scope.orders = response.data;
                scope.loading = false;
                scope.showOrders = true;
            },
            function (response) {
                alert(getResponseText(response));
            })
    };

    this.addOrder = function ($http, order) {
        $http.post("/orders/add", order).then(function (response) {
                order.id = response.data.id;
                order.carInfo = response.data.carInfo;
            },
            function (response) {
                alert(getResponseText(response));
            });
    }

    this.deleteOrder = function ($http, scope, order) {
        var index = scope.orders.indexOf(order);
        if (index > -1) {
            $http.get("/orders/delete/" + order.id).then(function (response) {
                scope.orders.splice(index, 1);
            },
            function (response) {
                alert(getResponseText(response));
            });
        }
    };

    this.showStatistic = function ($http) {
        $http.get("/orders/statistic").then(function (response) {
            alert(response.data);
        },
            function (response) {
                alert(getResponseText(response));
            })
    }

    function getResponseText(response) {
        return "Error: " + response.status + " " + response.statusText + "!";
    }
});

ordersApp.controller("ordersController", function ($scope, $http, orders) {
    $scope.parent = { checkOut: '' };
    $scope.hideSelectCar = true;
    $scope.loading = true;
    $scope.showOrders = false;

    orders.getCarsForSale($http, $scope);
    orders.getOrders($http, $scope);

    $scope.addOrder = function (carID, date, number) {
        date = $("#orderdate").find("input").val();
        var d = parseDate(date);
        if (isOrderValid(number, carID, d)) {
            var order = { carID: carID, orderNumber: number, orderDate: date, carInfo: "Loading..." };
            $scope.orders.push(order);
            orders.addOrder($http, order);
        }
    };

    $scope.deleteOrder = function (order) {
        orders.deleteOrder($http, $scope, order);
    };

    $scope.showStatistic = function () {
        orders.showStatistic($http);
    };
});

function isOrderValid(number, carID, d) {
    return number && number !== "" && carID && carID !== "" && !isNaN(d);
}