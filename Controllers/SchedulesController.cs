using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TennisApp2.Data;
using TennisApp2.Models;

namespace TennisApp2.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SchedulesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
              return _context.Schedule != null ? 
                          View(await _context.Schedule.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Schedule'  is null.");
        }

        // GET: Schedules/ScheduleMembers/5
        //public async Task<IActionResult> ScheduleMembers(int? id)
        //{
        //    if (id == null || _context.Schedule == null)
        //    {
        //        return NotFound();
        //    }

        //    // Find the target Schedule by ID
        //    var targetSchedule = await _context.Schedule
        //        .FirstOrDefaultAsync(schedule => schedule.Id == id);

        //    if (targetSchedule == null)
        //    {
        //        return NotFound();
        //    }

        //    // Retrieve the members associated with the target Schedule
        //    var membersForSchedule = targetSchedule.Member.ToList();

        //    // Pass the target Schedule and its associated members to the view
        //    var viewModel = new ScheduleMembersViewModel
        //    {
        //        Schedule = targetSchedule,
        //        Members = membersForSchedule
        //    };

        //    return View(viewModel);
        //}


        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Schedule == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        public async Task<IActionResult> Create()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,LessonName,LessonStartDate,LessonEndDate,Location,IsDeleted")] Schedule schedule)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            if (ModelState.IsValid)
            {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            if (id == null || _context.Schedule == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,LessonName,LessonStartDate,LessonEndDate,Location,IsDeleted")] Schedule schedule)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            if (id != schedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
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
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            if (id == null || _context.Schedule == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            if (_context.Schedule == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Schedule'  is null.");
            }
            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedule.Remove(schedule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
          return (_context.Schedule?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
