using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO
{
    public class Employee : BaseEntity
    {
        public int? IsAdmin { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        [MaxLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string AccountName { get; set; }
    }
}
