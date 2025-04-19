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
    public class ReturnBookService : IReturnBookService
    {
        private GenericRepository<Client> _clientRepository;
        private GenericRepository<Book> _bookService;

        public ReturnBookService(DbContext context){
            _clientRepository = new GenericRepository<Client>(context);
            _bookService = new GenericRepository<Book>(context);
        }

        public string ReturnBook(string pasportNumber, int bookId)
        {

            var client = _clientRepository.Get().FirstOrDefault(c => c.PasportNum.ToString() == pasportNumber);

            if (client == null)
            {
                return "Клиент с указанными паспортными данными не найден.";
            }
            var bookDto = _bookService.FindById(bookId);

            if (bookDto == null)
            {
                return "Книга не найдена.";
            }


            client.Books.Remove(client.Books.FirstOrDefault(b => b.Id == bookDto.Id));

            _bookService.Update(bookDto);

            return $"Книга '{bookDto.Name}' успешно возвращена.";
        }
    }
}
