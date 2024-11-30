using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevForge_Connect.Data;
using DevForge_Connect.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DevForge_Connect.Entities.Identity;
using DevForge_Connect.Services.NLP_Translator;
using System.Text.Json;

namespace DevForge_Connect.Controllers
{
    [Authorize]
    public class UserProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITranslator _nlTranslator;

        public UserProfilesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ITranslator nlTranslator)
        {
            _context = context;
            _userManager = userManager;
            _nlTranslator = nlTranslator;
        }

        // GET: UserProfiles
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfile.Include(u => u.User).FirstOrDefaultAsync(u => u.UserId == userId);

            if (userProfile == null)
            {
                var default_PFP_Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "default-pfp.jpeg");
                byte[] defaultPFP = System.IO.File.ReadAllBytes(default_PFP_Path);

                userProfile = new UserProfile
                {
                    UserId = userId,
                    Bio = "",
					Expirience = "",
                    ProfilePicture = defaultPFP
                };

                _context.UserProfile.Add(userProfile);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(userProfile);
        }

        // GET: UserProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfile
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // GET: UserProfiles/Create
        public IActionResult Create()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userProfile = new UserProfile
            {
                UserId = currentUserId
            };

            return View(userProfile);
        }

        // POST: UserProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Bio,Expirience,ProfilePicture")] UserProfile userProfile, IFormFile profilePicture)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = _userManager.GetUserId(User);
            if (currentUser != null)
                userProfile.UserId = currentUser.Id;


            if (profilePicture != null && profilePicture.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await profilePicture.CopyToAsync(memoryStream);
                    userProfile.ProfilePicture = memoryStream.ToArray();
                }
            }
            else
            {
                ModelState.Remove("ProfilePicture");
            }

            if (ModelState.IsValid)
            {
                _context.Add(userProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(userProfile);
        }

        // GET: UserProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfile.FindAsync(id);
            if (userProfile == null || userProfile.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // POST: UserProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserProfile updatedProfile, IFormFile profilePicture)
        {
            if (id != updatedProfile.Id)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var userProfile = await _context.UserProfile.FirstOrDefaultAsync(u => u.Id == id && u.UserId == currentUser.Id);

            if (userProfile == null)
            {
                return Unauthorized();
            }

            userProfile.Bio = updatedProfile.Bio;
            userProfile.Expirience = updatedProfile.Expirience;

            if (profilePicture != null && profilePicture.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await profilePicture.CopyToAsync(memoryStream);
                    userProfile.ProfilePicture = memoryStream.ToArray();
                }
            }
            else
            {
                ModelState.Remove("ProfilePicture");
            }



            if (ModelState.IsValid)
            {
                try
                {
                    string userNlpTags = await _nlTranslator.GetNlpTags([userProfile.Expirience]);
					var outerArray = JsonSerializer.Deserialize<string[][]>(userNlpTags);
                    var innerArray = outerArray[0];
                    string tag = "";
                    foreach (var item in innerArray)
                    {
                        tag = tag + "|" + item;
                    }
                    userProfile.NlpTags = tag;
					_context.Update(userProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProfileExists(userProfile.Id))
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
            
            return View(userProfile);
        }

        // GET: UserProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfile
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // POST: UserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userProfile = await _context.UserProfile.FindAsync(id);
            if (userProfile != null)
            {
                _context.UserProfile.Remove(userProfile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserProfileExists(int id)
        {
            return _context.UserProfile.Any(e => e.Id == id);
        }

        public IActionResult GetProfilePicture(string userId)
        {
            var userProfile = _context.UserProfile.FirstOrDefault(u => u.UserId == userId);

            if (userProfile != null && userProfile.ProfilePicture != null)
            {
                return File(userProfile.ProfilePicture, "image/jpeg");
            }

            return File("~/images/default-pfp.jpeg", "image/jpeg");
        }
    }
}
