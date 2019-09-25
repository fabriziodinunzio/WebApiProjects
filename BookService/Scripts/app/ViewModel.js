var ViewModel = function () {
    var self = this;
    var uri = 'api/books/';
    self.books = ko.observableArray();
    self.error = ko.observable();
    function AjaxHelper(uri, method, data) {
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'application/json',
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    };

    function getAllBooks() {
        AjaxHelper(uri, 'GET').done(function (data) {
            self.books(data);
        })
    }

    getAllBooks();
};

ko.applyBindings(new ViewModel());