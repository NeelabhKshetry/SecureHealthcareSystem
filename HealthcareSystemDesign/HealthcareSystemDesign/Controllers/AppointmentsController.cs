﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthcareSystemDesign.Models;
using Microsoft.AspNetCore.Authorization;

namespace HealthcareSystemDesign.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly healthcareContext _context;

        public AppointmentsController(healthcareContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Patient"))
            {
                var healthcareContext = _context.Appointment.Include(a => a.Doctor).Include(a => a.Patient).Where(c => c.PatientEmail == User.Identity.Name);

                return View(await healthcareContext.ToListAsync());
            }
            else if (User.IsInRole("Staff"))
            {
                Console.WriteLine(" User is in Role");
                var healthcareContext = _context.Appointment.Include(a => a.Doctor).Include(a => a.Patient);
                return View(await healthcareContext.ToListAsync());
            }
            else
                return View("NotAllowed");
        }

    




    // GET: Appointments/Details/5
    public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.ApptId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "DoctorId", "DoctorEmail");
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "Address");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApptId,ApptDate,AppointmentReason,PatientEmail,PatientId,VisitRecord,DoctorId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();

                if (appointment.VisitRecord)
                {
                    var record = new VisitRecord { VisitDate = DateTime.Now, VisitReason=appointment.AppointmentReason, Prescription="N/A", PatientId= (int) appointment.PatientId, DoctorId= appointment.DoctorId, Visited=false};
                    _context.Add(record);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "DoctorId", "DoctorEmail", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "Address", appointment.PatientId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "DoctorId", "DoctorEmail", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "Address", appointment.PatientId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApptId,ApptDate,AppointmentReason,PatientEmail,PatientId,VisitRecord,DoctorId")] Appointment appointment)
        {
            if (id != appointment.ApptId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (appointment.VisitRecord)
                    {
                        var record = new VisitRecord { VisitDate = DateTime.Now, VisitReason = appointment.AppointmentReason, Prescription = "N/A", PatientId = (int)appointment.PatientId, DoctorId = appointment.DoctorId, Visited = true };
                        _context.Add(record);
                        await _context.SaveChangesAsync();
                     
                    }
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.ApptId))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "DoctorId", "DoctorEmail", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "Address", appointment.PatientId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.ApptId == id);
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
            var appointment = await _context.Appointment.FindAsync(id);
            _context.Appointment.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointment.Any(e => e.ApptId == id);
        }
    }
}
