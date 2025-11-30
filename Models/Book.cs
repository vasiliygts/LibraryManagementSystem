using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace LibraryManagementSystem.Models;

public class Book
{
    //public int BookId { get; set; }
    [Key]
    [Column("bookid")] // Вказуємо назву стовпця у базі
    public int BookId { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("title")] // Вказуємо, якщо назва стовпця також у нижньому регістрі
    public string Title { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("author")] // Вказуємо правильну назву стовпця
    public string Author { get; set; }

    [MaxLength(13)]
    [Column("isbn")]
    public string ISBN { get; set; }

    [MaxLength(100)]
    [Column("genre")]
    public string Genre { get; set; }

    [Column("year")]
    public int Year { get; set; }


    [Column("isavailable")]
    public bool IsAvailable { get; set; } = true;
}
