using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;


namespace LibraryManagementSystem
{

    /// <summary>
    /// Interaction logic for BooksWindow.xaml
    /// </summary>
    public partial class BooksWindow : Window
    {
        private readonly LibraryContext _context = new LibraryContext();
              

        public BooksWindow()
        {
            InitializeComponent();
            this.Loaded += BooksWindow_Loaded; // Прив'язуємо метод для події Loaded
        }

        private void BooksWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Встановлення тексту за замовчуванням для SearchBox
                SearchBox.Text = SearchBox.Tag?.ToString();
                SearchBox.Foreground = System.Windows.Media.Brushes.Gray;

                //if (_context == null)
                //{
                //    throw new NullReferenceException("LibraryContext (_context) is null.");
                //}

                LoadBooks(); // Завантаження даних після повної ініціалізації елементів
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}");
            }
        }

        private void LoadBooks(string filter = "")
        {
            try
            {
                // Отримання даних із бази
                var books = _context.books.AsQueryable();


                // Динамічна фільтрація
                if (!string.IsNullOrEmpty(filter))
                //{
                //    books = books.Where(b => b.Title.Contains(filter) ||
                //                             b.Author.Contains(filter) ||
                //                             b.Genre.Contains(filter));
                //}

                {
                    filter = filter.ToLower(); // Перетворення фільтру на нижній регістр

                    books = books.Where(b => b.Title.ToLower().Contains(filter) ||
                                             b.Author.ToLower().Contains(filter) ||
                                             b.Genre.ToLower().Contains(filter) ||
                                             b.ISBN.ToLower().Contains(filter) ||
                                             b.Year.ToString().Contains(filter));
                }

                // Сортування по BookId
                books = books.OrderBy(b => b.BookId);


                // Встановлення джерела даних для DataGrid
                BooksDataGrid.ItemsSource = books.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}");
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Оновлення даних у DataGrid на основі введеного тексту
            LoadBooks(SearchBox.Text);
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            

            if (sender is TextBox textBox && textBox.Text == textBox.Tag?.ToString())
            {
                textBox.Text = string.Empty;
                textBox.Foreground = System.Windows.Media.Brushes.Black;
            }

        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Tag?.ToString();
                textBox.Foreground = System.Windows.Media.Brushes.Gray;
            }

        }

        private void AddBook(object sender, RoutedEventArgs e)
        {
            var addBookWindow = new AddBookWindow(_context);
            addBookWindow.ShowDialog();
            LoadBooks(); // Оновити дані після додавання
        }

        private void EditBook(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem is Book selectedBook)
            {
                var editWindow = new AddEditBookWindow(selectedBook);
                if (editWindow.ShowDialog() == true)
                {
                    try
                    {
                        _context.SaveChanges(); // Збереження змін у базі
                        MessageBox.Show("Book updated successfully!");
                        LoadBooks(); // Оновлення DataGrid
                    }
                    catch (DbUpdateException ex)
                    {
                        MessageBox.Show($"Error updating book: {ex.InnerException?.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a book to edit.");
            }
        }

        private void DeleteBook(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem is Book selectedBook)
            {
                var result = MessageBox.Show($"Are you sure you want to delete '{selectedBook.Title}'?",
                                             "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _context.books.Remove(selectedBook);
                        _context.SaveChanges(); // Збереження змін у базі
                        MessageBox.Show("Book deleted successfully!");
                        LoadBooks(); // Оновлення DataGrid
                    }
                    catch (DbUpdateException ex)
                    {
                        MessageBox.Show($"Error deleting book: {ex.InnerException?.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a book to delete.");
            }
        }

        private void ExportToFile(object sender, RoutedEventArgs e)
        {
            try
            {
                // Отримання даних із DataGrid
                var books = BooksDataGrid.ItemsSource as IEnumerable<Book>;
                if (books == null || !books.Any())
                {
                    MessageBox.Show("No data to export.");
                    return;
                }

                // Вибір місця для збереження файлу
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Text File (*.txt)|*.txt",
                    FileName = "BooksExport.txt"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (var writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        // Запис заголовків
                        writer.WriteLine("BookId\tTitle\tAuthor\tGenre\tYear\tISBN\tIsAvailable");

                        // Запис даних
                        foreach (var book in books)
                        {
                            writer.WriteLine($"{book.BookId}\t{book.Title}\t{book.Author}\t{book.Genre}\t{book.Year}\t{book.ISBN}\t{book.IsAvailable}");
                        }
                    }

                    MessageBox.Show("Data successfully exported!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data: {ex.Message}");
            }
        }

    }


}



