using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IssueTrackerAPI.Data;
using IssueTrackerAPI.Models;

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
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["AuthorSortParm"] = sortOrder == "Author" ? "author_desc" : "Author";
            ViewData["ProjectSortParm"] = sortOrder == "Project" ? "project_desc" : "Project";
            ViewData["SeveritySortParm"] = sortOrder == "Severity" ? "severity_desc" : "Severity";
            ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";
            ViewData["CurrentFilter"] = filter;

            var issues = from i in _context.Issues select i;

            // Filtering
            if (!String.IsNullOrEmpty(filter))
            {
                issues = issues.Where(i => i.Title.Contains(filter));
            }

            // Sorting
            switch (sortOrder)
            {
                case "title_desc":
                    issues = issues.OrderByDescending(i => i.Title);
                    break;

                case "Date":
                    issues = issues.OrderBy(i => i.CreationDate);
                    break;
                case "date_desc":
                    issues = issues.OrderByDescending(i => i.CreationDate);
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
        public async Task<IActionResult> Create([Bind("IssueId,Title,Description,CreationDate,AuthorId,ProjectId,SeverityId,StatusId")] Issue issue)
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
            ViewData["AuthorId"] = new SelectList(_context.People, "PersonId", "Email", issue.AuthorId);
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
        public async Task<IActionResult> Edit(int id, [Bind("IssueId,Title,Description,CreationDate,AuthorId,ProjectId,SeverityId,StatusId")] Issue issue)
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
            ViewData["AuthorId"] = new SelectList(_context.People, "PersonId", "Email", issue.AuthorId);
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
    }
}
