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
    public partial class AddIssueReturnWindow : Window
    {
        private readonly LibraryContext _context;

        public AddIssueReturnWindow(LibraryContext context)
        {
            InitializeComponent();
            _context = context;
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {

            // Завантаження списків читачів та книг з сортуванням
            ReaderComboBox.ItemsSource = _context.readers
                .OrderBy(r => r.Name) // Сортуємо читачів за іменем
                .ToList();

            BookComboBox.ItemsSource = _context.books
                .OrderBy(b => b.Title) // Сортуємо книги за назвою
                .ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ReaderComboBox.SelectedItem is Reader selectedReader &&
                    BookComboBox.SelectedItem is Book selectedBook)
                {
                    //// Логування вибраних даних
                    //Console.WriteLine($"ReaderId: {selectedReader.ReaderId}, BookId: {selectedBook.BookId}");

                    // Перевірка: чи ця книга вже була видана і не повернута
                    bool isAlreadyBorrowed = _context.borrowedbooks
                        .Any(bb => bb.BookId == selectedBook.BookId && bb.ReturnDate == null);

                    if (isAlreadyBorrowed)
                    {
                        MessageBox.Show("This book is already borrowed and has not been returned.");
                        return;
                    }

                    // Копіюємо існуючий запис (беремо перший із таблиці)
                    var templateBorrowedBook = _context.borrowedbooks.FirstOrDefault();
                    BorrowedBook newBorrowedBook;

                    if (templateBorrowedBook != null)
                    {
                        // Копіюємо всі поля, але залишаємо TransactionId 0 (бо це новий запис)
                        newBorrowedBook = new BorrowedBook
                        {
                            TransactionId = 0, // Новий запис, тому TransactionId не копіюємо
                            ReaderId = templateBorrowedBook.ReaderId,
                            BookId = templateBorrowedBook.BookId,
                            BorrowDate = templateBorrowedBook.BorrowDate,  // Копіюємо оригінальну дату
                            ReturnDate = templateBorrowedBook.ReturnDate   // Копіюємо оригінальну дату повернення (може бути null)
                        };
                    }
                    else
                    {
                        // Якщо база порожня, створюємо запис без копіювання
                        newBorrowedBook = new BorrowedBook
                        {
                            TransactionId = 0, // Новий запис
                            ReaderId = selectedReader.ReaderId,
                            BookId = selectedBook.BookId,
                            BorrowDate = BorrowDatePicker.SelectedDate ?? DateTime.Now,
                            ReturnDate = ReturnDatePicker.SelectedDate
                        };
                    }

                    // Оновлення значень для нового запису
                    newBorrowedBook.ReaderId = selectedReader.ReaderId;
                    newBorrowedBook.BookId = selectedBook.BookId;

                    //  Універсалізація дати (аналогічно до EditIssueReturnWindow)
                    newBorrowedBook.BorrowDate = BorrowDatePicker.SelectedDate?.ToUniversalTime() ?? DateTime.UtcNow;
                    newBorrowedBook.ReturnDate = ReturnDatePicker.SelectedDate?.ToUniversalTime();

                    //  **Перевірка коректності дат**
                    if (newBorrowedBook.ReturnDate.HasValue && newBorrowedBook.ReturnDate < newBorrowedBook.BorrowDate)
                    {
                        MessageBox.Show("Return date cannot be earlier than borrow date.", "Invalid Date", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Логування перед додаванням
                    Console.WriteLine($"Final BorrowedBook: ReaderId={newBorrowedBook.ReaderId}, BookId={newBorrowedBook.BookId}, BorrowDate={newBorrowedBook.BorrowDate}, ReturnDate={newBorrowedBook.ReturnDate}");

                    // Додаємо новий запис у контекст БД
                    _context.borrowedbooks.Add(newBorrowedBook);
                    _context.SaveChanges();

                    MessageBox.Show("Record added successfully!");
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please select both a reader and a book.");
                }
            }
            catch (Exception ex)
            {
                // Логування та відображення винятків
                StringBuilder errorMessage = new StringBuilder();
                errorMessage.AppendLine($"Error saving data: {ex.Message}");
                if (ex.InnerException != null)
                {
                    errorMessage.AppendLine($"Inner Exception: {ex.InnerException.Message}");
                }
                errorMessage.AppendLine("Stack Trace:");
                errorMessage.AppendLine(ex.StackTrace);

                MessageBox.Show(errorMessage.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Додатковий запис у консоль
                Console.WriteLine(errorMessage.ToString());
            }
        }



        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
