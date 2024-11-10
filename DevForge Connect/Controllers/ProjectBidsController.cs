using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevForge_Connect.Data;
using DevForge_Connect.Entities;
using DevForge_Connect.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace DevForge_Connect.Controllers
{
    public class ProjectBidsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectBidsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

            projectBid.StatusId = 2;
            await _context.SaveChangesAsync();
            return View(projectBid);
        }

        public async Task<IActionResult> Accept(int? id)
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
            foreach (var otherBid in _context.ProjectBids)
            {
                if (otherBid.ProjectId == projectBid.ProjectId && otherBid.Id != projectBid.Id)
                _context.ProjectBids.Remove(otherBid);
            }
            projectBid.StatusId = 3;
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "ProjectSubmissions", new { id = projectBid.ProjectId });
        }

        // GET: ProjectBids/Create
        public IActionResult Create()
        {
            var userTeams = _context.UserTeams.Where(ut => ut.UserId.Equals(_userManager.GetUserId(User))).Include(ut => ut.Team);

            List<Team> teams = new List<Team>();

            foreach (var ut in userTeams)
            {
                if(ut.Team != null)
                    teams.Add(ut.Team);
            }


            ViewData["TeamId"] = new SelectList(teams, "Id", "Name");
            ViewData["ProjectId"] = new SelectList(_context.ProjectSubmissions, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ProjectBids/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("OfferAmount,FinishDate,UserId,ProposalDescription,StatusId,ProjectId,TeamId")] ProjectBid projectBid, int? id)

        {
            if (projectBid.TeamId == -1)
            {
                projectBid.TeamId = null;
            }
            if (ModelState.IsValid)
            {
                projectBid.UserId = _userManager.GetUserId(User);
                projectBid.StatusId = (await _context.Statuses.FirstOrDefaultAsync())!.Id;
                projectBid.ProjectId = id;
                _context.Add(projectBid);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "ProjectSubmissions", new { id = id });
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

        public async Task<IActionResult> DeclineBid(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int? projectBidID = 0;
            var projectBid = await _context.ProjectBids.FindAsync(id);
            if (projectBid != null)
            {
                projectBidID = projectBid.ProjectId;
                _context.ProjectBids.Remove(projectBid);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "ProjectSubmissions", new { id = projectBidID });
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
