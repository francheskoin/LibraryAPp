using LibraryApp.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Entity
{
    public class ClientDto : LibraryPersonDto
    {

        public List<BookDto> Books { get; set; }

        public List<FineDto> Fines { get; set; }

    }
}
