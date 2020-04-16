using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTrackerAPI.Models
{
    public class SeverityContext : DbContext
    {
        public SeverityContext(DbContextOptions<SeverityContext> qqq) :base(qqq)
        {

        }

        public DbSet<Severity> SeverityItems { get; set; }
    }
}
