using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LibraryManagementSystem.Models;

public class BorrowedBook
{
    [Key] // Вказуємо, що це первинний ключ
    [Column("transactionid")] // Назва стовпця у базі даних
    public int TransactionId { get; set; }

    [Required]
    [ForeignKey("Book")]
    [Column("bookid")] // Назва стовпця у базі даних
    public int BookId { get; set; }

    [Required]
    [ForeignKey("Reader")]
    [Column("readerid")] // Назва стовпця у базі даних
    public int ReaderId { get; set; }

    [Required]
    [Column("borrowdate")] // Назва стовпця у базі даних
    public DateTime BorrowDate { get; set; }

    [Column("returndate")] // Назва стовпця у базі даних
    public DateTime? ReturnDate { get; set; }

    // Навігаційні властивості
    public virtual Book Book { get; set; }
    public virtual Reader Reader { get; set; }
}
