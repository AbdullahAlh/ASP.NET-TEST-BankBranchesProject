using System.ComponentModel.DataAnnotations;

namespace BankBranches.Models
{
    public class NewBranchForm

    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int EmployeeCount { get; set; }
       
        [Url]

        public string Location { get; set; }
        [Required]
        public string BranchManager { get; set; }
    }
}
