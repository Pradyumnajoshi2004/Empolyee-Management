using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpolyeeManagement.Model
{

    [Table("Designation")]
    public class Designation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DesignationId { get; set; }

        [Required]
        public string DesignationName { get; set; } = string.Empty;

        [Required]
        public int DepartmentId { get; set; }

    }
}
