using LibraryManagementSystem.Models;
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

namespace LibraryManagementSystem
{
    public partial class AddEditBookWindow : Window
    {
        public Book Book { get; private set; }

        public AddEditBookWindow(Book book)
        {
            InitializeComponent();
            Book = book;

            // Заповнення текстових полів поточними значеннями книги
            TitleTextBox.Text = Book.Title;
            AuthorTextBox.Text = Book.Author;
            GenreTextBox.Text = Book.Genre;
            YearTextBox.Text = Book.Year.ToString();
            ISBNTextBox.Text = Book.ISBN;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Перевірка коректності введених даних
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
                string.IsNullOrWhiteSpace(AuthorTextBox.Text) ||
                string.IsNullOrWhiteSpace(GenreTextBox.Text) ||
                !int.TryParse(YearTextBox.Text, out int year) ||
                string.IsNullOrWhiteSpace(ISBNTextBox.Text))
            {
                MessageBox.Show("Please fill in all fields correctly.");
                return;
            }

            // Збереження змін у вибраній книзі
            Book.Title = TitleTextBox.Text;
            Book.Author = AuthorTextBox.Text;
            Book.Genre = GenreTextBox.Text;
            Book.Year = year;
            Book.ISBN = ISBNTextBox.Text;

            DialogResult = true; // Повернення успішного результату
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Повернення відміни
            Close();
        }
    }
}
