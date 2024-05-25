using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCare_Animal_Clinic.Data;
using VetCare_Animal_Clinic.Models;

namespace VetCare_Animal_Clinic.Controllers
{
    [Authorize]
    public class PetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pets
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userIsReceptionist = User.IsInRole("Receptionist");

            // If the user is a receptionist, show all pets; otherwise, show only their own pets
            var applicationDbContext = userIsReceptionist
                ? _context.Pet.Include(p => p.AnimalTypes).Include(p => p.AppUser)
                : _context.Pet.Where(p => p.UserId == userId)
                    .Include(p => p.AnimalTypes)
                    .Include(p => p.AppUser);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Pets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pet == null)
            {
                return NotFound();
            }

            var pet = await _context.Pet
                .Include(p => p.AnimalTypes)
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(m => m.PetId == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }
       
        
        // GET: Pets/Create
        public IActionResult Create()
        {
            // Set the UserId based on the currently logged-in user
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve a list of distinct AnimalTypes from the database
            var animalTypes = _context.AnimalTypes
                .Select(at => at.AnimalType)
                .Distinct()
                .ToList();

            // Create a list of SelectListItem for the AnimalType dropdown
            var animalTypeList = animalTypes.Select(animalType => new SelectListItem { Value = animalType, Text = animalType }).ToList();

            // Create an empty Pet object with the UserId set
            var pet = new Pet { UserId = userId };

            ViewData["AnimalType"] = new SelectList(animalTypeList, "Value", "Text");
            ViewData["UserId"] = new SelectList(new List<string> { userId }, userId, userId);

            
            return View(pet);
        }

        // POST: Pets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pet pet)
        {
            if (ModelState.IsValid)
            {
                // Set the UserId based on the currently logged-in user
                pet.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                _context.Add(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate the dropdown for AnimalType based on distinct values
            var animalTypes = _context.AnimalTypes
                .Select(at => at.AnimalType)
                .Distinct()
                .ToList();

            // Remove the initial "Select AnimalType" option
            var animalTypeList = animalTypes.Select(animalType => new SelectListItem { Value = animalType, Text = animalType }).ToList();

            ViewData["AnimalType"] = new SelectList(animalTypeList, "Value", "Text");

            return View(pet);
        }


        

        // GET: Pets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pet == null)
            {
                return NotFound();
            }

            var pet = await _context.Pet.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }
            ViewData["AnimalType"] = new SelectList(_context.AnimalTypes.Select(at => at.AnimalType).Distinct().ToList());
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", pet.UserId);
            return View(pet);
        }

        // POST: Pets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PetId,PetName,UserId,AnimalType,Breed,YearOfBirth")] Pet pet)
        {
            if (id != pet.PetId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetExists(pet.PetId))
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
            ViewData["AnimalType"] = new SelectList(_context.AnimalTypes.Select(at => at.AnimalType).Distinct().ToList());
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", pet.UserId);
            return View(pet);
        }
        [HttpGet]
        public IActionResult GetBreedsByAnimalType(string animalType)
        {
            // Query your database to get the list of breeds based on the selected AnimalType.
            var breeds = _context.AnimalTypes
                .Where(b => b.AnimalType == animalType)
                .Select(b => b.Breed)
                .ToList();

            return Json(breeds);
        }
        // GET: Pets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pet == null)
            {
                return NotFound();
            }

            var pet = await _context.Pet
                .Include(p => p.AnimalTypes)
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(m => m.PetId == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // POST: Pets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Pet'  is null.");
            }
            var pet = await _context.Pet.FindAsync(id);
            if (pet != null)
            {
                _context.Pet.Remove(pet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetExists(int id)
        {
          return (_context.Pet?.Any(e => e.PetId == id)).GetValueOrDefault();
        }
    }
}
