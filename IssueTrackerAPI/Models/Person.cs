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
        public int IssueId { get; set; }
        [Required(ErrorMessage = "The First name field is required.")]
        [StringLength(255)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "The last name field is required.")]
        [StringLength(255)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; }

        public ICollection<Issue> ReportedIssues { get; set; }
        public ICollection<ProjectMember> ProjectMembers { get; set; }
        public ICollection<Assignee> Assignees { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
