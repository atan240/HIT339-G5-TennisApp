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
using TennisApp2.ViewModels;

namespace TennisApp2.Controllers
{
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MembersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Members
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }
            //// Retrieve all users from the AspNetUsers table
            //var users = await _userManager.Users.ToListAsync();

            //return View(users);

            // Fetch all users and their roles
            var usersWithRoles = (from user in _context.Users
                                  select new
                                  {
                                      user.Id,
                                      user.UserName,
                                      Roles = (from userRole in _context.UserRoles
                                               join role in _context.Roles on userRole.RoleId equals role.Id
                                               where userRole.UserId == user.Id
                                               select role.Name).ToList()
                                  }).ToList();


            var viewModel = new List<UserRoleViewModel>();

            foreach (var user in usersWithRoles)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Roles = user.Roles
                };

                viewModel.Add(userRoleViewModel);
            }

            return View(viewModel);
        }

        // Function to update role in Members/Index for Admin view only
        [HttpPost]
        public async Task<IActionResult> UpdateRole(string userId, string selectedRole)
        {
            // Find the user by their userId
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Get the current roles for the user
            var userRoles = await _userManager.GetRolesAsync(user);

            // Remove the user from their current roles
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            // Add the user to the selected role
            await _userManager.AddToRoleAsync(user, selectedRole);

            return RedirectToAction("Index");
        }



        // GET: Members/MemberHome
        [Authorize]
        //public async Task<IActionResult> MemberHome()
        //{
        //    if (_context.Member == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.Item' is null.");
        //    }

        //    var loggedInUser = await _userManager.GetUserAsync(User);

        //    var schedules = from s in _context.Schedule
        //                    join sj in _context.ScheduleJoin on s.Id equals sj.ScheduleId
        //                    join m in _context.Member on sj.MemberId equals m.Id
        //                    where m.UserName == loggedInUser.Email
        //                    select s;
        //    return View(await schedules.ToListAsync());
        //}

        // GET: Members/MemberHome to show the schedules for specific logged in user and admins
        public async Task<IActionResult> MemberHome(int? id)
        {        
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                var loggedInUserId = (from m in _context.Member
                                      where m.UserName == loggedInUser.Email
                                      select m.Id).FirstOrDefault();

                ViewBag.LoggedInUserId = loggedInUserId;

                if (id != loggedInUserId)
                {
                    return View("AccessDeniedPartial");
                }
            }

            if (id == null || _context.Member == null)
            {
                return NotFound();
            }

            var schedules = from s in _context.Schedule
                            join sj in _context.ScheduleJoin on s.Id equals sj.ScheduleId
                            join m in _context.Member on sj.MemberId equals m.Id
                            where sj.MemberId == id
                            select s;

            return View(await schedules.ToListAsync());

        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Coach") && !User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            if (id == null || _context.Member == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public async Task<IActionResult> Create()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,UserName")] Member member)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            if (id == null || _context.Member == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,UserName")] Member member)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
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
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            if (id == null || _context.Member == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin"))
            {
                return View("AccessDeniedPartial");
            }

            if (_context.Member == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Member'  is null.");
            }
            var member = await _context.Member.FindAsync(id);
            if (member != null)
            {
                _context.Member.Remove(member);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
          return (_context.Member?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
