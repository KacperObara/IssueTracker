using IssueTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTrackerAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.People.Any())
            {
                return; 
            }

            var people = new Person[]
            {
                new Person{PersonId = 1, FirstName = "Jan", LastName = "Kowalski", Email = "test@test.com"},
                new Person{PersonId = 2, FirstName = "Emil", LastName = "Nowak", Email = "test2@test.com"}
            };

            foreach (Person p in people)
            {
                context.People.Add(p);
            }
            context.SaveChanges();

            var severities = new Severity[]
{
                new Severity{SeverityId = 1, SeverityName = "Minor"},
                new Severity{SeverityId = 2, SeverityName = "Major"}
};

            foreach (Severity s in severities)
            {
                context.Severities.Add(s);
            }
            context.SaveChanges();

            Console.WriteLine("WTF");
        }
    }
}
