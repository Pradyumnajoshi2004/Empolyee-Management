using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpolyeeManagement.Model
{
    [Table("Employee")]
    public class Employee
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string EmployeeName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]

        public string Email { get; set; } = string.Empty;

        public int DesignationId { get; set; }
    }
}
