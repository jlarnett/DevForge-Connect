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
    public class ProjectBidsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectBidsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectBids
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProjectBids.Include(p => p.Project).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProjectBids/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectBid = await _context.ProjectBids
                .Include(p => p.Project)
                .Include(p => p.User)
                .Include(p => p.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectBid == null)
            {
                return NotFound();
            }

            return View(projectBid);
        }

        // GET: ProjectBids/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.ProjectSubmissions, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ProjectBids/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OfferAmount,FinishDate,UserId,ProposalDescription,StatusId,ProjectId")] ProjectBid projectBid)
        {
            if (ModelState.IsValid)
            {
                projectBid.StatusId = (await _context.Statuses.FirstOrDefaultAsync())!.Id;
                _context.Add(projectBid);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.ProjectSubmissions, "Id", "Id", projectBid.ProjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", projectBid.UserId);
            return View(projectBid);
        }

        // GET: ProjectBids/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectBid = await _context.ProjectBids.FindAsync(id);
            if (projectBid == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.ProjectSubmissions, "Id", "Id", projectBid.ProjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", projectBid.UserId);
            return View(projectBid);
        }

        // POST: ProjectBids/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OfferAmount,FinishDate,UserId,ProposalDescription,StatusId,ProjectId")] ProjectBid projectBid)
        {
            if (id != projectBid.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectBid);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectBidExists(projectBid.Id))
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
            ViewData["ProjectId"] = new SelectList(_context.ProjectSubmissions, "Id", "Id", projectBid.ProjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", projectBid.UserId);
            return View(projectBid);
        }

        // GET: ProjectBids/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectBid = await _context.ProjectBids
                .Include(p => p.Project)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectBid == null)
            {
                return NotFound();
            }

            return View(projectBid);
        }

        // POST: ProjectBids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectBid = await _context.ProjectBids.FindAsync(id);
            if (projectBid != null)
            {
                _context.ProjectBids.Remove(projectBid);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectBidExists(int id)
        {
            return _context.ProjectBids.Any(e => e.Id == id);
        }
    }
}
