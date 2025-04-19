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
    public class AuthorService : IEntityRepository<Author, AuthorDto>
    {
        private GenericRepository<Author> _authorRepository;

        public AuthorService(DbContext context)
        {
            _authorRepository = new GenericRepository<Author>(context);
        }

        public IQueryable<AuthorDto> GetItems()
        {
            var authors = _authorRepository.Get();

            var authorDtos = authors.Select(author => new AuthorDto
            {
                SurName = author.SurName,
                FirstName = author.FirstName,
                Patronomic = author.Patronomic,
                Description = author.Description,
                Books = author.Books.Select(book => new BookDto
                {
                    Name = book.Name,
                    Description = book.Description,
                    PublicationDate = book.PublicationDate,
                }).ToList(),
                Sex = author.Sex,
                DateOfBirthday = author.DateOfBirthday,
            });

            return authorDtos;
        }


        public void AddItem(Author author)
        {
            Author newAuthor = new Author
            {
                SurName = author.SurName,
                FirstName = author.FirstName,
                Patronomic = author.Patronomic,
                Description = author.Description,
                Books = author.Books,

                Sex = author.Sex,
                DateOfBirthday = author.DateOfBirthday,
            };

        _authorRepository.Create(newAuthor);
        }

        public AuthorDto GetItemdById(int id)
        {
            var author = _authorRepository.FindById(id);

            AuthorDto authorDto = new AuthorDto
            {
                SurName = author.SurName,
                FirstName = author.FirstName,
                Patronomic = author.Patronomic,
                Description = author.Description,
                Books = author.Books.Select(book => new BookDto
                {
                    Name = book.Name,
                    Description = book.Description,
                    PublicationDate = book.PublicationDate,
                }).ToList(),
                Sex = author.Sex,
                DateOfBirthday = author.DateOfBirthday,
            };

            return authorDto;
        } 

        public void RemoveItem(Author item)
        {
            _authorRepository.Remove(item);
        }

        public void UpdateItem(int id, Author updateAuthor)
        {
            var existingAuthor = GetItemdById(id);

            if (existingAuthor != null)
            {
                Author newAuthor = new Author
                {
                    SurName = updateAuthor.SurName,
                    FirstName = updateAuthor.FirstName,
                    Patronomic = updateAuthor.Patronomic,
                    Description = updateAuthor.Description,
                    Sex = updateAuthor.Sex,
                    DateOfBirthday = updateAuthor.DateOfBirthday,

                };

                _authorRepository.Update(newAuthor);

            }
        }

    }
}
