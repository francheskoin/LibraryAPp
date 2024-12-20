using LibraryApp.Interfaces;
using LibraryApp.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Entity
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime PublicationDate { get; set; }

        public LibraryDto Library { get; set; }

        public SerialBooksDto SerialBook { get; set; }

        public Author Author { get; set; }

        public ClientDto Client { get; set; }
    }

}