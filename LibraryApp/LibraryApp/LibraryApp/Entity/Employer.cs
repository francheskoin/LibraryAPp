using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Entity
{
    public class Employer : Person
    {
        public Library Library { get; set; }

        public JobTitle JobTitle { get; set; }

    }
}
