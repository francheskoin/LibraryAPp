using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Entity
{   

    public class Fine
    {
        public int Id {  get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Cost { get; set; }

        public Client Client { get; set; }

    }
}
