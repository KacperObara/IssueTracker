using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IssueTrackerAPI.Models
{
    public class Status
    {
        [Key]
        public int StatusId { get; set; }
        [Required(ErrorMessage = "The status name field is required.")]
        [StringLength(255)]
        [Display(Name = "Status name")]
        public string StatusName { get; set; }

        public ICollection<Issue> Issues { get; set; }
    }
}
