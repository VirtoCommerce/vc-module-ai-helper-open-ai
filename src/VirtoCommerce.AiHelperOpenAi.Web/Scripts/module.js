// Call this to register your module to main application
var moduleName = 'VirtoCommerce.AiHelperOpenAi';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .config(['$stateProvider',
        function ($stateProvider) {
            $stateProvider
                .state('workspace.AiHelperOpenAiState', {
                    url: '/ai-helper-open-ai',
                    templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                    controller: [
                        'platformWebApp.bladeNavigationService',
                        function (bladeNavigationService) {
                            var newBlade = {
                                id: 'blade1',
                                controller: 'VirtoCommerce.AiHelperOpenAi.helloWorldController',
                                template: 'Modules/$(VirtoCommerce.AiHelperOpenAi)/Scripts/blades/hello-world.html',
                                isClosingDisabled: true,
                            };
                            bladeNavigationService.showBlade(newBlade);
                        }
                    ]
                });
        }
    ])
    .run(['platformWebApp.mainMenuService', '$state',
        function (mainMenuService, $state) {
            //Register module in main menu
            var menuItem = {
                path: 'browse/ai-helper-open-ai',
                icon: 'fa fa-cube',
                title: 'AiHelperOpenAi',
                priority: 100,
                action: function () { $state.go('workspace.AiHelperOpenAiState'); },
                permission: 'ai-helper-open-ai:access',
            };
            mainMenuService.addMenuItem(menuItem);
        }
    ]);
