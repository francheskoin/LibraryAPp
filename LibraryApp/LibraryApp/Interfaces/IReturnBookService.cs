﻿using LibraryApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Interfaces
{
    public interface IReturnBookService
    {
        string ReturnBook(string pasportNumber, int bookId);
    }
}
