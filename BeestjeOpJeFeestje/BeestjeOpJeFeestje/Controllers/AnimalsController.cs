﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;

namespace BeestjeOpJeFeestje.Controllers {
    [Authorize(Policy = "RequireAdminClaim")]
    [Authorize(Policy = "RequireAdminRole")]
    public class AnimalsController : Controller {
        private readonly ApplicationDbContext _context;

        public AnimalsController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: Animals
        public async Task<IActionResult> Index() {
            var applicationDbContext = _context.Animals.Include(a => a.AnimalType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Animals/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var animal = await _context.Animals
                .Include(a => a.AnimalType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null) {
                return NotFound();
            }

            return View(animal);
        }

        // GET: Animals/Create
        public IActionResult Create() {
            ViewData["AnimalTypeId"] = new SelectList(_context.AnimalTypes, "Id", "TypeName");
            return View();
        }

        // POST: Animals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,ImagePath,AnimalTypeId")] CreateAnimalViewModel animalViewModel) {
            if (ModelState.IsValid) {
                var animal = new Animal {
                    Id = animalViewModel.Id,
                    Name = animalViewModel.Name,
                    Price = animalViewModel.Price,
                    ImagePath = animalViewModel.ImagePath,
                    AnimalTypeId = animalViewModel.AnimalTypeId
                };
                _context.Add(animal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnimalTypeId"] = new SelectList(_context.AnimalTypes, "Id", "TypeName", animalViewModel.AnimalTypeId);
            return View(animalViewModel);
        }

        // GET: Animals/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var animal = await _context.Animals.FindAsync(id);
            if (animal == null) {
                return NotFound();
            }
            ViewData["AnimalTypeId"] = new SelectList(_context.AnimalTypes, "Id", "TypeName", animal.AnimalTypeId);
            return View(animal);
        }

        // POST: Animals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,ImagePath,AnimalTypeId")] CreateAnimalViewModel animalViewModel) {
            if (id != animalViewModel.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                var animal = new Animal {
                    Id = animalViewModel.Id,
                    Name = animalViewModel.Name,
                    Price = animalViewModel.Price,
                    ImagePath = animalViewModel.ImagePath,
                    AnimalTypeId = animalViewModel.AnimalTypeId
                };

                try {
                    _context.Update(animal);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!AnimalExists(animal.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnimalTypeId"] = new SelectList(_context.AnimalTypes, "Id", "TypeName", animalViewModel.AnimalTypeId);
            return View(animalViewModel);
        }


        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var animal = await _context.Animals
                .Include(a => a.AnimalType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null) {
                return NotFound();
            }

            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var animal = await _context.Animals.FindAsync(id);
            if (animal != null) {
                _context.Animals.Remove(animal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimalExists(int id) {
            return _context.Animals.Any(e => e.Id == id);
        }
    }
}
