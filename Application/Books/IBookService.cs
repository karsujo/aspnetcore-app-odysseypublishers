﻿using System.Collections.Generic;

namespace Application.Books
{
    public interface IBookService
    {
        BookDto GetBook(string Id);

        IEnumerable<BookDto> GetBooks();

        IEnumerable<BookDto> GetBooks(IEnumerable<string> Ids);

    }
}