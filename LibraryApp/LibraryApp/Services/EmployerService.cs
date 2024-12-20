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
    public class EmployerService : IEntityRepository<Employer, EmployerDto>
    {
        private GenericRepository<Employer> _employerRepository;
        private GenericRepository<Library> _libraryRepository;

        public EmployerService(DbContext context)
        {
            _employerRepository = new GenericRepository<Employer>(context);
            _libraryRepository = new GenericRepository<Library>(context);
        }

        public IQueryable<EmployerDto> GetItems()
        {
            var employers = _employerRepository.Get();

            var employerDtos = employers.Select(employer => new EmployerDto
            {
                SurName = employer.SurName,
                FirstName = employer.FirstName,
                Patronomic = employer.Patronomic,
                Sex = employer.Sex,
                DateOfBirthday = employer.DateOfBirthday,
            });

            return employerDtos;
        }

        public void AddItem(Employer employer)
        {
            Employer newEmployer = new Employer
            {
                FirstName = employer.FirstName,
                SurName = employer.SurName,
                Patronomic = employer.Patronomic,
                Sex = employer.Sex,
                DateOfBirthday = employer.DateOfBirthday,
                Library = employer.Library,
                JobTitle = employer.JobTitle
            };

            _employerRepository.Create(newEmployer);
        }

        public EmployerDto GetItemdById(int id)
        {
            var employer = _employerRepository.FindById(id);

            EmployerDto employerDto = new EmployerDto
            {
                SurName = employer.SurName,
                FirstName = employer.FirstName,
                Patronomic = employer.Patronomic,
                Sex = employer.Sex,
                DateOfBirthday = employer.DateOfBirthday,
            };

            return employerDto;
        }

        public void RemoveItem(Employer item)
        {
            _employerRepository.Remove(item);
        }

        public void UpdateItem(int id, Employer updatedEmployer)
        {
            var existingEmployer = GetItemdById(id);

            if (existingEmployer != null)
            {
                Employer newEmployer = new Employer
                {
                    FirstName = updatedEmployer.FirstName,
                    SurName = updatedEmployer.SurName,
                    Patronomic = updatedEmployer.Patronomic,
                    Sex = updatedEmployer.Sex,
                    DateOfBirthday = updatedEmployer.DateOfBirthday,
                    Library = updatedEmployer.Library,
                    JobTitle = updatedEmployer.JobTitle
                };

                _employerRepository.Update(newEmployer);
            }
        }
    }
}