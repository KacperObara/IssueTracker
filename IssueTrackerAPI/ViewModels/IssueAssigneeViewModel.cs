using IssueTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTrackerAPI.ViewModels
{
    public class IssueAssigneeViewModel
    {
        public Issue Issue { get; set; }
        public List<Person> People { get; set; }

        public bool IsAssigned(Person person)
        {
            foreach (Assignee assignee in Issue.Assignees)
            {
                if (assignee.Person == person)
                    return true;
            }

            return false;
        }
    }
}
