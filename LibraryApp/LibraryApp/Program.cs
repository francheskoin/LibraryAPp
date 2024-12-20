using LibraryApp.Entity;
using LibraryApp.Services;
using System;

namespace LibraryApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new Context())
            {
                BookService bookservice = new BookService(context);

                var books = bookservice.GetItems();

                foreach (var book in books) 
                { 
                    string authorInfo = book.Author != null ? $"{book.Author.SurName} {book.Author.FirstName}" : "Автор не указан";

                    string serialBookInfo = book.SerialBook != null ? book.SerialBook.Name : "Серия не указана";

                    Console.WriteLine($"Название книги: {book.Name}.\nОписание книги: {book.Description}\nАвтор книги: {authorInfo}\nСерия книг: {serialBookInfo}\n");
                }
            }

        }
    }
}

