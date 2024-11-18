public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public bool Available { get; set; }

    public Book(string title, string author)
    {
        Title = title;
        Author = author;
        Available = true;
    }
}

public class User
{
    public string Name { get; set; }
    public int BooksBorrowed { get; set; }

    public User(string name)
    {
        Name = name;
        BooksBorrowed = 0;
    }
}

public enum Action
{
    Borrow,
    Return,
    View,
    Exit,
    Invalid
}

public class Program
{
    private static Dictionary<string, Book> books = new Dictionary<string, Book>(StringComparer.OrdinalIgnoreCase)
    {
        { "To Kill a Mockingbird", new Book("To Kill a Mockingbird", "Harper Lee") },
        { "1984", new Book("1984", "George Orwell") },
        { "The Great Gatsby", new Book("The Great Gatsby", "F. Scott Fitzgerald") },
        { "The Catcher in the Rye", new Book("The Catcher in the Rye", "J.D. Salinger") },
        { "Moby-Dick", new Book("Moby-Dick", "Herman Melville") }
    };

    private static User user = new User("Michael");

    public static void Main()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("Enter 'Borrow' to add a book, 'Return' to return a book, 'View' to view available books, or 'Exit' to exit the program");
            string actionInput = Console.ReadLine() ?? string.Empty;
            Action action = ParseAction(actionInput);

            switch (action)
            {
                case Action.Borrow:
                    BorrowBook();
                    break;
                case Action.Return:
                    ReturnBook();
                    break;
                case Action.View:
                    ViewBooks();
                    break;
                case Action.Exit:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid action");
                    break;
            }
        }
    }

    private static Action ParseAction(string actionInput)
    {
        return Enum.TryParse(actionInput, true, out Action action) ? action : Action.Invalid;
    }

    private static void BorrowBook()
    {
        if (user.BooksBorrowed >= 3)
        {
            Console.WriteLine("You have borrowed the maximum number of books");
            return;
        }

        Console.WriteLine("Enter the title of the book you want to borrow");
        string searchBook = Console.ReadLine() ?? string.Empty;

        if (!books.TryGetValue(searchBook, out Book book))
        {
            Console.WriteLine("Book not found");
            return;
        }

        if (!book.Available)
        {
            Console.WriteLine("Book is already borrowed");
            return;
        }

        book.Available = false;
        user.BooksBorrowed++;
        Console.WriteLine($"You have borrowed \"{book.Title}\" by {book.Author}");
    }

    private static void ReturnBook()
    {
        if (user.BooksBorrowed == 0)
        {
            Console.WriteLine("You have no books to return");
            return;
        }

        Console.WriteLine("Enter the title of the book you want to return");
        string bookToReturn = Console.ReadLine() ?? string.Empty;

        if (!books.TryGetValue(bookToReturn, out Book book))
        {
            Console.WriteLine("Book not found");
            return;
        }

        if (book.Available)
        {
            Console.WriteLine("Book is already available");
            return;
        }

        book.Available = true;
        user.BooksBorrowed--;
        Console.WriteLine($"You have returned \"{book.Title}\" by {book.Author}");
    }

    private static void ViewBooks()
    {
        foreach (var book in books.Values)
        {
            Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, Available: {book.Available}");
        }
    }
}
