using Moq;
using NUnit.Framework;
using sample2;

namespace LibraryManagement
{
    [TestFixture]
    public class LoginManagerTests
    {
        [Test]
        public void ValidateUser_ValidUser_ReturnsTrue()
        {
            // Arrange
            var loginManager = new LoginManager();

            // Act
            bool isValid = loginManager.ValidateUser("Admin", "Admin123");

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void ValidateUser_InvalidUser_ReturnsFalse()
        {
            // Arrange
            var loginManager = new LoginManager();
            var username = "admi";
            var password = "sdmi";

            // Act
            var result = loginManager.ValidateUser(username, password);

            // Assert
            Assert.IsFalse(result);
        }
    }
    [TestFixture]
    public class BookManagerTests
    {
        [Test]
        public void ManageBook_WhenDataInserted_Returns1()
        {
            ////Arrange
            string query = "INSERT INTO Book (Title, Author, Publication, AvailableStock) VALUES (@Title, @Author, @Publication, @AvailableStock)";
            var repo = new Mock<IBookRepository>();
            repo.Setup(x => x.ManageBook(query,"3 States", "Chetan Bhagat","NCERT", 750)).Returns(1);
            var service = new BookService(repo.Object);
            ////Act
            var result = service.ManageBook(query, "3 States", "Chetan Bhagat", "NCERT", 750);
            ////Assert
            Assert.That(result, Is.EqualTo(1));

        }

        [Test]
        public void ManageBook_WhenDataNotInserted_Returns0()
        {
            ////Arrange
            string query = "INSERT INTO Book (Title, Author, Publication, AvailableStock) VALUES (@Title, @Author, @Publication, @AvailableStock)";
            var repo = new Mock<IBookRepository>();
            repo.Setup(x => x.ManageBook(query, "3 States", "Chetan Bhagat", "NCERT", 750)).Returns(0);
            var service = new BookService(repo.Object);
            ////Act
            var result = service.ManageBook(query, "3 States", "Chetan Bhagat", "NCERT", 750);
            ////Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void DeleteBook_WhenDataDataDeleted_Returns1()
        {
            ////Arrange
            var repo = new Mock<IBookRepository>();
            repo.Setup(x => x.DeleteBook(1)).Returns(1);
            var service = new BookService(repo.Object);
            ////Act
            var result = service.DeleteBook(1);
            ////Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ManageStudent_WhenDataInserted_Returns1()
        {
            ////Arrange
            string query = "INSERT INTO Student (Name, Address, PhoneNumber, DOB) VALUES (@Name, @Address, @number, @dob)";
            DateTime dateOfBirth = new DateTime(1999, 12, 21);
            var repo = new Mock<IBookRepository>();
            repo.Setup(x => x.InsertStudent(query, "Prajwal", "Nagpur",1234567890, It.IsAny<DateTime>())).Returns(1);
            var service = new BookService(repo.Object);
            ////Act
            var result = service.InsertStudent(query, "Prajwal", "Nagpur", 1234567890, It.IsAny<DateTime>());
            ////Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void DeleteStudent_WhenDataDataDeleted_Returns1()
        {
            ////Arrange
            var repo = new Mock<IBookRepository>();
            repo.Setup(x => x.DeleteStudent(1)).Returns(1);
            var service = new BookService(repo.Object);
            ////Act
            var result = service.DeleteStudent(1);
            ////Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void SearchBook_WhenFound_ReturnsTrue()
        {
            ////Arrange
            var repo = new Mock<IBookRepository>();
            repo.Setup(x => x.SearchBook("3 States")).Returns(true);
            var service = new BookService(repo.Object);
            ////Act
            var result = service.SearchBook("3 States");
            ////Assert
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void SearchStudent_WhenFound_ReturnsTrue()
        {
            ////Arrange
            var repo = new Mock<IBookRepository>();
            repo.Setup(x => x.SearchStudent(2)).Returns(true);
            var service = new BookService(repo.Object);
            ////Act
            var result = service.SearchStudent(2);
            ////Assert
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void ShowStudentsWithBooks_WhenFound_ReturnsTrue()
        {
            //// Arrange
            //var connectionString = "YourConnectionString"; 
            //var query = "SELECT S.RollNo, S.Name, B.BookId, B.Title, IB.IssueDate FROM Student S " +
            //            "JOIN BookIssue IB ON S.RollNo = IB.StudentId " +
            //            "JOIN Book B ON IB.BookId = B.BookId WHERE IB.ReturnDate IS NULL";

            //var bookService = new BookService(); 
            //var libraryManager = new LibraryManager(bookService);

            //// Act
            //bool res = libraryManager.ShowStudentsWithBooks();

            //// Assert
            //Assert.IsTrue(res);
            // Arrange

            // Arrange
            //var libraryManager = new LibraryManager();

            //// Act
            //var result = libraryManager.ShowStudentsWithBooks();

            //// Assert
            //Assert.IsNotNull(result);
            //Assert.IsTrue(result.HasRows);
        }
    }
}
