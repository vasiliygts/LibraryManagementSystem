using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace LibraryManagementSystem;

public partial class IssueReturnWindow : Window
{
    private readonly LibraryContext _context = new LibraryContext();

    public IssueReturnWindow()
    {
        InitializeComponent();
        this.Loaded += IssueReturnWindow_Loaded;
    }

    private void IssueReturnWindow_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            // Встановлення тексту за замовчуванням для поля пошуку
            SearchBox.Text = SearchBox.Tag?.ToString();
            SearchBox.Foreground = System.Windows.Media.Brushes.Gray;

            LoadRecords(); // Завантаження даних
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading records: {ex.Message}");
        }
    }

    private void LoadRecords(string filter = "")
    {
        try
        {
            // Завантаження даних із використанням DTO
            var records = _context.borrowedbooks
            // NEW  ///
            .Include(bb => bb.Reader)  // Додаємо завантаження даних читача
            .Include(bb => bb.Book)    // Додаємо завантаження даних книги

            ///



                .Select(bb => new IssueReturnDto
                {
                    TransactionId = bb.TransactionId,
                    //ReaderName = bb.Reader.Name,
                    ReaderName = bb.Reader != null ? bb.Reader.Name : "Unknown Reader",
                    //BookTitle = bb.Book.Title,
                    BookTitle = bb.Book != null ? bb.Book.Title : "Unknown Book",
                    BorrowDate = bb.BorrowDate,
                    ReturnDate = bb.ReturnDate
                })
                .AsQueryable();

            // Фільтрація
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                records = records.Where(r =>
                    r.ReaderName.ToLower().Contains(filter) ||
                    r.BookTitle.ToLower().Contains(filter));
            }




            // Сортування за TransactionId
            records = records.OrderBy(r => r.TransactionId);

            // Логування кількості записів
            Console.WriteLine($"Loaded {records.Count()} records.");

            // Прив'язка до DataGrid
            IssueReturnDataGrid.ItemsSource = records.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading records: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            MessageBox.Show($"Error loading records: {ex.Message}");
        }
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        LoadRecords(SearchBox.Text);
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

    private void AddRecord(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddIssueReturnWindow(_context);
        if (addWindow.ShowDialog() == true)
        {
            LoadRecords(); // Оновлення після додавання
        }
    }
        

    private void EditRecord(object sender, RoutedEventArgs e)
    {
        if (IssueReturnDataGrid.SelectedItem is IssueReturnDto selectedDto)
        {
            var borrowedBook = _context.borrowedbooks
                .FirstOrDefault(bb => bb.TransactionId == selectedDto.TransactionId);

            if (borrowedBook != null)
            {
                var editWindow = new EditIssueReturnWindow(_context, borrowedBook);
                if (editWindow.ShowDialog() == true)
                {
                    LoadRecords(); // Оновлення після редагування
                }
            }
            else
            {
                MessageBox.Show("Record not found in database.");
            }
        }
    }


    private void DeleteRecord(object sender, RoutedEventArgs e)
    {
        if (IssueReturnDataGrid.SelectedItem is IssueReturnDto selectedRecord)
        {
            // Знаходимо запис у базі даних за TransactionId
            var borrowedBook = _context.borrowedbooks
                .FirstOrDefault(bb => bb.TransactionId == selectedRecord.TransactionId);

            if (borrowedBook != null)
            {
                try
                {
                    _context.borrowedbooks.Remove(borrowedBook);
                    _context.SaveChanges();
                    MessageBox.Show("Record deleted successfully!");
                    LoadRecords(); // Оновлення після видалення
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting record: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Record not found in the database.");
            }
        }
        else
        {
            MessageBox.Show("Please select a record to delete.");
        }
    }

    private void ExportToText_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Отримання даних з DataGrid
            var records = IssueReturnDataGrid.ItemsSource as IEnumerable<IssueReturnDto>;

            if (records == null || !records.Any())
            {
                MessageBox.Show("Немає записів для експорту!", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Формування текстового файлу
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Transaction ID\tReader Name\tBook Title\tBorrow Date\tReturn Date");
            foreach (var record in records)
            {
                sb.AppendLine($"{record.TransactionId}\t{record.ReaderName}\t{record.BookTitle}\t{record.BorrowDate:dd.MM.yyyy}\t{record.ReturnDate:dd.MM.yyyy}");
            }

            // Збереження у файл
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt",
                FileName = "IssueReturnRecords.txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show("Експорт успішно завершено!", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка під час експорту: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
