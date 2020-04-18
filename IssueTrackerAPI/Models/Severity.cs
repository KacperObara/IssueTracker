using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTrackerAPI.Models
{
    public class Severity
    {
        [Key]
        public int SeverityId { get; set; }
        [Required(ErrorMessage = "The severity name field is required.")]
        [StringLength(255)]
        [Display(Name = "Severity name")]
        public string SeverityName { get; set; }

        public ICollection<Issue> Issues { get; set; }
    }
}
