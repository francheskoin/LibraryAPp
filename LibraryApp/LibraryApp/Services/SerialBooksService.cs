using LibraryApp.Entity;
using LibraryApp.Entitys;
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
    public class SerialBooksService : IEntityRepository<SerialBooks, SerialBooksDto>
    {
        private GenericRepository<SerialBooks> _serialBooksRepository;

        public SerialBooksService(DbContext context)
        {
            _serialBooksRepository = new GenericRepository<SerialBooks>(context);
        }

        public IQueryable<SerialBooksDto> GetItems()
        {
            var serialBooks = _serialBooksRepository.Get();

            var serialBooksDtos = serialBooks.Select(serialBook => new SerialBooksDto
            {
                Name = serialBook.Name,
                Books = serialBook.Books.Select(book => new BookDto
                {
                    Name = book.Name,               
                    Description = book.Description,  
                    PublicationDate = book.PublicationDate, 
                    Library = new LibraryDto         
                    {
                        Name = book.Library.Name,
                        Adress = book.Library.Adress,
                        PhoneNumber = Convert.ToInt32(book.Library.PhoneNumber), 
                    },
                    SerialBook = new SerialBooksDto  
                    {
                        Name = book.SerialBook.Name
                    }
                }).ToList()
            });

            return serialBooksDtos;
        }

        public void AddItem(SerialBooks serialBooks)
        {
            SerialBooks newSerialBook = new SerialBooks
            {
                Name = serialBooks.Name,
                Books = serialBooks.Books
            };

            _serialBooksRepository.Create(newSerialBook);
        }

        public SerialBooksDto GetItemdById(int id)
        {
            var serialBooks = _serialBooksRepository.FindById(id);

            SerialBooksDto serialBooksDto = new SerialBooksDto
            {
                Name = serialBooks.Name,
                Books = serialBooks.Books.Select(book => new BookDto
                {
                    Name = book.Name,               
                    Description = book.Description,  
                    PublicationDate = book.PublicationDate, 
                    Library = new LibraryDto         
                    {
                        Name = book.Library.Name,
                        Adress = book.Library.Adress,
                        PhoneNumber = Convert.ToInt32(book.Library.PhoneNumber), 
                    },
                    SerialBook = new SerialBooksDto  
                    {
                        Name = book.SerialBook.Name
                    }
                }).ToList()
            };

            return serialBooksDto;
        }

        public void RemoveItem(SerialBooks item)
        {
            _serialBooksRepository.Remove(item);
        }

        public void UpdateItem(int id, SerialBooks updatedSerialBooks)
        {
            var existingSerialBooks = GetItemdById(id);

            if (existingSerialBooks != null)
            {
                SerialBooks newSerialBook = new SerialBooks
                {
                    Name = updatedSerialBooks.Name,
                    Books = updatedSerialBooks.Books
                };

                _serialBooksRepository.Update(newSerialBook);
            }
        }
    }
}