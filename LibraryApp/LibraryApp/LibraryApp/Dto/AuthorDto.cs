using LibraryApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Entitys
{
    public class AuthorDto : PersonDto
    {

        public string Description {  get; set; }

        public List<BookDto> Books { get; set; }

    }
}
