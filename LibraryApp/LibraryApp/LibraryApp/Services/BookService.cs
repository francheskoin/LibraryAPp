using LibraryApp.Entity;
using LibraryApp.Interfaces;
using LibraryApp.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Services
{
    public class BookService : IEntityRepository<Book, BookDto>
    {
        private GenericRepository<Book> _bookRepository;

        public BookService(DbContext context)
        {
            _bookRepository = new GenericRepository<Book>(context);
        }

        public IQueryable<BookDto> GetItems()
        {
            var books = _bookRepository.Get();

            var bookDtos = books.Select(book => new BookDto
            {
                Name = book.Name,
                Description = book.Description,
                Author = new Author
                {
                    SurName = book.Author.SurName,
                },
                SerialBook = new SerialBooksDto
                {
                    Name = book.SerialBook.Name
                }

            });

            return bookDtos;
        }

            public void AddItem(Book book)
            {
                Book newBook = new Book
                {
                    Name = book.Name,
                    Description = book.Description,
                    Author = book.Author,
                    SerialBook = new SerialBooks
                    {
                        Name = book.SerialBook.Name
                    }
                };

                _bookRepository.Create(newBook);
            }

        public BookDto GetItemdById(int id)
        {
            var books = _bookRepository.FindById(id);

            BookDto bookDto = new BookDto
            {
                Name = books.Name,
                Description = books.Description,
                Author = books.Author,
                SerialBook = new SerialBooksDto
                {
                    Name = books.SerialBook.Name
                }

            };

            return bookDto;
        } 

        public void RemoveItem(Book item)
        {
            _bookRepository.Remove(item);
        }

        public void UpdateItem(int id, Book updatedBook)
        {
            var existingBook = GetItemdById(id);

            if (existingBook != null)
            {
                Book newBook = new Book
                {
                    Name = updatedBook.Name,
                    Description = updatedBook.Description,
                    Author = updatedBook.Author,
                    SerialBook = updatedBook.SerialBook,
                    PublicationDate = updatedBook.PublicationDate,
                };

             _bookRepository.Update(newBook);
                
            }
        }

    }
}
