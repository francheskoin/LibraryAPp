using LibraryApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Entity
{
    public class Author : Person
    {
        public string Description {  get; set; }

        public List<Book> Books { get; set; }

    }
}
