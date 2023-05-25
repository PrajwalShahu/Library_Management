using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample2
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;

        public BookService(IBookRepository repository)
        {
            _repository = repository;
        }

        public int ManageBook(string query, string title, string authorName, string publicationName, int availableStock)
        {
            return _repository.ManageBook(query, title, authorName, publicationName, availableStock);            
        }

        public int DeleteBook(int bookId)
        {
            return _repository.DeleteBook(bookId);
        }

        public int InsertStudent(string query, string name, string address, long number, DateTime dob)
        {
            return _repository.InsertStudent(query, name, address,number,dob);
        }

        public int DeleteStudent(int rollNo)
        {
            return _repository.DeleteStudent(rollNo);
        }

        public bool SearchBook(string searchName)
        {
            return _repository.SearchBook(searchName);
        }

        public bool SearchStudent(int rolNo) 
        {
            return _repository.SearchStudent(rolNo);
        }
    }
}
