using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IssueTrackerAPI.Models
{
    public class Issue
    {
        [Key]
        public int IssueId { get; set; }
        [Required(ErrorMessage = "The title field is required.")]
        [StringLength(255, MinimumLength = 3)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Last edit date")]
        public DateTime LastEditDate { get; set; }
        public int AuthorId { get; set; }
        public Person Author { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int SeverityId { get; set; }
        public Severity Severity { get; set; }
        public int StatusId { get; set; }
        public Status Status{ get; set; }

        public ICollection<Assignee> Assignees { get; set; }

        [Display(Name = "Assignees")]
        public string AssigneesNames
        {
            get
            {
                string assignees = "| ";
                foreach (Assignee assignee in Assignees)
                {
                    assignees += $"{assignee.Person.FullName} | ";
                }

                return assignees;
            }
        }

        public Issue()
        {
            this.LastEditDate = DateTime.UtcNow;
        }
    }
}
