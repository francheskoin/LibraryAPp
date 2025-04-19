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
    public class FineService : IEntityRepository<Fine, FineDto>
    {
        private GenericRepository<Fine> _fineRepository;

        public FineService(DbContext context)
        {
            _fineRepository = new GenericRepository<Fine>(context);
        }

        public IQueryable<FineDto> GetItems()
        {
            var fines = _fineRepository.Get();

            var fineDtos = fines.Select(fine => new FineDto
            {
                Title = fine.Title,
                Description = fine.Description,
                Cost = fine.Cost.ToString(), 
                Client = new ClientDto
                {
                    FirstName = fine.Client.FirstName,
                    SurName = fine.Client.SurName,
                    Patronomic = fine.Client.Patronomic
                }
            });

            return fineDtos;
        }

        public void AddItem(Fine fine)
        {
            Fine newFine = new Fine
            {
                Title = fine.Title,
                Description = fine.Description,
                Cost = fine.Cost,
                Client = fine.Client
            };

            _fineRepository.Create(newFine);
        }

        public FineDto GetItemdById(int id)
        {
            var fine = _fineRepository.FindById(id);

            FineDto fineDto = new FineDto
            {
                Title = fine.Title,
                Description = fine.Description,
                Cost = fine.Cost.ToString(),
                Client = new ClientDto
                {
                    FirstName = fine.Client.FirstName,
                    SurName = fine.Client.SurName,
                    Patronomic = fine.Client.Patronomic
                }
            };

            return fineDto;
        }

        public void RemoveItem(Fine item)
        {
            _fineRepository.Remove(item);
        }

        public void UpdateItem(int id, Fine updatedFine)
        {
            var existingFine = GetItemdById(id);

            if (existingFine != null)
            {
                Fine newFine = new Fine
                {
                    Title = updatedFine.Title,
                    Description = updatedFine.Description,
                    Cost = updatedFine.Cost,
                    Client = updatedFine.Client
                };

                _fineRepository.Update(newFine);
            }
        }
    }
}
