using LibraryApp.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Entity
{
    public class Client : LibraryPerson
    {

        public List<Book> Books { get; set; }

        public List<Fine> Fines { get; set; }

    }
}
