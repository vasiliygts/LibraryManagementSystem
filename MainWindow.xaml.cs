using System.Linq;
using System.Windows;
using LibraryManagementSystem.Data;

namespace LibraryManagementSystem;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly LibraryContext _context = new LibraryContext();

    public MainWindow()
    {
        InitializeComponent();
        LoadSummaryData(); // Завантаження підсумкових даних при ініціалізації вікна
        UpdateBookAndReaderStats();
    }

    private void LoadSummaryData()
    {
        try
        {
            // Підрахунок загальної кількості читачів
            int totalReaders = _context.readers.Count();
            TotalReadersText.Text = $"Всього читачів: {totalReaders}";

            // Підрахунок загальної кількості книг
            int totalBooks = _context.books.Count();
            TotalBooksText.Text = $"Всього книг: {totalBooks}";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading summary data: {ex.Message}");
        }
    }

    public void UpdateBookAndReaderStats()
    {
        try
        {
            using (var context = new LibraryContext())
            {
                // Загальна кількість книг
                int totalBooks = context.books.Count();

                // Загальна кількість читачів
                int totalReaders = context.readers.Count();

                // Кількість книг на руках у читачів
                int booksOnLoan = context.borrowedbooks
                    .Where(bb => bb.ReturnDate == null)
                    .Count();

                // Кількість книг у бібліотеці
                int booksInLibrary = totalBooks - booksOnLoan;

                // Відображення даних у текстових блоках
                //TotalBooksText.Text = $"Загальна кількість книг: {totalBooks}";
                //TotalReadersText.Text = $"Загальна кількість читачів: {totalReaders}";
                BooksOnLoanText.Text = $"Кількість книг на руках: {booksOnLoan}";
                BooksInLibraryText.Text = $"Кількість книг у бібліотеці: {booksInLibrary}";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка під час оновлення статистики: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OpenBooksWindow(object sender, RoutedEventArgs e)
    {
        BooksWindow booksWindow = new BooksWindow();
        booksWindow.Show();
    }

    private void OpenReadersWindow(object sender, RoutedEventArgs e)
    {
        ReadersWindow readersWindow = new ReadersWindow();
        readersWindow.Show();
    }

    private void OpenIssueReturnWindow(object sender, RoutedEventArgs e)
    {
        IssueReturnWindow issueReturnWindow = new IssueReturnWindow();
        issueReturnWindow.Show();
    }

    private void ExitApplication(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}
