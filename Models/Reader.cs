using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LibraryManagementSystem.Models;

public class Reader
{
    [Key]
    [Column("readerid")] // Вказуємо назву стовпця у базі
    public int ReaderId { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("name")] // Вказуємо правильну назву стовпця
    public string Name { get; set; }

    [MaxLength(50)]
    [Column("class")] // Вказуємо правильну назву стовпця
    public string Class { get; set; }

    [Phone]
    [MaxLength(20)]
    [Column("phone")] // Вказуємо правильну назву стовпця
    public string Phone { get; set; }

    [EmailAddress]
    [MaxLength(100)]
    [Column("email")] // Вказуємо правильну назву стовпця
    public string Email { get; set; }
}
