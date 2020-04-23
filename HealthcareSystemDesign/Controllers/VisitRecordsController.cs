using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthcareSystemDesign.Models;

namespace HealthcareSystemDesign.Controllers
{
    public class VisitRecordsController : Controller
    {
        private readonly healthcareContext _context;

        public VisitRecordsController(healthcareContext context)
        {
            _context = context;
        }

        // GET: VisitRecords
        public async Task<IActionResult> Index()
        {
            var healthcareContext = _context.VisitRecord.Include(v => v.Doctor).Include(v => v.Patient);
            return View(await healthcareContext.ToListAsync());
        }

        // GET: VisitRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitRecord = await _context.VisitRecord
                .Include(v => v.Doctor)
                .Include(v => v.Patient)
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (visitRecord == null)
            {
                return NotFound();
            }

            return View(visitRecord);
        }

        // GET: VisitRecords/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "DoctorId", "DoctorEmail");
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "Address");
            return View();
        }

        // POST: VisitRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VisitReason,Prescription,VisitDate,Visited,PatientId,DoctorId")] VisitRecord visitRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(visitRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "DoctorId", "DoctorEmail", visitRecord.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "Address", visitRecord.PatientId);
            return View(visitRecord);
        }

        // GET: VisitRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitRecord = await _context.VisitRecord.FindAsync(id);
            if (visitRecord == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "DoctorId", "DoctorEmail", visitRecord.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "Address", visitRecord.PatientId);
            return View(visitRecord);
        }

        // POST: VisitRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VisitReason,Prescription,VisitDate,Visited,PatientId,DoctorId")] VisitRecord visitRecord)
        {
            if (id != visitRecord.PatientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visitRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitRecordExists(visitRecord.PatientId))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "DoctorId", "DoctorEmail", visitRecord.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "Address", visitRecord.PatientId);
            return View(visitRecord);
        }

        // GET: VisitRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitRecord = await _context.VisitRecord
                .Include(v => v.Doctor)
                .Include(v => v.Patient)
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (visitRecord == null)
            {
                return NotFound();
            }

            return View(visitRecord);
        }

        // POST: VisitRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visitRecord = await _context.VisitRecord.FindAsync(id);
            _context.VisitRecord.Remove(visitRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisitRecordExists(int id)
        {
            return _context.VisitRecord.Any(e => e.PatientId == id);
        }
    }
}
