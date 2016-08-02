fileBrowserApp.factory('webService', function ($http) {

    this.getFolderInfo = function (id, isRecursion) {
        return $http({
            method: 'GET',
            contentType: "application/json; charset=utf-8",
            url: 'http://localhost:64154/api/FileBroswer?id=' + id + '&isRecursion=' + isRecursion
        });
    }
    return this;
});