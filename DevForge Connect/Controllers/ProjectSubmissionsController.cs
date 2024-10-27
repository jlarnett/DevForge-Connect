using DevForge_Connect.Data;
using DevForge_Connect.Entities;
using DevForge_Connect.Entities.Identity;
using DevForge_Connect.Services.NLP_Translator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Identity.Client;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DevForge_Connect.Controllers
{
    public class ProjectSubmissionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITranslator _translator;

        public ProjectSubmissionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ITranslator translator)
        {
            _context = context;
            _userManager = userManager;
            _translator = translator;
        }


        // GET: ProjectSubmissions
        [Authorize]
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
                .Include(p => p.Bids)
                .Include(p => p.Status)
                .FirstOrDefaultAsync(m => m.Id == id);

            foreach (var bid in projectSubmission.Bids)
            {
                if (bid.StatusId != null)
                {
                    bid.Status = await _context.Statuses.FindAsync(bid.StatusId);
                }
            }

            if (projectSubmission == null)
            {
                return NotFound();
            }

            return View(projectSubmission);
        }

        // GET: ProjectSubmissions/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["creatorId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ProjectSubmissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Deadline,Funding, AIGeneratedSummary, NlpTags, creatorId,StatusId")] ProjectSubmission projectSubmission)
        {
            //Assign the current user to project submission
            projectSubmission.creatorId = _userManager.GetUserId(User);

            //Convert project description into the string array
            string[] stringArrayDescription= Regex.Split(projectSubmission.Description, @"(?<=[.!?])\s+");

            //Get the NLP tags associated with project submission and assign to project

            projectSubmission.NlpTags = await _translator.GetNlpTags(stringArrayDescription);

            if (ModelState.IsValid)
            {
                projectSubmission.StatusId = (await _context.Statuses.FirstOrDefaultAsync())!.Id;
                _context.Add(projectSubmission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["creatorId"] = new SelectList(_context.Users, "Id", "Id", projectSubmission.creatorId);
            return View(projectSubmission);
        }

        // GET: ProjectSubmissions/Edit/5
        [Authorize]
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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Deadline,Funding, AIGeneratedSummary, NlpTags, creatorId,StatusId")] ProjectSubmission projectSubmission)
        {
            if (id != projectSubmission.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                //Assign the current user to project submission
                projectSubmission.creatorId = _userManager.GetUserId(User);

                //Get the NLP tags associated with project submission and assign to project
                projectSubmission.NlpTags = await _translator.GetNlpTags(projectSubmission.Description);

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
        [Authorize]
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
        [Authorize]
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

        public class NlpResponse
        {
            public List<string> MyArray { get; set; }
        }


    }


}
