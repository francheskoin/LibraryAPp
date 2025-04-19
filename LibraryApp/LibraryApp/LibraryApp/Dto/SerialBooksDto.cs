using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Entity
{
    public class SerialBooksDto
    {
        public string Name { get; set; }

        public List<BookDto> Books { get; set;}

    }
}
