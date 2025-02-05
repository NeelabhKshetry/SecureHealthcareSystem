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
    public class MonthlyReportsController : Controller
    {
        private readonly healthcareContext _context;

        public MonthlyReportsController(healthcareContext context)
        {
            _context = context;
        }

        // GET: MonthlyReports
        public async Task<IActionResult> Index()
        {
            return View(await _context.MonthlyReport.ToListAsync());
        }

        // GET: MonthlyReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monthlyReport = await _context.MonthlyReport
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (monthlyReport == null)
            {
                return NotFound();
            }

            return View(monthlyReport);
        }

        // GET: MonthlyReports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MonthlyReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportId,ReportMonth,NoPatients,DoctorName,MonthlyIncome")] MonthlyReport monthlyReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(monthlyReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(monthlyReport);
        }

        // GET: MonthlyReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monthlyReport = await _context.MonthlyReport.FindAsync(id);
            if (monthlyReport == null)
            {
                return NotFound();
            }
            return View(monthlyReport);
        }

        // POST: MonthlyReports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportId,ReportMonth,NoPatients,DoctorName,MonthlyIncome")] MonthlyReport monthlyReport)
        {
            if (id != monthlyReport.ReportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monthlyReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonthlyReportExists(monthlyReport.ReportId))
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
            return View(monthlyReport);
        }

        // GET: MonthlyReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monthlyReport = await _context.MonthlyReport
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (monthlyReport == null)
            {
                return NotFound();
            }

            return View(monthlyReport);
        }

        // POST: MonthlyReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monthlyReport = await _context.MonthlyReport.FindAsync(id);
            _context.MonthlyReport.Remove(monthlyReport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonthlyReportExists(int id)
        {
            return _context.MonthlyReport.Any(e => e.ReportId == id);
        }
    }
}
