using LibraryApp.Entity;
using LibraryApp.Interfaces;
using LibraryApp.Services;
using System;
using System.Linq;

namespace LibraryApp.Services
{
    public class GetBookService : IGetBookService
    {
        private BookService _bookService;

        public GetBookService(BookService bookService)
        {
            _bookService = bookService;
        }

        public void DisplayAvailableBooks()
        {
            var availableBooks = _bookService.GetItems().Where(b => b.IsAvailable).ToList();
            Console.WriteLine("Доступные книги:");
            foreach (var book in availableBooks)
            {
                Console.WriteLine($"ID: {book.Id}, Название: {book.Name}, Автор: {book.Author.SurName}");
            }
        }

        public string TakeBook(Client client, int bookId)
        {
            var bookDto = _bookService.GetItemdById(bookId);

            if (bookDto == null)
            {
                return "Книга не найдена.";
            }

            if (bookDto.IsAvailable == false)
            {
                return "Эта книга уже была взята.";
            }

            var book = new Book
            {
                Id = bookDto.Id,
                Name = bookDto.Name,
                Author = bookDto.Author,
                Description = bookDto.Description,
                PublicationDate = bookDto.PublicationDate,
                Client = client
            };
            client.Books.Add(book);

            bookDto.IsAvailable = false;
            _bookService.UpdateItem(book.Id, book);

            return $"Книга '{book.Name}' успешно взята.";
        }
    }
}