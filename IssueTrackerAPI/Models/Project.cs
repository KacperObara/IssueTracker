using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IssueTrackerAPI.Models
{
    [Authorize]
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [Required(ErrorMessage = "The title field is required.")]
        [StringLength(255, MinimumLength = 3)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }
        public ICollection<Issue> Issues { get; set; }
        public ICollection<ProjectMember> ProjectMembers { get; set; }

        public int IssuesCount
        {
            get
            {
                if (Issues != null)
                    return Issues.Count;
                else
                    return -1;
            }
        }

        public Project()
        {
            this.CreationDate = DateTime.UtcNow;
        }
    }
}
