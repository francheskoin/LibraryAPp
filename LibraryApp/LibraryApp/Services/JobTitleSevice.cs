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
    public class JobTitleService : IEntityRepository<JobTitle, JobTitleDto>
    {
        private GenericRepository<JobTitle> _jobTitleRepository;

        public JobTitleService(DbContext context)
        {
            _jobTitleRepository = new GenericRepository<JobTitle>(context);
        }

        public IQueryable<JobTitleDto> GetItems()
        {
            var jobTitles = _jobTitleRepository.Get();

            var jobTitleDtos = jobTitles.Select(jobTitle => new JobTitleDto
            {
                Name = jobTitle.Name,
                Description = jobTitle.Description,
                Employers = jobTitle.Employers.Select(employer => new EmployerDto
                {
                    FirstName = employer.FirstName,
                    SurName = employer.SurName,
                    Patronomic = employer.Patronomic,
                    Sex = employer.Sex,
                    DateOfBirthday = employer.DateOfBirthday,
                }).ToList()
            });

            return jobTitleDtos;
        }

        public void AddItem(JobTitle jobTitle)
        {
            JobTitle newJobTitle = new JobTitle
            {
                Name = jobTitle.Name,
                Description = jobTitle.Description,
                Employers = jobTitle.Employers
            };

            _jobTitleRepository.Create(newJobTitle);
        }

        public JobTitleDto GetItemdById(int id)
        {
            var jobTitle = _jobTitleRepository.FindById(id);

            JobTitleDto jobTitleDto = new JobTitleDto
            {
                Name = jobTitle.Name,
                Description = jobTitle.Description,
                Employers = jobTitle.Employers.Select(employer => new EmployerDto
                {
                    FirstName = employer.FirstName,
                    SurName = employer.SurName,
                    Patronomic = employer.Patronomic,
                    Sex = employer.Sex,
                    DateOfBirthday = employer.DateOfBirthday,
                }).ToList()
            };

            return jobTitleDto;
        }

        public void RemoveItem(JobTitle item)
        {
            _jobTitleRepository.Remove(item);
        }

        public void UpdateItem(int id, JobTitle updatedJobTitle)
        {
            var existingJobTitle = GetItemdById(id);

            if (existingJobTitle != null)
            {
                JobTitle newJobTitle = new JobTitle
                {
                    Name = updatedJobTitle.Name,
                    Description = updatedJobTitle.Description,
                    Employers = updatedJobTitle.Employers
                };

                _jobTitleRepository.Update(newJobTitle);
            }
        }
    }
}