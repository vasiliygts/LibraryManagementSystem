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
/// Interaction logic for AddBookWindow.xaml
/// </summary>
public partial class AddBookWindow : Window
{
    private readonly LibraryContext _context;

    public AddBookWindow(LibraryContext context)
    {
        InitializeComponent();
        _context = context;
    }

    private void AddBook_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(YearTextBox.Text) || !int.TryParse(YearTextBox.Text, out int year))
            {
                MessageBox.Show("Please enter a valid year.");
                return;
            }

            var newBook = new Book
            {
                Title = TitleTextBox.Text,
                Author = AuthorTextBox.Text,
                Genre = GenreTextBox.Text,
                Year = int.Parse(YearTextBox.Text),
                ISBN = ISBNTextBox.Text,
                IsAvailable = true
            };

            _context.books.Add(newBook);
            _context.SaveChanges();
            MessageBox.Show("Book added successfully!");
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error adding book: {ex.Message}");
        }
    }
}
