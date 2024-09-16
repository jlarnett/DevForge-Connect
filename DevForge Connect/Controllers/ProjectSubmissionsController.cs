using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevForge_Connect.Data;
using DevForge_Connect.Entities;

namespace DevForge_Connect.Controllers
{
    public class ProjectSubmissionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectSubmissionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectSubmissions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProjectSubmissions.Include(p => p.Creator);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProjectSubmissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectSubmission = await _context.ProjectSubmissions
                .Include(p => p.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectSubmission == null)
            {
                return NotFound();
            }

            return View(projectSubmission);
        }

        // GET: ProjectSubmissions/Create
        public IActionResult Create()
        {
            ViewData["creatorId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ProjectSubmissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Deadline,Funding,creatorId")] ProjectSubmission projectSubmission)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectSubmission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["creatorId"] = new SelectList(_context.Users, "Id", "Id", projectSubmission.creatorId);
            return View(projectSubmission);
        }

        // GET: ProjectSubmissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectSubmission = await _context.ProjectSubmissions.FindAsync(id);
            if (projectSubmission == null)
            {
                return NotFound();
            }
            ViewData["creatorId"] = new SelectList(_context.Users, "Id", "Id", projectSubmission.creatorId);
            return View(projectSubmission);
        }

        // POST: ProjectSubmissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Deadline,Funding,creatorId")] ProjectSubmission projectSubmission)
        {
            if (id != projectSubmission.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectSubmission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectSubmissionExists(projectSubmission.Id))
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
            ViewData["creatorId"] = new SelectList(_context.Users, "Id", "Id", projectSubmission.creatorId);
            return View(projectSubmission);
        }

        // GET: ProjectSubmissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectSubmission = await _context.ProjectSubmissions
                .Include(p => p.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectSubmission == null)
            {
                return NotFound();
            }

            return View(projectSubmission);
        }

        // POST: ProjectSubmissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectSubmission = await _context.ProjectSubmissions.FindAsync(id);
            if (projectSubmission != null)
            {
                _context.ProjectSubmissions.Remove(projectSubmission);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectSubmissionExists(int id)
        {
            return _context.ProjectSubmissions.Any(e => e.Id == id);
        }
    }
}
