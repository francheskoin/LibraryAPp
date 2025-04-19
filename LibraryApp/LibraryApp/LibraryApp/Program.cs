using LibraryApp.Entity;
using LibraryApp.Services;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System;

namespace LibraryApp
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                AnsiConsole.Clear();
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Что вы хотите сделать?")
                        .AddChoices(new[]
                        {
                        "Показать все книги",
                        "Передать книгу клиенту",
                        "Забрать книгу у клиента",
                        "Поиск по имени",
                        "Добавить книгу",
                        "Обновить книгу",
                        "Удалить книгу",
                        "Выйти"
                        }));

                switch (choice)
                {
                    case "Показать все книги":
                        ShowAllBooks();
                        break;
                    case "Поиск по имени":
                        SearchBookByName();
                        break;
                    case "Передать книгу клиенту":
                        GiveBookToClient();
                        break;
                    case "Забрать книгу у клиента":
                        ReturnBookFromClient();
                        break;
                    case "Добавить книгу":
                        AddBook();
                        break;
                    case "Обновить книгу":
                        UpdateBook();
                        break;
                    case "Удалить книгу":
                        DeleteBook();
                        break;
                    case "Выйти":
                        return;
                }

                AnsiConsole.MarkupLine("\n[gray]Нажмите любую клавишу, чтобы продолжить...[/]");
                Console.ReadKey();
            }
        }

        static void ShowAllBooks()
        {
            using var db = new Context();

            var books = db.Books
                .Include(b => b.Author)
                .Include(b => b.Library)
                .ToList();

            var table = new Table()
                .Border(TableBorder.Rounded)
                .Title("[yellow]Список книг[/]")
                .AddColumn("ID")
                .AddColumn("Название")
                .AddColumn("Автор")
                .AddColumn("Дата публикации")
                .AddColumn("Доступна");

            foreach (var book in books)
            {
                string authorName = book.Author != null
                    ? $"{book.Author.SurName} {book.Author.FirstName} {book.Author.Patronomic}"
                    : "Неизвестно";

                table.AddRow(
                    book.Id.ToString(),
                    book.Name,
                    authorName,
                    book.PublicationDate.ToShortDateString(),
                    book.IsAvailable ? "Да" : "Нет");
            }

            AnsiConsole.Write(table);
        }


        static void SearchBookByName()
        {
            var name = AnsiConsole.Ask<string>("Введите часть названия книги:");
            using var db = new Context();

            var books = db.Books
                .Include(b => b.Author)
                .Where(b => EF.Functions.ILike(b.Name, $"%{name}%"))
                .ToList();

            if (!books.Any())
            {
                AnsiConsole.MarkupLine("[red]Книги не найдены[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Minimal)
                .AddColumn("ID")
                .AddColumn("Название")
                .AddColumn("Автор");

            foreach (var book in books)
            {
                string authorName = book.Author != null
                    ? $"{book.Author.SurName} {book.Author.FirstName} {book.Author.Patronomic}"
                    : "—";

                table.AddRow(
                    book.Id.ToString(),
                    book.Name,
                    authorName);
            }

            AnsiConsole.Write(table);
        }

        static void AddBook()
        {
            using var db = new Context();

            var name = AnsiConsole.Ask<string>("Введите название книги:");
            var description = AnsiConsole.Ask<string>("Введите описание:");

            var publicationDate = AnsiConsole.Ask<DateTime>("Введите дату публикации (yyyy-MM-dd):");
            publicationDate = DateTime.SpecifyKind(publicationDate, DateTimeKind.Local).ToUniversalTime();

            // Показ авторов
            AnsiConsole.MarkupLine("\n[yellow]Список авторов:[/]");
            var authors = db.Authors.ToList();
            foreach (var a in authors)
            {
                AnsiConsole.MarkupLine($"[grey]{a.Id}[/]: {a.SurName} {a.FirstName} {a.Patronomic}");
            }
            var authorId = AnsiConsole.Ask<int>("Введите [green]ID автора[/]:");

            // Показ клиентов
            AnsiConsole.MarkupLine("\n[yellow]Список клиентов:[/]");
            var clients = db.Clients.ToList();
            foreach (var c in clients)
            {
                AnsiConsole.MarkupLine($"[grey]{c.Id}[/]: {c.SurName} {c.FirstName} {c.Patronomic}");
            }
            var clientId = AnsiConsole.Ask<int?>("Введите [green]ID клиента[/] (или нажмите Enter, если книга свободна):");

            // Показ библиотек
            AnsiConsole.MarkupLine("\n[yellow]Список библиотек:[/]");
            var libraries = db.Libraries.ToList();
            foreach (var l in libraries)
            {
                AnsiConsole.MarkupLine($"[grey]{l.Id}[/]: {l.Name}");
            }
            var libraryId = AnsiConsole.Ask<int>("Введите [green]ID библиотеки[/]:");

            // Показ сериалов (серий книг)
            AnsiConsole.MarkupLine("\n[yellow]Список серий книг:[/]");
            var serials = db.SerialsBooks.ToList();
            foreach (var s in serials)
            {
                AnsiConsole.MarkupLine($"[grey]{s.Id}[/]: {s.Name}");
            }
            var serialBookId = AnsiConsole.Ask<int>("Введите [green]ID серии книги[/]:");

            // Поиск сущностей
            var author = db.Authors.Find(authorId);
            var library = db.Libraries.Find(libraryId);
            var client = clientId.HasValue ? db.Clients.Find(clientId.Value) : null;
            var serialBook = db.SerialsBooks.Find(serialBookId);

            if (author == null || library == null || serialBook == null || (clientId.HasValue && client == null))
            {
                AnsiConsole.MarkupLine("[red]Ошибка: один из указанных ID не найден.[/]");
                return;
            }

            // Создание книги
            var book = new Book
            {
                Name = name,
                Description = description,
                PublicationDate = publicationDate,
                Author = author,
                Library = library,
                SerialBook = serialBook,
                Client = client,
                IsAvailable = client == null
            };

            db.Books.Add(book);
            db.SaveChanges();

            AnsiConsole.MarkupLine("[green]Книга успешно добавлена![/]");


        }

        static void UpdateBook()
        {
            using var db = new Context();

            // Выводим все книги с ID в таблице
            AnsiConsole.MarkupLine("\n[yellow]Список всех книг:[/]");

            var books = db.Books.Include(b => b.Author).Include(b => b.Library).Include(b => b.SerialBook).ToList();

            // Создание таблицы для вывода книг
            var table = new Table()
                .Border(TableBorder.Minimal)
                .AddColumn("ID")
                .AddColumn("Название")
                .AddColumn("Автор")
                .AddColumn("Библиотека");

            // Заполнение таблицы
            foreach (var b in books)
            {
                string authorName = b.Author != null ? $"{b.Author.SurName} {b.Author.FirstName}" : "—";
                string libraryName = b.Library != null ? b.Library.Name : "—";

                table.AddRow(b.Id.ToString(), b.Name, authorName, libraryName);
            }

            // Выводим таблицу
            AnsiConsole.Write(table);

            // Запрашиваем ID книги для редактирования
            var bookId = AnsiConsole.Ask<int>("Введите [green]ID книги[/] для редактирования:");

            // Очищаем консоль после выбора книги
            AnsiConsole.Clear();

            // Ищем книгу по ID
            var book = db.Books
                .Include(b => b.Author)
                .Include(b => b.Library)
                .Include(b => b.SerialBook)
                .Include(b => b.Client)
                .FirstOrDefault(b => b.Id == bookId);

            if (book == null)
            {
                AnsiConsole.MarkupLine("[red]Книга не найдена.[/]");
                return;
            }

            // Ввод нового названия и описания
            book.Name = AnsiConsole.Ask<string>($"Введите новое название ([gray]{book.Name}[/]):", book.Name);
            book.Description = AnsiConsole.Ask<string>($"Описание ([gray]{book.Description}[/]):", book.Description);

            // Ввод доступности книги
            book.IsAvailable = AnsiConsole.Confirm("Доступна ли книга?", book.IsAvailable);

            // Показ авторов и выбор нового (если необходимо)
            AnsiConsole.MarkupLine("\n[yellow]Список авторов:[/]");
            var authors = db.Authors.ToList();
            foreach (var a in authors)
            {
                AnsiConsole.MarkupLine($"[grey]{a.Id}[/]: {a.SurName} {a.FirstName} {a.Patronomic}");
            }
            var authorId = AnsiConsole.Ask<int>($"Введите [green]ID нового автора[/] (или оставьте текущего, если не меняете):", book.Author.Id);

            // Показ клиентов и выбор нового (если необходимо)
            AnsiConsole.MarkupLine("\n[yellow]Список клиентов:[/]");
            var clients = db.Clients.ToList();
            foreach (var c in clients)
            {
                AnsiConsole.MarkupLine($"[grey]{c.Id}[/]: {c.SurName} {c.FirstName} {c.Patronomic}");
            }
            var clientId = AnsiConsole.Ask<int?>($"Введите [green]ID нового клиента[/] (или оставьте текущего, если не меняете):", book.Client?.Id);

            // Показ серий книг и выбор новой (если необходимо)
            AnsiConsole.MarkupLine("\n[yellow]Список серий книг:[/]");
            var serials = db.SerialsBooks.ToList();
            foreach (var s in serials)
            {
                AnsiConsole.MarkupLine($"[grey]{s.Id}[/]: {s.Name}");
            }
            var serialBookId = AnsiConsole.Ask<int>($"Введите [green]ID новой серии книги[/] (или оставьте текущую, если не меняете):", book.SerialBook.Id);

            // Показ библиотек и выбор новой (если необходимо)
            AnsiConsole.MarkupLine("\n[yellow]Список библиотек:[/]");
            var libraries = db.Libraries.ToList();
            foreach (var l in libraries)
            {
                AnsiConsole.MarkupLine($"[grey]{l.Id}[/]: {l.Name}");
            }
            var libraryId = AnsiConsole.Ask<int>($"Введите [green]ID новой библиотеки[/] (или оставьте текущую, если не меняете):", book.Library.Id);

            // Обновляем связанные сущности
            var author = db.Authors.Find(authorId);
            var client = clientId.HasValue ? db.Clients.Find(clientId.Value) : null;
            var serialBook = db.SerialsBooks.Find(serialBookId);
            var library = db.Libraries.Find(libraryId);

            if (author == null || serialBook == null || library == null || (clientId.HasValue && client == null))
            {
                AnsiConsole.MarkupLine("[red]Ошибка: один из указанных ID не найден.[/]");
                return;
            }

            // Применяем изменения
            book.Name = book.Name;
            book.Description = book.Description;
            book.IsAvailable = book.IsAvailable;
            book.Author = author;
            book.Library = library;
            book.SerialBook = serialBook;
            book.Client = client;

            db.SaveChanges();

            AnsiConsole.MarkupLine("[green]Книга успешно обновлена![/]");
        }




        static void DeleteBook()
        {
            using var db = new Context();

            // Выводим все книги с ID в таблице
            AnsiConsole.MarkupLine("\n[yellow]Список всех книг:[/]");

            var books = db.Books.Include(b => b.Author).Include(b => b.Library).Include(b => b.SerialBook).ToList();

            // Создание таблицы для вывода книг
            var table = new Table()
                .Border(TableBorder.Minimal)
                .AddColumn("ID")
                .AddColumn("Название")
                .AddColumn("Автор")
                .AddColumn("Библиотека");

            // Заполнение таблицы
            foreach (var b in books)
            {
                string authorName = b.Author != null ? $"{b.Author.SurName} {b.Author.FirstName}" : "—";
                string libraryName = b.Library != null ? b.Library.Name : "—";

                table.AddRow(b.Id.ToString(), b.Name, authorName, libraryName);
            }

            // Выводим таблицу
            AnsiConsole.Write(table);

            // Запрашиваем ID книги для удаления
            var bookId = AnsiConsole.Ask<int>("Введите [red]ID книги[/] для удаления:");

            // Очищаем консоль после выбора книги
            AnsiConsole.Clear();

            // Ищем книгу по ID
            var book = db.Books
                .Include(b => b.Author)
                .Include(b => b.Library)
                .Include(b => b.SerialBook)
                .Include(b => b.Client)
                .FirstOrDefault(b => b.Id == bookId);

            if (book == null)
            {
                AnsiConsole.MarkupLine("[red]Книга не найдена.[/]");
                return;
            }

            // Запрашиваем подтверждение на удаление
            var confirmation = AnsiConsole.Confirm($"Вы уверены, что хотите удалить книгу: [red]{book.Name}[/]?");

            if (!confirmation)
            {
                AnsiConsole.MarkupLine("[yellow]Удаление отменено.[/]");
                return;
            }

            // Удаляем книгу из базы данных
            db.Books.Remove(book);
            db.SaveChanges();

            AnsiConsole.MarkupLine("[green]Книга успешно удалена![/]");
        }

        static void GiveBookToClient()
        {
            using var db = new Context();

            // Получаем все доступные книги
            var availableBooks = db.Books
                .Include(b => b.Author)
                .Include(b => b.Library)
                .Where(b => b.IsAvailable)
                .ToList();

            if (!availableBooks.Any())
            {
                AnsiConsole.MarkupLine("[red]Нет доступных книг для выдачи.[/]");
                return;
            }

            // Выводим таблицу доступных книг
            AnsiConsole.MarkupLine("\n[yellow]Доступные книги:[/]");
            var bookTable = new Table()
                .Border(TableBorder.Minimal)
                .AddColumn("ID")
                .AddColumn("Название")
                .AddColumn("Автор")
                .AddColumn("Библиотека");

            foreach (var book in availableBooks)
            {
                string authorName = book.Author != null ? $"{book.Author.SurName} {book.Author.FirstName}" : "—";
                string libraryName = book.Library != null ? book.Library.Name : "—";

                bookTable.AddRow(book.Id.ToString(), book.Name, authorName, libraryName);
            }

            AnsiConsole.Write(bookTable);

            // Запрашиваем ID книги
            var bookId = AnsiConsole.Ask<int>("Введите [green]ID книги[/] для выдачи:");
            AnsiConsole.Clear();

            var selectedBook = db.Books
                .Include(b => b.Client)
                .FirstOrDefault(b => b.Id == bookId && b.IsAvailable);

            if (selectedBook == null)
            {
                AnsiConsole.MarkupLine("[red]Книга не найдена или уже выдана.[/]");
                return;
            }

            // Получаем всех клиентов
            var clients = db.Clients.ToList();

            if (!clients.Any())
            {
                AnsiConsole.MarkupLine("[red]Нет зарегистрированных клиентов.[/]");
                return;
            }

            // Выводим таблицу клиентов
            AnsiConsole.MarkupLine("\n[yellow]Список клиентов:[/]");
            var clientTable = new Table()
                .Border(TableBorder.Minimal)
                .AddColumn("ID")
                .AddColumn("ФИО")
                .AddColumn("Пол")
                .AddColumn("Дата рождения");

            foreach (var client in clients)
            {
                clientTable.AddRow(
                    client.Id.ToString(),
                    $"{client.SurName} {client.FirstName} {client.Patronomic}",
                    client.Sex,
                    client.DateOfBirthday.ToShortDateString());
            }

            AnsiConsole.Write(clientTable);

            // Запрашиваем ID клиента
            var clientId = AnsiConsole.Ask<int>("Введите [green]ID клиента[/] для выдачи книги:");
            AnsiConsole.Clear();

            var selectedClient = db.Clients.Find(clientId);
            if (selectedClient == null)
            {
                AnsiConsole.MarkupLine("[red]Клиент не найден.[/]");
                return;
            }

            // Устанавливаем клиента и обновляем статус книги
            selectedBook.Client = selectedClient;
            selectedBook.IsAvailable = false;

            db.SaveChanges();
            AnsiConsole.MarkupLine($"[green]Книга [bold]{selectedBook.Name}[/] успешно выдана клиенту [bold]{selectedClient.SurName} {selectedClient.FirstName}[/]![/]");
        }

        static void ReturnBookFromClient()
        {
            using var db = new Context();

            // Получаем все книги, которые выданы (т.е. IsAvailable == false и Client != null)
            var issuedBooks = db.Books
                .Include(b => b.Client)
                .Include(b => b.Author)
                .Where(b => !b.IsAvailable && b.Client != null)
                .ToList();

            if (!issuedBooks.Any())
            {
                AnsiConsole.MarkupLine("[yellow]Нет книг, выданных клиентам.[/]");
                return;
            }

            // Вывод таблицы выданных книг
            AnsiConsole.MarkupLine("\n[yellow]Список выданных книг:[/]");
            var table = new Table()
                .Border(TableBorder.Minimal)
                .AddColumn("ID")
                .AddColumn("Название книги")
                .AddColumn("Автор")
                .AddColumn("Клиент");

            foreach (var book in issuedBooks)
            {
                var authorName = book.Author != null ? $"{book.Author.SurName} {book.Author.FirstName}" : "—";
                var clientName = $"{book.Client.SurName} {book.Client.FirstName}";

                table.AddRow(
                    book.Id.ToString(),
                    book.Name,
                    authorName,
                    clientName
                );
            }

            AnsiConsole.Write(table);

            // Запрашиваем ID книги для возврата
            var bookId = AnsiConsole.Ask<int>("Введите [green]ID книги[/], которую хотите вернуть:");
            AnsiConsole.Clear();

            // Ищем выбранную книгу
            var bookToReturn = db.Books
                .Include(b => b.Client)
                .FirstOrDefault(b => b.Id == bookId && !b.IsAvailable);

            if (bookToReturn == null)
            {
                AnsiConsole.MarkupLine("[red]Книга не найдена или уже возвращена.[/]");
                return;
            }

            // Подтверждение возврата
            var confirm = AnsiConsole.Confirm($"Вы действительно хотите вернуть книгу [bold]{bookToReturn.Name}[/] от клиента [bold]{bookToReturn.Client.SurName} {bookToReturn.Client.FirstName}[/]?");
            if (!confirm)
            {
                AnsiConsole.MarkupLine("[yellow]Возврат отменён.[/]");
                return;
            }

            // Возвращаем книгу
            bookToReturn.Client = null;
            bookToReturn.IsAvailable = true;

            db.SaveChanges();

            AnsiConsole.MarkupLine($"[green]Книга [bold]{bookToReturn.Name}[/] успешно возвращена в библиотеку![/]");
        }
    }

}

