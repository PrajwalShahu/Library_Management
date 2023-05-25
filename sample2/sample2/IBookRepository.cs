using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample2
{
    public interface IBookRepository
    {
        int ManageBook(string query,string title, string authorName, string publicationName, int availableStock);
        int DeleteBook(int bookId);
        int InsertStudent(string query, string name, string address, long number, DateTime dob);
        int DeleteStudent(int rollNo);
        bool SearchBook(string searchName);
        bool SearchStudent(int rolNo);
        
    }

}

