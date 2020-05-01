namespace IssueTrackerAPI.Models
{
    public class ProjectMember
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
