using LibraryApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Entitys
{
    public abstract class LibraryPersonDto : PersonDto
    {
        public int PasportNum { get; set; }

        public string PhoneNumber { get; set; }

        public string Adress { get; set; }
    }
}
