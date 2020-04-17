using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTrackerAPI.Models
{
    public class Issue
    {
        [Key]
        public int Id_Issue { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }
    }
}
