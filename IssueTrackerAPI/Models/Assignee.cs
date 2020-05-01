namespace IssueTrackerAPI.Models
{
    public class Assignee
    {
        public int IssueId { get; set; }
        public Issue Issue { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
