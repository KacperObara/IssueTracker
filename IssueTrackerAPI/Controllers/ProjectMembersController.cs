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
    public class ProjectMembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectMembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectMembers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProjectMembers.Include(p => p.Person).Include(p => p.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProjectMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectMember = await _context.ProjectMembers
                .Include(p => p.Person)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (projectMember == null)
            {
                return NotFound();
            }

            return View(projectMember);
        }

        // GET: ProjectMembers/Create
        public IActionResult Create()
        {
            ViewData["PersonId"] = new SelectList(_context.People, "PersonId", "Email");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title");
            return View();
        }

        // POST: ProjectMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,PersonId")] ProjectMember projectMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PersonId"] = new SelectList(_context.People, "PersonId", "Email", projectMember.PersonId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title", projectMember.ProjectId);
            return View(projectMember);
        }

        // GET: ProjectMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectMember = await _context.ProjectMembers.FindAsync(id);
            if (projectMember == null)
            {
                return NotFound();
            }
            ViewData["PersonId"] = new SelectList(_context.People, "PersonId", "Email", projectMember.PersonId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title", projectMember.ProjectId);
            return View(projectMember);
        }

        // POST: ProjectMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,PersonId")] ProjectMember projectMember)
        {
            if (id != projectMember.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectMemberExists(projectMember.ProjectId))
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
            ViewData["PersonId"] = new SelectList(_context.People, "PersonId", "Email", projectMember.PersonId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title", projectMember.ProjectId);
            return View(projectMember);
        }

        // GET: ProjectMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectMember = await _context.ProjectMembers
                .Include(p => p.Person)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (projectMember == null)
            {
                return NotFound();
            }

            return View(projectMember);
        }

        // POST: ProjectMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectMember = await _context.ProjectMembers.FindAsync(id);
            _context.ProjectMembers.Remove(projectMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectMemberExists(int id)
        {
            return _context.ProjectMembers.Any(e => e.ProjectId == id);
        }
    }
}
