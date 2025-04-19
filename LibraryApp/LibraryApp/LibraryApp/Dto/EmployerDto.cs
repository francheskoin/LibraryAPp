using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Entity
{
    public class EmployerDto : PersonDto
    {
        public LibraryDto Library { get; set; }

        public JobTitleDto JobTitle { get; set; }

    }
}
