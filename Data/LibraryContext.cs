using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagementSystem.Data;

public class LibraryContext : DbContext
{
    public DbSet<Book> books { get; set; }
    public DbSet<Reader> readers { get; set; }
    public DbSet<BorrowedBook> borrowedbooks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=123456")
        //.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information); // Додано логування SQL-запитів

        // Шлях до файлу бази даних у тій же папці, що і виконуваний файл
        string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "LibraryDB.sqlite");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().ToTable("books");
        modelBuilder.Entity<Reader>().ToTable("readers");
        modelBuilder.Entity<BorrowedBook>().ToTable("borrowedbooks");

        // Налаштовуємо генерацію значення для TransactionId
        modelBuilder.Entity<BorrowedBook>()
            .Property(bb => bb.TransactionId)
            .ValueGeneratedOnAdd(); // Вказуємо, що значення згенерується автоматично
    }
}



