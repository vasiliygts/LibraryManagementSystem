using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models
{
    public class IssueReturnDto
    {
        public int TransactionId { get; set; }      // Унікальний ідентифікатор запису
        public string ReaderName { get; set; }     // Ім'я читача
        public string BookTitle { get; set; }      // Назва книги
        public DateTime BorrowDate { get; set; }   // Дата позики
        public DateTime? ReturnDate { get; set; }  // Дата повернення (може бути null)
    }
}
