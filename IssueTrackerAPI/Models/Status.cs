using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
