using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication.Common;

namespace WebApplication.Entity;

[Table("student")]
public class Student : IEntity
{ 
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
                
    [Column("name")]
    public string Name { get; set; }
        
    [Column("age")]
    public int Age { get; set; }
}