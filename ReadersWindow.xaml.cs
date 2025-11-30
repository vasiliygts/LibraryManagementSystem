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

namespace LibraryManagementSystem;

/// <summary>
/// Interaction logic for ReadersWindow.xaml
/// </summary>
public partial class ReadersWindow : Window
{
    private readonly LibraryContext _context = new LibraryContext();

    public ReadersWindow()
    {
        InitializeComponent();
        this.Loaded += ReadersWindow_Loaded;// Прив'язуємо метод для події Loaded
    }

    private void ReadersWindow_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            // Встановлення тексту за замовчуванням для SearchBox
            SearchBox.Text = SearchBox.Tag?.ToString();
            SearchBox.Foreground = System.Windows.Media.Brushes.Gray;

            LoadReaders(); // Завантаження даних після повної ініціалізації елементів
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading readers: {ex.Message}");
        }
    }

    private void LoadReaders(string filter = "")
    {
        try
        {
            var readers = _context.readers.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                readers = readers.Where(r => r.Name.ToLower().Contains(filter) ||
                                             r.Class.ToLower().Contains(filter) ||
                                             r.Phone.ToLower().Contains(filter) ||
                                             r.Email.ToLower().Contains(filter));
            }

            ReadersDataGrid.ItemsSource = readers.OrderBy(r => r.ReaderId).ToList();

            // new // Сортування по ReaderId
            readers = readers.OrderBy(r => r.ReaderId); // ????
                                                        // Встановлення джерела даних для DataGrid
            ReadersDataGrid.ItemsSource = readers.ToList();
            ///////

        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading readers: {ex.Message}");
        }
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        LoadReaders(SearchBox.Text);
    }

    private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox && textBox.Text == textBox.Tag?.ToString())
        {
            //textBox.Text = "";
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

    private void AddReader(object sender, RoutedEventArgs e)
    {
        var addReaderWindow = new AddReaderWindow(_context);
        addReaderWindow.ShowDialog();
        LoadReaders();// Оновити дані після додавання
    }

    private void EditReader(object sender, RoutedEventArgs e)
    {
        if (ReadersDataGrid.SelectedItem is Reader selectedReader)
        {
            var editReaderWindow = new EditReaderWindow(_context, selectedReader);
            editReaderWindow.ShowDialog();
            LoadReaders();// Оновити дані після редагування
        }
        else
        {
            MessageBox.Show("Please select a reader to edit.");
        }
    }

    private void DeleteReader(object sender, RoutedEventArgs e)
    {
        if (ReadersDataGrid.SelectedItem is Reader selectedReader)
        {
            var result = MessageBox.Show($"Are you sure you want to delete reader {selectedReader.Name}?",
                                         "Confirm Deletion", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                _context.readers.Remove(selectedReader);
                _context.SaveChanges();
                LoadReaders();
                MessageBox.Show("Reader deleted successfully!");
            }
        }
        else
        {
            MessageBox.Show("Please select a reader to delete.");
        }
    }
}
