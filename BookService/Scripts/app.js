var ViewModel = function () {
    var self = this;
    self.books = ko.observableArray();
    self.error = ko.observable();

    self.detail = ko.observable();

    self.authors = ko.observableArray();

    var booksUri = '/api/books/';

    function ajaxHelper(uri, method, data) {
        self.error(''); // Clear error message
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    function getAllBooks() {
        ajaxHelper(booksUri, 'GET').done(function (data) {
            self.books(data);
        });
    }

    self.getDetail = function(currentBook) {
        var currentUri = booksUri + currentBook.Id;
        ajaxHelper(currentUri, 'GET').done(function (data) {
            self.detail(data);
        });

        function getAllAuthors() {
            ajaxHelper(authorUrl, 'GET').done(function (data) {
                self.authors(data);
            });
        }
    }

    self.newBook = {
        Author: ko.observable(),
        Genre: ko.observable(),
        Title: ko.observable(),
        Price: ko.observable(),
        Year: ko.observable()
    }

    var authorUrl = '/api/authors/';

    self.addNewBook = function() {
        var book = {
            AuthorId: self.newBook.Author().Id,
            Title: self.newBook.Title(),
            Genre: self.newBook.Genre(),
            Year: self.newBook.Year(),
            Price: self.newBook.Price()
        }
        ajaxHelper(booksUri, 'POST').done(function (data) {
            self.books(data);
        });
    }

    // Fetch the initial data.
        getAllBooks();
        getAllAuthors();
};

ko.applyBindings(new ViewModel());