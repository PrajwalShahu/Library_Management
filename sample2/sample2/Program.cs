using LibraryManagement;
using sample2;
using Spectre.Console;
using System.Data.SqlClient;
using System.Net;
using Unity;

namespace LibraryManagement
{
    public class LoginManager
    {
        public string ConnectionString = "Data Source=IN-B33K9S3;Initial Catalog=Library_Management;Integrated Security=True;";
        public bool Login()
        {
            AnsiConsole.MarkupLine("[bold red3_1 underline]--- Login ---[/]");

            string username = AnsiConsole.Prompt(new TextPrompt<string>("[darkslategray1]Username:[/]"));
            string password = AnsiConsole.Prompt(new TextPrompt<string>("[darkslategray1]Password:[/]"));

            bool isValid = ValidateUser(username, password);
            return isValid;
        }

        public bool ValidateUser(string username, string password)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string query = "SELECT COUNT(*) FROM [User] WHERE Username = @Username AND Password = @Password";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);

            connection.Open();

            int result = (int)command.ExecuteScalar();

            connection.Close();

            return result > 0;
        }

    }


    public class LibraryManager 
    {
        public string ConnectionString = "Data Source=IN-B33K9S3;Initial Catalog=Library_Management;Integrated Security=True;";

        public void RunTasks()
        {
            while (true)
            {
                Console.WriteLine();
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold deeppink2]Select an option:[/]")
                        .AddChoices(new[]
                        {
                            "[rosybrown] Add Book Details[/]",
                            "[rosybrown] Edit Book Details[/]",
                            "[rosybrown] Delete Book Details[/]",
                            "[rosybrown] Add Student Details[/]",
                            "[rosybrown] Edit Student Details[/]",
                            "[rosybrown] Delete Student Details[/]",
                            "[rosybrown] Issue Book[/]",
                            "[rosybrown] Return Book[/]",
                            "[rosybrown] Search Books[/]",
                            "[rosybrown] Search Student[/]",
                            "[rosybrown] Show Students with Issued Books[/]",
                            //"[rosybrown] Exit[/]"
                        }));

                switch (choice)
                {
                    case "[rosybrown] Add Book Details[/]":
                        string insert_book = "INSERT INTO Book (Title, Author, Publication, AvailableStock) VALUES (@Title, @Author, @Publication, @AvailableStock)";
                        ManageBook(insert_book);
                        break;
                    case "[rosybrown] Edit Book Details[/]":
                        int id = AnsiConsole.Prompt(new TextPrompt<int>("[violet]Enter the Id of the Book to Edit:[/]"));
                        string update_book = $"UPDATE Book SET Title = @Title, Author = @Author, Publication = @Publication, AvailableStock = @AvailableStock WHERE BookId ={id}";
                        ManageBook(update_book);
                        break;
                    case "[rosybrown] Delete Book Details[/]":
                        DeleteBook();
                        break;
                    case "[rosybrown] Add Student Details[/]":
                        string insert_student = "INSERT INTO Student (Name, Address, PhoneNumber, DOB) VALUES (@Name, @Address, @number, @dob)";
                        ManageStudent(insert_student);
                        break;
                    case "[rosybrown] Edit Student Details[/]":
                        int RollNo = AnsiConsole.Prompt(new TextPrompt<int>("[violet]Enter the RollNo of the Student to Edit:[/]"));
                        string update_student = $"UPDATE Student SET Name = @Name, Address = @Address, PhoneNumber = @number, DOB = @dob WHERE RollNo = {RollNo}";
                        ManageStudent(update_student);
                        break;
                    case "[rosybrown] Delete Student Details[/]":
                        DeleteStudent();
                        break;
                    case "[rosybrown] Issue Book[/]":
                        IssueBook();
                        break;
                    case "[rosybrown] Return Book[/]":
                        ReturnBook();
                        break;
                    case "[rosybrown] Search Books[/]":
                        SearchBooks();
                        break;
                    case "[rosybrown] Search Student[/]":
                        SearchStudent();
                        break;
                    case "[rosybrown] Show Students with Issued Books[/]":
                        ShowStudentsWithBooks();
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
        public readonly IBookService _bookService;

        public LibraryManager(IBookService bookService)
        {
            _bookService = bookService;
        }

        public void ManageBook(string query)
        {
            try
            {
                AnsiConsole.MarkupLine("[bold cyan underline] Enter Book Details:[/]");

                string title = AnsiConsole.Prompt(new TextPrompt<string>("[violet] Book Title:[/]"));
                string authorName = AnsiConsole.Prompt(new TextPrompt<string>("[violet] Author Name:[/]"));
                string publicationName = AnsiConsole.Prompt(new TextPrompt<string>("[violet] Publication Name:[/]"));
                int availableStock = AnsiConsole.Prompt(new TextPrompt<int>("[violet] Available Stock:[/]"));

                int rowsAffected = _bookService.ManageBook(query, title, authorName, publicationName, availableStock);

                if (rowsAffected > 0)
                {
                    AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]Book Added/Edited successfully.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]Failed to Add/Edit book.[/]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Occurred: {ex.Message}");
            }
        }


        public void DeleteBook()
        {
            try
            {
                int bookId = AnsiConsole.Prompt(new TextPrompt<int>("[violet]Enter book Id to be deleted:[/]"));

                int rowsAffected = _bookService.DeleteBook(bookId);
                
                if (rowsAffected > 0)
                {
                    AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]Book deleted successfully.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]Failed to delete book.[/]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Occurred: {ex.Message}");
            }
        }


        public void ManageStudent(string query)
        {
            try
            {
                AnsiConsole.MarkupLine("[bold cyan underline] Enter Student details:[/]");

                string name = AnsiConsole.Prompt(new TextPrompt<string>("[violet] Student Name:[/]"));
                string address = AnsiConsole.Prompt(new TextPrompt<string>("[violet] Student Address:[/]"));
                long number = AnsiConsole.Prompt(new TextPrompt<long>("[violet] Student Phone number:[/]"));
                DateTime dob = AnsiConsole.Prompt(new TextPrompt<DateTime>("[violet] Student Date of Birth(mm-dd-yyyy):[/]"));

                int rowsAffected = _bookService.InsertStudent(query, name, address, number, dob);

                if (rowsAffected > 0)
                {
                    AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]Student Added/Updated successfully.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]Failed to Add/Update Student.[/]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Occurred: {ex.Message}");
            }
        }

        public void DeleteStudent()
        {
            int rollNo = AnsiConsole.Prompt(new TextPrompt<int>("[violet] Enter Student RollNo to be deleted:[/]"));

            int rowsAffected = _bookService.DeleteBook(rollNo);

            if (rowsAffected > 0)
            {
                AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]Student Deleted successfully.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]Failed to Delete student.[/]");
            }
        }

        public void IssueBook()
        {
            AnsiConsole.MarkupLine("[bold cyan underline]Issue Book -  Enter Book and Student details:[/]");

            int StudentId = AnsiConsole.Prompt(new TextPrompt<int>("[violet] Student RollNo:[/]"));
            int bookId = AnsiConsole.Prompt(new TextPrompt<int>("[violet] Book ID:[/]"));

            SqlConnection connection = new SqlConnection(ConnectionString);
            string availabilityQuery = "SELECT AvailableStock FROM Book WHERE BookId = @BookId";
            SqlCommand availabilityCommand = new SqlCommand(availabilityQuery, connection);
            availabilityCommand.Parameters.AddWithValue("@BookId", bookId);

            connection.Open();
            int availableStock = (int)availabilityCommand.ExecuteScalar();

            if (availableStock > 0)
            {
                string insertQuery = "INSERT INTO BookIssue (StudentId, BookId, IssueDate) VALUES (@StudentId, @BookId, @IssueDate)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@StudentId", StudentId);
                insertCommand.Parameters.AddWithValue("@BookId", bookId);
                insertCommand.Parameters.AddWithValue("@IssueDate", DateTime.Now);

                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    string updateStockQuery = "UPDATE Book SET AvailableStock = AvailableStock - 1 WHERE BookId = @BookId";
                    SqlCommand updateStockCommand = new SqlCommand(updateStockQuery, connection);
                    updateStockCommand.Parameters.AddWithValue("@BookId", bookId);
                    updateStockCommand.ExecuteNonQuery();

                    AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]Book issued successfully.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]Failed to issue book.[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]Book is not available.[/]");
            }

            connection.Close();
        }


        public void ReturnBook()
        {
            AnsiConsole.MarkupLine("[bold cyan underline]Return Book - Enter Book and Student details:[/]");

            int StudentId = AnsiConsole.Prompt(new TextPrompt<int>("[violet] Student RollNo:[/]"));
            int bookId = AnsiConsole.Prompt(new TextPrompt<int>("[violet] Book ID:[/]"));

            SqlConnection connection = new SqlConnection(ConnectionString);
            string issuedQuery = "SELECT COUNT(*) FROM BookIssue WHERE StudentId = @StudentId AND BookId = @BookId";
            SqlCommand issuedCommand = new SqlCommand(issuedQuery, connection);
            issuedCommand.Parameters.AddWithValue("@StudentId", StudentId);
            issuedCommand.Parameters.AddWithValue("@BookId", bookId);

            connection.Open();
            int count = (int)issuedCommand.ExecuteScalar();

            if (count > 0)
            {
                string updateQuery = $"UPDATE BookIssue SET ReturnDate = @returndate WHERE StudentId = @StudentId AND BookId = @BookId";
                SqlCommand deleteCommand = new SqlCommand(updateQuery, connection);
                deleteCommand.Parameters.AddWithValue("@StudentId", StudentId);
                deleteCommand.Parameters.AddWithValue("@BookId", bookId);
                deleteCommand.Parameters.AddWithValue("@returndate", DateTime.Now);

                int rowsAffected = deleteCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    string updateStockQuery = "UPDATE Book SET AvailableStock = AvailableStock + 1 WHERE BookId = @BookId";
                    SqlCommand updateStockCommand = new SqlCommand(updateStockQuery, connection);
                    updateStockCommand.Parameters.AddWithValue("@BookId", bookId);
                    updateStockCommand.ExecuteNonQuery();

                    AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]Book returned successfully.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]Failed to return book.[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]Book is not issued to the student.[/]");
            }

            connection.Close();
        }


        public void SearchBooks()
        {
            AnsiConsole.MarkupLine("[bold cyan underline]Search Books - Enter search Details:[/]");

            string SearchName = AnsiConsole.Prompt(new TextPrompt<string>("[violet] Author Name or Publication Name:[/]"));

            _bookService.SearchBook(SearchName);
        }

        public void SearchStudent()
        {
            int rollNo = AnsiConsole.Prompt(new TextPrompt<int>("[violet] Enter student roll number:[/]"));

            _bookService.SearchStudent(rollNo);            
        }


        public int ShowStudentsWithBooks()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string query = "SELECT S.RollNo, S.Name, B.BookId, B.Title, IB.IssueDate FROM Student S " +
                           "JOIN BookIssue IB ON S.RollNo = IB.StudentId " +
                           "JOIN Book B ON IB.BookId = B.BookId WHERE IB.ReturnDate IS NULL";
            SqlCommand command = new SqlCommand(query, connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                var table = new Table();
                table.Centered();
                table.Border = TableBorder.Heavy;
                table.AddColumn("Roll No");
                table.AddColumn("Name");
                table.AddColumn("Book Id");
                table.AddColumn("Book Title");
                table.AddColumn("Issue Date");

                while (reader.Read())
                {
                    table.AddRow(reader["RollNo"].ToString(), reader["Name"].ToString(), reader["BookId"].ToString(), reader["Title"].ToString(), reader["IssueDate"].ToString());
                }

                AnsiConsole.Render(new Markup("[underline lightgoldenrod2_1 bold]Students with Books:[/]").Centered());
                AnsiConsole.Write(table);
                reader.Close();
                connection.Close();
                return 1;
            }
            else
            {
                AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]No students currently have books.[/]");
                return 0;
            }            
        }


    }


    internal class Program
    {
        static void Main(string[] args)
        {
            AnsiConsole.Render(new FigletText("Library Management").Color(Color.LightGoldenrod3).Centered());
            LoginManager loginManager = new LoginManager();
            bool isLoggedIn = false;
            while (!isLoggedIn)
            {
                isLoggedIn = loginManager.Login();

                if (isLoggedIn)
                {
                    Console.WriteLine();
                    AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]Login successful![/]");
                    var container = new UnityContainer();
                    container.RegisterType<IBookRepository, BookRepository>();
                    container.RegisterType<IBookService, BookService>();

                    var program = container.Resolve<LibraryManager>();
                    //LibraryManager libraryManager = new LibraryManager();
                    program.RunTasks();
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]Login failed.[/]");
                    Console.WriteLine();
                }
            }
        }
    }
}