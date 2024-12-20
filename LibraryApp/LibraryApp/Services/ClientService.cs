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
    public class ClientService : IEntityRepository<Client, ClientDto>
    {
        private GenericRepository<Client> _clientRepository;

        public ClientService(DbContext context)
        {
            _clientRepository = new GenericRepository<Client>(context);
        }

        public IQueryable<ClientDto> GetItems()
        {
            var clients = _clientRepository.Get();

            var clientDtos = clients.Select(client => new ClientDto
            {
                SurName = client.SurName,
                FirstName = client.FirstName,
                Patronomic = client.Patronomic,
                Adress = client.Adress,
                Books = client.Books.Select(book => new BookDto
                {
                    Name = book.Name,
                    Description = book.Description,
                    PublicationDate = book.PublicationDate,
                }).ToList(),
                Fines = client.Fines.Select(fine => new FineDto
                {
                    Cost = fine.Cost,
                    Description = fine.Description,
                    Title = fine.Title,
                }).ToList(),
                Sex = client.Sex,
                DateOfBirthday = client.DateOfBirthday,
            });

            return clientDtos;
        }

        public void AddItem(Client client)
        {
            Client newClient = new Client
            {
                SurName = client.SurName,
                FirstName = client.FirstName,
                Patronomic = client.Patronomic,
                Books = client.Books,
                Fines = client.Fines,
                Sex = client.Sex,
                Adress = client.Adress,
                PasportNum = client.PasportNum,
                PhoneNumber = client.PhoneNumber,
                DateOfBirthday = client.DateOfBirthday,
            };

            _clientRepository.Create(newClient);
        }

        public ClientDto GetItemdById(int id)
        {
            var client = _clientRepository.FindById(id);

            ClientDto clientDto = new ClientDto
            {
                SurName = client.SurName,
                FirstName = client.FirstName,
                Patronomic = client.Patronomic,
                Adress = client.Adress,
                Books = client.Books.Select(book => new BookDto
                {
                    Name = book.Name,
                    Description = book.Description,
                    PublicationDate = book.PublicationDate,
                }).ToList(),
                Fines = client.Fines.Select(fine => new FineDto
                {
                    Cost = fine.Cost,
                    Description = fine.Description,
                    Title = fine.Title,
                }).ToList(),
                Sex = client.Sex,
                DateOfBirthday = client.DateOfBirthday,
            };

            return clientDto;
        }

        public void RemoveItem(Client item)
        {
            _clientRepository.Remove(item);
        }

        public void UpdateItem(int id, Client updatedClient)
        {
            var existingClient = GetItemdById(id);

            if (existingClient != null)
            {
                Client newClient = new Client
                {
                    SurName = updatedClient.SurName,
                    FirstName = updatedClient.FirstName,
                    Patronomic = updatedClient.Patronomic,
                    Books = updatedClient.Books,
                    Fines = updatedClient.Fines,
                    Sex = updatedClient.Sex,
                    DateOfBirthday = updatedClient.DateOfBirthday,
                };

                _clientRepository.Update(newClient);
            }
        }
    }
}