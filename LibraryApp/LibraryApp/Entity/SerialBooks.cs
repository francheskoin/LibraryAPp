﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Entity
{
    public class SerialBooks
    {
        public int Id {  get; set; }

        public string Name { get; set; }

        public List<Book> Books { get; set;}

    }
}
