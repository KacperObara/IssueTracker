using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IssueTrackerAPI.Data;
using IssueTrackerAPI.Models;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace IssueTrackerAPI.Controllers
{
    public class IssuesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IssuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Issues
        public async Task<IActionResult> Index(string sortOrder, string filter)
        {
            var issues = from i in _context.Issues select i;

            issues = Filter(issues, filter);
            issues = Sort(issues, sortOrder);

            var applicationDbContext = issues.Include(i => i.Author).Include(i => i.Project).Include(i => i.Severity).Include(i => i.Status);
            return View(await applicationDbContext.ToListAsync());
        }

        private IQueryable<Issue> Filter(IQueryable<Issue> issues, string filter)
        {
            ViewData["CurrentFilter"] = filter;

            if (!String.IsNullOrEmpty(filter))
            {
                issues = issues.Where(i => i.Title.Contains(filter) || i.Project.Title.Contains(filter));
            }

            return issues;
        }

        private IQueryable<Issue> Sort(IQueryable<Issue> issues, string sortOrder)
        {
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["AuthorSortParm"] = sortOrder == "Author" ? "author_desc" : "Author";
            ViewData["ProjectSortParm"] = sortOrder == "Project" ? "project_desc" : "Project";
            ViewData["SeveritySortParm"] = sortOrder == "Severity" ? "severity_desc" : "Severity";
            ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";

            switch (sortOrder)
            {
                case "title_desc":
                    issues = issues.OrderByDescending(i => i.Title);
                    break;

                case "Date":
                    issues = issues.OrderBy(i => i.LastEditDate);
                    break;
                case "date_desc":
                    issues = issues.OrderByDescending(i => i.LastEditDate);
                    break;

                case "Author":
                    issues = issues.OrderBy(i => i.Author);
                    break;
                case "author_desc":
                    issues = issues.OrderByDescending(i => i.Author);
                    break;

                case "Project":
                    issues = issues.OrderBy(i => i.Project);
                    break;
                case "project_desc":
                    issues = issues.OrderByDescending(i => i.Project);
                    break;

                case "Severity":
                    issues = issues.OrderBy(i => i.Severity);
                    break;
                case "severity_desc":
                    issues = issues.OrderByDescending(i => i.Severity);
                    break;

                case "Status":
                    issues = issues.OrderBy(i => i.Status);
                    break;
                case "status_desc":
                    issues = issues.OrderByDescending(i => i.Status);
                    break;

                default:
                    issues = issues.OrderBy(i => i.Title);
                    break;
            }

            return issues;
        }

        public async Task<IActionResult> ProjectIssues(string filter, string ProjectDescription)
        {
            ViewData["Description"] = ProjectDescription;

            var issues = from i in _context.Issues select i;

            issues = Filter(issues, filter);

            var applicationDbContext = issues.Include(i => i.Author).Include(i => i.Project).Include(i => i.Severity).Include(i => i.Status);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Issues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issues
                .Include(i => i.Author)
                .Include(i => i.Project)
                .Include(i => i.Severity)
                .Include(i => i.Status)
                .FirstOrDefaultAsync(m => m.IssueId == id);

            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        // GET: Issues/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.People, "PersonId", "FullName");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title");
            ViewData["SeverityId"] = new SelectList(_context.Severities, "SeverityId", "SeverityName");
            return View();
        }

        // POST: Issues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IssueId,Title,Description,LastEditDate,AuthorId,ProjectId,SeverityId,StatusId")] Issue issue)
        {
            issue.StatusId = _context.Statuses.Single(s => s.StatusName == "Open").StatusId;
            if (ModelState.IsValid)
            {
                _context.Add(issue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.People, "PersonId", "FullName", issue.AuthorId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title", issue.ProjectId);
            ViewData["SeverityId"] = new SelectList(_context.Severities, "SeverityId", "SeverityName", issue.SeverityId);
            return View(issue);
        }

        // GET: Issues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issues.FindAsync(id);

            if (issue == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.People, "PersonId", "FullName", issue.AuthorId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title", issue.ProjectId);
            ViewData["SeverityId"] = new SelectList(_context.Severities, "SeverityId", "SeverityName", issue.SeverityId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusName", issue.StatusId);
            return View(issue);
        }

        // POST: Issues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IssueId,Title,Description,LastEditDate,AuthorId,ProjectId,SeverityId,StatusId")] Issue issue)
        {
            if (id != issue.IssueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(issue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssueExists(issue.IssueId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.People, "PersonId", "FullName", issue.AuthorId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title", issue.ProjectId);
            ViewData["SeverityId"] = new SelectList(_context.Severities, "SeverityId", "SeverityName", issue.SeverityId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusName", issue.StatusId);
            return View(issue);
        }

        // GET: Issues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issues
                .Include(i => i.Author)
                .Include(i => i.Project)
                .Include(i => i.Severity)
                .Include(i => i.Status)
                .FirstOrDefaultAsync(m => m.IssueId == id);

            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var issue = await _context.Issues.FindAsync(id);
            _context.Issues.Remove(issue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IssueExists(int id)
        {
            return _context.Issues.Any(e => e.IssueId == id);
        }

        public IActionResult ExportToExcel()
        {
            var issues = _context.Issues
                .Include(i => i.Author)
                .Include(i => i.Project)
                .Include(i => i.Severity)
                .Include(i => i.Assignees)
                .Include(i => i.Status).ToList();

            byte[] fileContents;

            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Issues");
            Sheet.Cells["A1"].Value = "Title";
            Sheet.Cells["B1"].Value = "Description";
            Sheet.Cells["C1"].Value = "Last edit date";
            Sheet.Cells["D1"].Value = "Author";
            Sheet.Cells["E1"].Value = "Project";
            Sheet.Cells["F1"].Value = "Assignees";
            Sheet.Cells["G1"].Value = "Severity";
            Sheet.Cells["H1"].Value = "Status";


            int row = 2;
            foreach (var item in issues)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.Title;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.Description;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.LastEditDate.ToString();
                Sheet.Cells[string.Format("D{0}", row)].Value = item.Author.FullName;
                Sheet.Cells[string.Format("E{0}", row)].Value = item.Project.Title;

                // TODO: Fix error null Person
                //string assignees = "";
                //foreach (Assignee assignee in item.Assignees)
                //{
                //    assignees += assignee.Person.FullName + "\n";
                //}
                //Sheet.Cells[string.Format("F{0}", row)].Value = assignees;

                Sheet.Cells[string.Format("G{0}", row)].Value = item.Severity.SeverityName;
                Sheet.Cells[string.Format("H{0}", row)].Value = item.Status.StatusName;
                row++;
            }


            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            fileContents = Ep.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "Issues.xlsx"
            );
        }
    }
}
