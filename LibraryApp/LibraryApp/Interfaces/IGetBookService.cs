using LibraryApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Interfaces
{
    public interface IGetBookService
    {
        void DisplayAvailableBooks();
        string TakeBook(Client client, int bookId);
    }
}
