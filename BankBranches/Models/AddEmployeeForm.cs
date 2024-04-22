using System.ComponentModel.DataAnnotations;

namespace BankBranches.Models
{
    public class AddEmployeeForm
    {
        [Required]
        public string Name { get; set; }


        [Required]

        public string Position { get; set; }

        [ValidCivilId]
        public string CivilId { get; set; }



    }
}
