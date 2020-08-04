﻿using AutoMapper;
using System;
using System.Collections.Generic;

namespace Application.Books
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _mapper = mapper;
            _bookRepository = bookRepository;

        }

        public IEnumerable<BookDto> GetBooksForAuthor(string authorId)
        {
            var res = _bookRepository.GetBooksForAuthor(authorId);
            return _mapper.Map<IEnumerable<BookDto>>(res);
        }

        public BookDto GetBookForAuthor(string bookId)
        {
            var res = _bookRepository.GetBookForAuthor(bookId);
            //res.Validate();
            return _mapper.Map<BookDto>(res);
        }

       
        public IEnumerable<BookDto> GetBooks(BookResourceParameters bookResourceParameters)
        {
            var res = _bookRepository.GetBooks(bookResourceParameters);
            //validate
            return _mapper.Map<List<BookDto>>(res);
        }

        public IEnumerable<BookDto> GetBooks()
        {
            var res = _bookRepository.GetBooks();
            //validate
            return _mapper.Map<List<BookDto>>(res);
        }


        public IEnumerable<BookDto> CreateBooks(IEnumerable<BookForCreationDto> bookForCreationDtos, string authorId)
        {
            foreach( var book in bookForCreationDtos )
            {
                _bookRepository.CreateBook(book);
            }
   
            return _mapper.Map<List<BookDto>>(bookForCreationDtos);
        }
    }
}
