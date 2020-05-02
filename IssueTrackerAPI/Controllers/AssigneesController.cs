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
    public class AssigneesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssigneesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Assignees
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Assignees.Include(a => a.Issue).Include(a => a.Person);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Assignees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignee = await _context.Assignees
                .Include(a => a.Issue)
                .Include(a => a.Person)
                .FirstOrDefaultAsync(m => m.IssueId == id);
            if (assignee == null)
            {
                return NotFound();
            }

            return View(assignee);
        }

        // GET: Assignees/Create
        public IActionResult Create()
        {
            ViewData["IssueId"] = new SelectList(_context.Issues, "IssueId", "Title");
            ViewData["PersonId"] = new SelectList(_context.People, "PersonId", "FullName");
            return View();
        }

        // POST: Assignees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IssueId,PersonId")] Assignee assignee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IssueId"] = new SelectList(_context.Issues, "IssueId", "Title", assignee.IssueId);
            ViewData["PersonId"] = new SelectList(_context.People, "PersonId", "FullName", assignee.PersonId);
            return View(assignee);
        }

        // GET: Assignees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignee = await _context.Assignees.FindAsync(id);
            if (assignee == null)
            {
                return NotFound();
            }
            ViewData["IssueId"] = new SelectList(_context.Issues, "IssueId", "Title", assignee.IssueId);
            ViewData["PersonId"] = new SelectList(_context.People, "PersonId", "FullName", assignee.PersonId);
            return View(assignee);
        }

        // POST: Assignees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IssueId,PersonId")] Assignee assignee)
        {
            if (id != assignee.IssueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssigneeExists(assignee.IssueId))
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
            ViewData["IssueId"] = new SelectList(_context.Issues, "IssueId", "Title", assignee.IssueId);
            ViewData["PersonId"] = new SelectList(_context.People, "PersonId", "FullName", assignee.PersonId);
            return View(assignee);
        }

        // GET: Assignees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignee = await _context.Assignees
                .Include(a => a.Issue)
                .Include(a => a.Person)
                .FirstOrDefaultAsync(m => m.IssueId == id);
            if (assignee == null)
            {
                return NotFound();
            }

            return View(assignee);
        }

        // POST: Assignees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignee = await _context.Assignees.FindAsync(id);
            _context.Assignees.Remove(assignee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssigneeExists(int id)
        {
            return _context.Assignees.Any(e => e.IssueId == id);
        }
    }
}
