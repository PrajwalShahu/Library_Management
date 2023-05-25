using sample2;
using Spectre.Console;
using System.Data.SqlClient;
using System.Net;
using System.Xml.Linq;

public class BookRepository : IBookRepository
{
    public string connectionString = "Data Source=IN-B33K9S3;Initial Catalog=Library_Management;Integrated Security=True;";

    public int ManageBook(string query,string title, string authorName, string publicationName, int availableStock)
    {

        //string query = "INSERT INTO Book (Title, Author, Publication, AvailableStock) VALUES (@Title, @Author, @Publication, @AvailableStock)";

        SqlConnection connection = new SqlConnection(connectionString);

        SqlCommand command = new SqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@Title", title);
        command.Parameters.AddWithValue("@Author", authorName);
        command.Parameters.AddWithValue("@Publication", publicationName);
        command.Parameters.AddWithValue("@AvailableStock", availableStock);

        connection.Open();
        int rowsAffected = command.ExecuteNonQuery();
        connection.Close();
        if (rowsAffected > 0)
        {
            return rowsAffected;
        }
        else
        {
            return 0;
        }

    }

    public int DeleteBook(int bookId)
    {
        SqlConnection connection = new SqlConnection(connectionString);
        string query = "DELETE FROM Book WHERE BookId = @BookId";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@BookId", bookId);

        connection.Open();
        int rowsAffected = command.ExecuteNonQuery();
        connection.Close();
        if (rowsAffected > 0)
        {
            return rowsAffected;
        }
        else
        {
            return 0;
        }
    }

    public int InsertStudent(string query, string name, string address, long number, DateTime dob)
    {
        //string query = "INSERT INTO Student (Name, Address, PhoneNumber, DOB) VALUES (@Name, @Address, @number, @dob)";
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Name", name);
        command.Parameters.AddWithValue("@address", address);
        command.Parameters.AddWithValue("@number", number);
        command.Parameters.AddWithValue("@dob", dob);

        connection.Open();
        int rowsAffected = command.ExecuteNonQuery();

        connection.Close();
        if (rowsAffected > 0)
        {
            return rowsAffected;
        }
        else
        {
            return 0;
        }
    }

    public int DeleteStudent(int rollNo)
    {
        SqlConnection connection = new SqlConnection(connectionString);
        string query = "DELETE FROM Student WHERE RollNo = @RollNo";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@RollNo", rollNo);

        connection.Open();
        int rowsAffected = command.ExecuteNonQuery();

        connection.Close();

        if (rowsAffected > 0)
        {
            return rowsAffected;
        }
        else
        {
            return 0;
        }
    }

    public bool SearchBook(string searchName)
    {
        SqlConnection connection = new SqlConnection(connectionString);
        string query = "SELECT * FROM Book WHERE Author LIKE @SearchName OR Publication LIKE @SearchName";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@SearchName", "%" + searchName + "%");

        connection.Open();
        SqlDataReader reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            var table = new Table();
            table.Centered();
            table.Border = TableBorder.AsciiDoubleHead;
            table.AddColumn("Book ID");
            table.AddColumn("Title");
            table.AddColumn("Author");
            table.AddColumn("Publication");
            table.AddColumn("Available Stock");

            while (reader.Read())
            {
                table.AddRow(reader["BookId"].ToString(), reader["Title"].ToString(), reader["Author"].ToString(), reader["Publication"].ToString(), reader["AvailableStock"].ToString());
            }

            AnsiConsole.Render(new Markup("[underline lightgoldenrod2_1 bold]Searched Books[/]").Centered());
            AnsiConsole.Write(table);
            reader.Close();
            connection.Close();
            return true;
        }
        else
        {
            AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]No books found matching the search criteria.[/]");
            return false;
        }        
    }

    public bool SearchStudent(int rollNo)
    {
        SqlConnection connection = new SqlConnection(connectionString);
        string query = "SELECT * FROM Student WHERE RollNo = @RollNo";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@RollNo", rollNo);

        connection.Open();
        SqlDataReader reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            var table = new Table();
            table.Centered();
            table.Border = TableBorder.Rounded;
            table.AddColumn("Roll No");
            table.AddColumn("Name");
            table.AddColumn("Address");
            table.AddColumn("Phone Number");
            table.AddColumn("Date of Birth");

            while (reader.Read())
            {
                table.AddRow(reader["RollNo"].ToString(), reader["Name"].ToString(), reader["Address"].ToString(), reader["PhoneNumber"].ToString(), reader["DOB"].ToString());
            }

            AnsiConsole.Render(new Markup("[underline lightgoldenrod2_1 bold]Searched Students[/]").Centered());
            AnsiConsole.Write(table);
            reader.Close();
            return true;
        }
        else
        {
            AnsiConsole.MarkupLine("[bold lightgoldenrod2_1]No student found with the provided Roll No.[/]");
            return false;
        }
    }

}
