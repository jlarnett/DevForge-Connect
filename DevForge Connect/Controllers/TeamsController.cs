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
using DevForge_Connect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DevForge_Connect.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public TeamsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Teams
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(currentUser!);

            var teams = await _context.Teams.Include(t => t.UserTeams).ToListAsync();
            var myTeams = new List<Team>();

            foreach (var team in teams)
            {
                if (team.UserTeams.Any(p => p.UserId.Equals(userId)))
                {
                    myTeams.Add(team);
                }
            }
            return View(myTeams);
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id, string errorMessage = "")
        {
            if (errorMessage != "")
            {
                ModelState.AddModelError("", errorMessage);
            }

            if (id == null) return NotFound();

            var team = await _context.Teams.Include(m => m.UserTeams)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (team == null)
            {
                return NotFound();
            }

            foreach (var userTeam in team.UserTeams)
            {
                team.Users.Add(await _userManager.FindByIdAsync(userTeam.UserId));
            }

            var fullUserList = await _userManager.Users.ToListAsync();
            var pendingInvites =
                await _context.TeamInvites.Where(i => i.TeamId.Equals(team.Id) && i.StatusId.Equals(1)).Include(i => i.User).Include(i => i.Status).ToListAsync();

            var vm = new TeamDetailsVm()
            {
                Id = team.Id,
                Users = team.Users,
                Name = team.Name,
                UserTeams = team.UserTeams,
                fullUserList = fullUserList,
                pendingInvites = pendingInvites
            };

            return View(vm);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name")] Team team)
        {
            if (ModelState.IsValid)
            {
                //Get the user creating the team
                var currentUser = await _userManager.GetUserAsync(User);
                var userId = await _userManager.GetUserIdAsync(currentUser!);

                _context.Add(team);
                var numberOfTeamDbChanges = await _context.SaveChangesAsync();

                //Try to add the current user to the newly created team.
                _context.Add(new UserTeam()
                {
                    UserId = userId,
                    TeamId = team.Id
                });

                var numberOfUserTeamDbChanges = await _context.SaveChangesAsync();

                if (numberOfTeamDbChanges < 1)
                {
                    return BadRequest("Problem adding team to DB");
                }
                if (numberOfUserTeamDbChanges < 1)
                {
                    return BadRequest("Problem mapping user to team in DB");
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Team team)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.Id))
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
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                _context.Teams.Remove(team);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}
