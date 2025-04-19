using LibraryApp.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Entity
{
    public class Book
    {
        public int Id {  get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime PublicationDate { get; set; }

        public Author Author { get; set; }

        public Library Library { get; set; }

        public SerialBooks SerialBook { get; set; }

        public Client Client { get; set; }


    }
    
}
