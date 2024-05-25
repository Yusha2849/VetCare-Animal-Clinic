using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AnimalTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnimalTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AnimalTypes
        public async Task<IActionResult> Index()
        {
              return _context.AnimalTypes != null ? 
                          View(await _context.AnimalTypes.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.AnimalTypes'  is null.");
        }

        // GET: AnimalTypes/Details
        public async Task<IActionResult> Details(string animaltype, string breed)
        {
            if (animaltype == null || breed == null)
            {
                return NotFound();
            }

            var animalTypes = await _context.AnimalTypes
                .FirstOrDefaultAsync(m => m.AnimalType == animaltype && m.Breed == breed);

            if (animalTypes == null)
            {
                return NotFound();
            }

            return View(animalTypes);
        }


        // GET: AnimalTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnimalTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AnimalType,Breed,Species")] AnimalTypes animalTypes)
        {
            if (ModelState.IsValid)
            {
                // Check if the combination of AnimalType and Breed already exists
                var existingAnimalType = await _context.AnimalTypes
                    .FirstOrDefaultAsync(e => e.AnimalType == animalTypes.AnimalType && e.Breed == animalTypes.Breed);

                if (existingAnimalType != null)
                {
                    ModelState.AddModelError(string.Empty, "This combination of Animal Type and Breed already exists.");
                    return View(animalTypes);
                }

                // The combination does not exist, proceed with adding it
                _context.Add(animalTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(animalTypes);
        }


        // GET: AnimalTypes/Edit/5
        public async Task<IActionResult> Edit(string animaltype, string breed)
        {
            if (animaltype == null || breed == null)
            {
                return NotFound();
            }

            var animalTypes = await _context.AnimalTypes.FindAsync(animaltype, breed);
            if (animalTypes == null)
            {
                return NotFound();
            }

            return View(animalTypes);
        }


        // POST: AnimalTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string animaltype, string breed, [Bind("AnimalType,Breed,Species")] AnimalTypes animalTypes)
        {
           if (animaltype != animalTypes.AnimalType || breed != animalTypes.Breed)
           {
                return NotFound();
           }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingAnimalType = await _context.AnimalTypes.FindAsync(animaltype, breed);
                    if (existingAnimalType == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the existing entity
                    existingAnimalType.Species = animalTypes.Species;
                    _context.Entry(existingAnimalType).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalTypesExists(animalTypes.AnimalType, animalTypes.Breed))
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
            return View(animalTypes);
        }


        // GET: AnimalTypes/Delete/5
        public async Task<IActionResult> Delete(string animaltype, string breed)
        {
            if (animaltype == null || breed == null)
            {
                return NotFound();
            }

            var animalTypes = await _context.AnimalTypes
                .FirstOrDefaultAsync(m => m.AnimalType == animaltype && m.Breed == breed);

            if (animalTypes == null)
            {
                return NotFound();
            }

            return View(animalTypes);
        }

        // POST: AnimalTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string animaltype, string breed)
        {
            if (animaltype == null || breed == null)
            {
                return NotFound();
            }

            var animalTypes = await _context.AnimalTypes.FindAsync(animaltype, breed);

            if (animalTypes != null)
            {
                _context.AnimalTypes.Remove(animalTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }


        private bool AnimalTypesExists(string animaltype, string breed)
        {
            return _context.AnimalTypes.Any(e => e.AnimalType == animaltype && e.Breed == breed);
        }

    }
}
