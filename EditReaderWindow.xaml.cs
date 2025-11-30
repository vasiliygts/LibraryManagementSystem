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

public partial class EditReaderWindow : Window
{
    private readonly LibraryContext _context;
    private readonly Reader _reader;

    public EditReaderWindow(LibraryContext context, Reader reader)
    {
        InitializeComponent();
        _context = context;
        _reader = reader;

        // Заповнення текстових полів існуючими даними
        NameTextBox.Text = _reader.Name;
        ClassTextBox.Text = _reader.Class;
        PhoneTextBox.Text = _reader.Phone;
        EmailTextBox.Text = _reader.Email;
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

            // Оновлення даних читача
            _reader.Name = NameTextBox.Text.Trim();
            _reader.Class = ClassTextBox.Text.Trim();
            _reader.Phone = PhoneTextBox.Text.Trim();
            _reader.Email = EmailTextBox.Text.Trim();

            // Збереження змін у базі
            _context.SaveChanges();

            MessageBox.Show("Reader updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close(); // Закриваємо вікно після успішного оновлення
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
