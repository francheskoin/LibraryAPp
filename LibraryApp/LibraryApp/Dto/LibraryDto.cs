using LibraryApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Entity
{
    public class LibraryDto
    {
        public string Name { get; set; }

        public string Adress { get; set; }

        public int PhoneNumber { get; set; }

        public List<EmployerDto> Employers { get; set; }

        public List<BookDto> Books { get; set; }

    }
}
