using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TennisApp2.Data;
using TennisApp2.Models;

namespace TennisApp2.Controllers
{
    public class CoachesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CoachesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Coaches
        public async Task<IActionResult> Index()
        {
            //return _context.Coach != null ? 
            //            View(await _context.Coach.ToListAsync()) :
            //            Problem("Entity set 'ApplicationDbContext.Coach'  is null.");

            if (_context.Coach == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Item' is null.");
            }

            var coaches = from m in _context.Coach
                        select m;

            return View(await coaches.ToListAsync());
        }

        //GET: Coaches/CoachHome
       [Authorize]
        public async Task<IActionResult> CoachHome()
        {
            if (_context.Member == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Item' is null.");
            }

            var loggedInUser = await _userManager.GetUserAsync(User);

            var CoachSchedules = from s in _context.Schedule
                                 where s.UserName == loggedInUser.Email
                                 select s;
            return View(await CoachSchedules.ToListAsync());
        }

        //public async Task<IActionResult> CoachHome(int? id)
        //{
        //    var loggedInUser = await _userManager.GetUserAsync(User);

        //    if (!User.IsInRole("Coach") && !User.IsInRole("Admin"))
        //    {
        //        return View("AccessDeniedPartial");
        //    }

        //    if (id == null || _context.Member == null)
        //    {
        //        return NotFound();
        //    }

        //    //var member = await _context.Schedule
        //    //    .FirstOrDefaultAsync(m => m.Id == id);
        //    //if (member == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    //return View(await member.ToListAsync());

        //    var CoachSchedules = from s in _context.Schedule
        //                         where s.Id == id
        //                         select s;
        //    return View(await CoachSchedules.ToListAsync());

        //    //var schedules = await _context.Schedule.ToListAsync(); // Fetch all schedules
        //    //return View(schedules);
        //}

        // GET: Coaches/CoachBio
        public async Task<IActionResult> CoachBio()
        {
            //if (_context.Member == null)
            //{
            //    return Problem("Entity set 'ApplicationDbContext.Item' is null.");
            //}

            //var loggedInUser = await _userManager.GetUserAsync(User);

            //if (!User.IsInRole("Coach") || !User.IsInRole("Admin"))
            //{
            //    return View("AccessDeniedPartial");
            //}

            //var CoachBio = from s in _context.Coach
            //                     where s.UserName == loggedInUser.Email
            //                     select s;
            //return View(await CoachBio.ToListAsync());
            if (_context.Coach == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Item' is null.");
            }

            var coaches = from m in _context.Coach
                          select m;

            return View(await coaches.ToListAsync());
        }

        // GET: Coaches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }
            
            if (id == null || _context.Coach == null)
            {
                return NotFound();
            }

            var coach = await _context.Coach
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coach == null)
            {
                return NotFound();
            }

            return View(coach);
        }

        // GET: Coaches/Create
        public async Task<IActionResult> Create()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }
            return View();
        }

        // POST: Coaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Image,Biography,Expertise,Accreditations,IsDeleted")] Coach coach)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            if (ModelState.IsValid)
            {
                // Check if the submitted UserName exists in dbo.AspNetUsers
                var userExists = await _userManager.FindByNameAsync(coach.UserName);

                if (userExists == null)
                {
                    return View("CreateError");
                }

                    // Check if the user is not in the "Coach" role
    if (!await _userManager.IsInRoleAsync(userExists, "Coach"))
    {
        return View("CreateError");
    }

                _context.Add(coach);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(coach);
        }

        // GET: Coaches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Coach") && !User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            if (id == null || _context.Coach == null)
            {
                return NotFound();
            }

            var coach = await _context.Coach.FindAsync(id);
            if (coach == null)
            {
                return NotFound();
            }
            return View(coach);
        }

        // POST: Coaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Image,Biography,Expertise,Accreditations,IsDeleted")] Coach coach)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Coach") && !User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            if (id != coach.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoachExists(coach.Id))
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
            return View(coach);
        }

        // GET: Coaches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            if (id == null || _context.Coach == null)
            {
                return NotFound();
            }

            var coach = await _context.Coach
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coach == null)
            {
                return NotFound();
            }

            return View(coach);
        }

        // POST: Coaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            if (_context.Coach == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Coach'  is null.");
            }
            var coach = await _context.Coach.FindAsync(id);
            if (coach != null)
            {
                _context.Coach.Remove(coach);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoachExists(int id)
        {
          return (_context.Coach?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
