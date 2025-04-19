using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Entity
{   

    public class FineDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Cost { get; set; }

        public ClientDto Client { get; set; }

    }
}
