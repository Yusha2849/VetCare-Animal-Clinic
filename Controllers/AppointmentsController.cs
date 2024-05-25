using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using VetCare_Animal_Clinic.Areas.Data;
using VetCare_Animal_Clinic.Data;
using VetCare_Animal_Clinic.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace VetCare_Animal_Clinic.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AppointmentsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userIsReceptionist = User.IsInRole("Receptionist");

            var applicationDbContext = userIsReceptionist
                ? _context.Appointment.Include(a => a.Pet)
                : _context.Appointment.Where(a => a.Pet.UserId == userId).Include(a => a.Pet);

            return View(await applicationDbContext.ToListAsync());
        }

        /*public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Appointment.Include(a => a.Pet);
            return View(await applicationDbContext.ToListAsync());
        }*/

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .Include(a => a.Pet)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }
        // GET: Appointments/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User); // Get the currently logged-in user
            if (user != null)
            {
                ViewData["PetID"] = User.IsInRole("Receptionist")
                    ? new SelectList(_context.Pet, "PetId", "PetName")
                    : new SelectList(_context.Pet.Where(p => p.UserId == user.Id), "PetId", "PetName");

                var appointment = new Appointment
                {
                    ADate = DateTime.Today // Set the default date to today
                };

                return View(appointment);
            }

            // Handle the case when the user is not found
            ViewData["PetID"] = new SelectList(_context.Pet, "PetId", "PetName");
            return View(new Appointment());
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentID,ADate,ATime,PetID,Appointment_Reason,AppointmentStatus")] Appointment appointment)
        {
            appointment.AppointmentStatus = AppointmentStatus.Unseen;
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate the PetID dropdown based on the user's role
            var user = await _userManager.GetUserAsync(User);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userIsReceptionist = User.IsInRole("Receptionist");

            ViewData["PetID"] = userIsReceptionist
                ? new SelectList(_context.Pet, "PetId", "PetName", appointment.PetID)
                : new SelectList(_context.Pet.Where(p => p.UserId == userId), "PetId", "PetName", appointment.PetID);

            return View(appointment);
        }



        // GET: Appointments/Create
        /*public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User); // Get the currently logged-in user
            if (user != null)
            {
                // Query the Pet database to retrieve the PetId for the user
                var petId = _context.Pet.FirstOrDefault(p => p.UserId == user.Id)?.PetId;

                if (petId.HasValue)
                {
                    // Populate the Appointment model with the PetId
                    var appointment = new Appointment
                    {
                        PetID = petId.Value,
                        ADate = DateTime.Today // Set the default date to today
                    };

                    ViewData["PetID"] = new SelectList(_context.Pet, "PetId", "PetName", appointment.PetID);
                    return View(appointment);
                }
            }

            // Handle the case when the user is not found or doesn't have a pet
            ViewData["PetID"] = new SelectList(_context.Pet, "PetId", "PetName");
            return View(new Appointment());
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentID,ADate,ATime,PetID,Appointment_Reason,AppointmentStatus")] Appointment appointment)
        {
            appointment.AppointmentStatus = AppointmentStatus.Unseen;
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PetID"] = new SelectList(_context.Pet, "PetId", "PetName", appointment.PetID);
            return View(appointment);
        }*/

        [Authorize(Roles = "Receptionist")]
        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Appointment == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["PetID"] = new SelectList(_context.Pet, "PetId", "AnimalType", appointment.PetID);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentID,ADate,ATime,PetID,Appointment_Reason,AppointmentStatus")] Appointment appointment)
        {
            if (id != appointment.AppointmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointmentID))
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
            ViewData["PetID"] = new SelectList(_context.Pet, "PetId", "AnimalType", appointment.PetID);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Appointment == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .Include(a => a.Pet)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Appointment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Appointment'  is null.");
            }
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointment.Remove(appointment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
          return (_context.Appointment?.Any(e => e.AppointmentID == id)).GetValueOrDefault();
        }
    }
}
