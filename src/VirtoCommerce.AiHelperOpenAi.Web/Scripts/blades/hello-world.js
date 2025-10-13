angular.module('VirtoCommerce.AiHelperOpenAi')
    .controller('VirtoCommerce.AiHelperOpenAi.helloWorldController', ['$scope', 'VirtoCommerce.AiHelperOpenAi.webApi', function ($scope, api) {
        var blade = $scope.blade;
        blade.title = 'AiHelperOpenAi';

        blade.refresh = function () {
            api.get(function (data) {
                blade.title = 'AiHelperOpenAi.blades.hello-world.title';
                blade.data = data.result;
                blade.isLoading = false;
            });
        };

        blade.refresh();
    }]);
