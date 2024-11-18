using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevForge_Connect.Data;
using DevForge_Connect.Entities;
using Microsoft.AspNetCore.Components.RenderTree;

namespace DevForge_Connect.Controllers
{
    public class TeamInvitesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamInvitesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TeamInvites
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TeamInvites.Include(t => t.InvitingTeam).Include(t => t.Status);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TeamInvites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamInvite = await _context.TeamInvites
                .Include(t => t.InvitingTeam)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamInvite == null)
            {
                return NotFound();
            }

            return View(teamInvite);
        }

        // GET: TeamInvites/Create
        public IActionResult Create()
        {
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Id");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id");
            return View();
        }

        // POST: TeamInvites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TeamId,UserId,StatusId,LastUpdatedDate")] TeamInvite teamInvite)
        {
            //Needs to be review in DB
            teamInvite.StatusId = 1;

            if (ModelState.IsValid)
            {
                //Verify user isn't already part of the team. 
                //Verify that a team invite doesn't alreayd exists

                var userAlreadyInTeam = await _context.UserTeams
                    .Where(ut => ut.TeamId.Equals(teamInvite.TeamId) && ut.UserId.Equals(teamInvite.UserId)).AnyAsync();

                var teamInviteAlreadySent = await _context.TeamInvites.Where(ti =>
                        ti.TeamId.Equals(teamInvite.TeamId) && ti.UserId.Equals(teamInvite.UserId) &&
                        ti.StatusId.Equals(1))
                    .AnyAsync();

                if (userAlreadyInTeam)
                {
                    return RedirectToAction("Details", "Teams", new {id=teamInvite.TeamId, errorMessage="supplied user is already a team member"} );
                }

                if (teamInviteAlreadySent)
                {
                    return RedirectToAction("Details", "Teams", new {id=teamInvite.TeamId, errorMessage="team invite already sent"} );
                }

                _context.Add(teamInvite);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Teams", new {id=teamInvite.TeamId} );
            }

            return RedirectToAction("Details", "Teams", new {id=teamInvite.TeamId, errorMessage="error sending team invite, user not defined"} );
        }

        public async Task<IActionResult> AcceptTeamInvite(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamInvite = await _context.TeamInvites.FindAsync(id);

            if (teamInvite == null)
            {
                return NotFound();
            }

            teamInvite.StatusId = 2;
            await _context.SaveChangesAsync();

            await _context.UserTeams.AddAsync(new UserTeam()
            {
                TeamId = teamInvite.TeamId,
                UserId = teamInvite.UserId
            });

            var changes = await _context.SaveChangesAsync();

            if (changes > 0)
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }

            return BadRequest();
        }

        public async Task<IActionResult> DeclineTeamInvite(int? id)
        {
            if (id == null)
                return NotFound();

            var teamInvite = await _context.TeamInvites.FindAsync(id);

            if (teamInvite == null)
                return NotFound();

            //Change the statusId to 3 declined
            teamInvite.StatusId = 3;
            var changes = await _context.SaveChangesAsync();

            if (changes > 0)
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }

            return BadRequest();
        }

        // GET: TeamInvites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamInvite = await _context.TeamInvites.FindAsync(id);
            if (teamInvite == null)
            {
                return NotFound();
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Id", teamInvite.TeamId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", teamInvite.StatusId);
            return View(teamInvite);
        }

        // POST: TeamInvites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeamId,UserId,StatusId,LastUpdatedDate")] TeamInvite teamInvite)
        {
            if (id != teamInvite.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teamInvite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamInviteExists(teamInvite.Id))
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
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Id", teamInvite.TeamId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", teamInvite.StatusId);
            return View(teamInvite);
        }

        // GET: TeamInvites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamInvite = await _context.TeamInvites
                .Include(t => t.InvitingTeam)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamInvite == null)
            {
                return NotFound();
            }

            return View(teamInvite);
        }

        // POST: TeamInvites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamInvite = await _context.TeamInvites.FindAsync(id);
            if (teamInvite != null)
            {
                _context.TeamInvites.Remove(teamInvite);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamInviteExists(int id)
        {
            return _context.TeamInvites.Any(e => e.Id == id);
        }
    }
}
