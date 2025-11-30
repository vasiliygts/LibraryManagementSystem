using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem
{
    public partial class EditIssueReturnWindow : Window
    {
        private readonly LibraryContext _context;
        private readonly BorrowedBook _borrowedBook;

        public EditIssueReturnWindow(LibraryContext context, BorrowedBook borrowedBook)
        {
            InitializeComponent();
            _context = context;
            _borrowedBook = borrowedBook;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Завантаження списків читачів і книг
                ReaderComboBox.ItemsSource = _context.readers.ToList();
                BookComboBox.ItemsSource = _context.books.ToList();

                // Встановлення обраного значення
                ReaderComboBox.SelectedItem = _context.readers.FirstOrDefault(r => r.ReaderId == _borrowedBook.ReaderId);
                BookComboBox.SelectedItem = _context.books.FirstOrDefault(b => b.BookId == _borrowedBook.BookId);
                BorrowDatePicker.SelectedDate = _borrowedBook.BorrowDate;
                ReturnDatePicker.SelectedDate = _borrowedBook.ReturnDate;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Отримати реальний запис з бази даних
                var borrowedBook = _context.borrowedbooks
                    .FirstOrDefault(bb => bb.TransactionId == _borrowedBook.TransactionId);

                if (borrowedBook == null)
                {
                    MessageBox.Show("Error: Record not found in the database.");
                    return;
                }

                // Логування перед оновленням
                Console.WriteLine($"Before Update - BorrowedBook: TransactionId={borrowedBook.TransactionId}, ReaderId={borrowedBook.ReaderId}, BookId={borrowedBook.BookId}");

                // Перевірка та конвертація дат у UTC
                DateTime borrowDate = BorrowDatePicker.SelectedDate?.ToUniversalTime() ?? DateTime.UtcNow;
                DateTime? returnDate = ReturnDatePicker.SelectedDate?.ToUniversalTime();

                // Перевірка коректності дат
                if (returnDate.HasValue && returnDate.Value < borrowDate)
                {
                    MessageBox.Show("Return date cannot be earlier than borrow date.");
                    return;
                }

                // Оновити дані запису
                borrowedBook.BorrowDate = borrowDate;
                borrowedBook.ReturnDate = returnDate;

                //   NEW       // Явно оновлюємо стан об'єкта перед збереженням 
                _context.Entry(borrowedBook).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                //   NEW  

                //new2
                if (ReaderComboBox.SelectedItem is Reader selectedReader)
                {
                    borrowedBook.ReaderId = selectedReader.ReaderId;
                }

                if (BookComboBox.SelectedItem is Book selectedBook)
                {
                    borrowedBook.BookId = selectedBook.BookId;
                }
                //new2

                // Логування після оновлення
                Console.WriteLine($"After Update - BorrowedBook: TransactionId={borrowedBook.TransactionId}, ReaderId={borrowedBook.ReaderId}, BookId={borrowedBook.BookId}, BorrowDate={borrowedBook.BorrowDate}, ReturnDate={borrowedBook.ReturnDate}");



                _context.SaveChanges(); // Зберегти зміни
                MessageBox.Show("Record updated successfully!");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                // Логування винятку
                Console.WriteLine($"Exception: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                MessageBox.Show($"Error saving data: {ex.Message}\n\n" +
                    $"Inner Exception: {ex.InnerException?.Message}\n\n" +
                    $"Stack Trace: {ex.StackTrace}");
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
