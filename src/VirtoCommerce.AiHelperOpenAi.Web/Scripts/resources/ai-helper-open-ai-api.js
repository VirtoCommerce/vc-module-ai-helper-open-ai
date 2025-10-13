angular.module('VirtoCommerce.AiHelperOpenAi')
    .factory('VirtoCommerce.AiHelperOpenAi.webApi', ['$resource', function ($resource) {
        return $resource('api/ai-helper-open-ai');
    }]);
