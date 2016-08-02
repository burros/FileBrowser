fileBrowserApp.controller('folderBrowserCtrl', function ($scope, webService) {
    $scope.findFolder = function (id) {
        console.log(webService);
        webService.getFolderInfo(id, $scope.isRecursion.text)
      .success(function (response) {
          $scope.currentFolder = response;
          if ($scope.currentFolder.CurrentPath === 'Root') {
              $scope.isNotRoot.text = false;
          } else {
              $scope.isNotRoot.text = true;
          }
      })
      .error(function (a, b) {
          alert("Upps.. we have error code: " + b );
      });
    }
    $scope.isRecursion = {
        text: 'false'
    };
    $scope.isNotRoot = {
        text: true
    };

    $scope.findFolder('');

});
