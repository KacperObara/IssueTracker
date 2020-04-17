using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTrackerAPI.Models
{
    public class Person
    {
        [Key]
        public int Id_Issue { get; set; }
        [Required(ErrorMessage = "The First name field is required.")]
        [StringLength(255)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "The last name field is required.")]
        [StringLength(255)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; }
    }
}
