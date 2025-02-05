﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthcareSystemDesign.Models;

namespace HealthcareSystemDesign.Controllers
{
    public class DoctorUnavailabilitiesController : Controller
    {
        private readonly healthcareContext _context;

        public DoctorUnavailabilitiesController(healthcareContext context)
        {
            _context = context;
        }

        // GET: DoctorUnavailabilities
        public async Task<IActionResult> Index()
        {
            var healthcareContext = _context.DoctorUnavailability.Include(d => d.Doctor);
            return View(await healthcareContext.ToListAsync());
        }

        // GET: DoctorUnavailabilities/Details/5
        public async Task<IActionResult> Details(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorUnavailability = await _context.DoctorUnavailability
                .Include(d => d.Doctor)
                .FirstOrDefaultAsync(m => m.Unavailability == id);
            if (doctorUnavailability == null)
            {
                return NotFound();
            }

            return View(doctorUnavailability);
        }

        // GET: DoctorUnavailabilities/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "DoctorId", "DoctorEmail");
            return View();
        }

        // POST: DoctorUnavailabilities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Unavailability,DoctorId")] DoctorUnavailability doctorUnavailability)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctorUnavailability);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "DoctorId", "DoctorEmail", doctorUnavailability.DoctorId);
            return View(doctorUnavailability);
        }

        // GET: DoctorUnavailabilities/Edit/5
        public async Task<IActionResult> Edit(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorUnavailability = await _context.DoctorUnavailability.FindAsync(id);
            if (doctorUnavailability == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "DoctorId", "DoctorEmail", doctorUnavailability.DoctorId);
            return View(doctorUnavailability);
        }

        // POST: DoctorUnavailabilities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DateTime id, [Bind("Unavailability,DoctorId")] DoctorUnavailability doctorUnavailability)
        {
            if (id != doctorUnavailability.Unavailability)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctorUnavailability);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorUnavailabilityExists(doctorUnavailability.Unavailability))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "DoctorId", "DoctorEmail", doctorUnavailability.DoctorId);
            return View(doctorUnavailability);
        }

        // GET: DoctorUnavailabilities/Delete/5
        public async Task<IActionResult> Delete(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorUnavailability = await _context.DoctorUnavailability
                .Include(d => d.Doctor)
                .FirstOrDefaultAsync(m => m.Unavailability == id);
            if (doctorUnavailability == null)
            {
                return NotFound();
            }

            return View(doctorUnavailability);
        }

        // POST: DoctorUnavailabilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DateTime id)
        {
            var doctorUnavailability = await _context.DoctorUnavailability.FindAsync(id);
            _context.DoctorUnavailability.Remove(doctorUnavailability);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorUnavailabilityExists(DateTime id)
        {
            return _context.DoctorUnavailability.Any(e => e.Unavailability == id);
        }
    }
}
