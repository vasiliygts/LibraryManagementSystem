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

public partial class AddReaderWindow : Window
{
    private readonly LibraryContext _context;

    public AddReaderWindow(LibraryContext context)
    {
        InitializeComponent();
        _context = context;
    }

    private void SaveReader(object sender, RoutedEventArgs e)
    {
        try
        {
            // Перевірка на заповнення обов’язкових полів
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(ClassTextBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Валідація електронної пошти
            if (!IsValidEmail(EmailTextBox.Text))
            {
                MessageBox.Show("Invalid email format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Створення нового читача
            var newReader = new Reader
            {
                Name = NameTextBox.Text.Trim(),
                Class = ClassTextBox.Text.Trim(),
                Phone = PhoneTextBox.Text.Trim(),
                Email = EmailTextBox.Text.Trim()
            };

            // Додавання до бази даних
            _context.readers.Add(newReader);
            _context.SaveChanges();

            MessageBox.Show("Reader added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close(); // Закриваємо вікно після успішного додавання
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving reader: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
        this.Close(); // Закриваємо вікно без збереження
    }

    // Метод для валідації електронної пошти
    private bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
