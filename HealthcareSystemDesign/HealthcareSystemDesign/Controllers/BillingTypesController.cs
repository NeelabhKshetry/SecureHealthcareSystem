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
    public class BillingTypesController : Controller
    {
        private readonly healthcareContext _context;

        public BillingTypesController(healthcareContext context)
        {
            _context = context;
        }

        // GET: BillingTypes
        public async Task<IActionResult> Index()
        {
            var healthcareContext = _context.BillingType.Include(b => b.Billing);
            return View(await healthcareContext.ToListAsync());
        }

        // GET: BillingTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billingType = await _context.BillingType
                .Include(b => b.Billing)
                .FirstOrDefaultAsync(m => m.BillingtypeId == id);
            if (billingType == null)
            {
                return NotFound();
            }

            return View(billingType);
        }

        // GET: BillingTypes/Create
        public IActionResult Create()
        {
            ViewData["BillingId"] = new SelectList(_context.Billing, "BillingId", "BillingId");
            return View();
        }

        // POST: BillingTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BillingtypeId,BillingName,BillingId")] BillingType billingType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(billingType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BillingId"] = new SelectList(_context.Billing, "BillingId", "BillingId", billingType.BillingId);
            return View(billingType);
        }

        // GET: BillingTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billingType = await _context.BillingType.FindAsync(id);
            if (billingType == null)
            {
                return NotFound();
            }
            ViewData["BillingId"] = new SelectList(_context.Billing, "BillingId", "BillingId", billingType.BillingId);
            return View(billingType);
        }

        // POST: BillingTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BillingtypeId,BillingName,BillingId")] BillingType billingType)
        {
            if (id != billingType.BillingtypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billingType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillingTypeExists(billingType.BillingtypeId))
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
            ViewData["BillingId"] = new SelectList(_context.Billing, "BillingId", "BillingId", billingType.BillingId);
            return View(billingType);
        }

        // GET: BillingTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billingType = await _context.BillingType
                .Include(b => b.Billing)
                .FirstOrDefaultAsync(m => m.BillingtypeId == id);
            if (billingType == null)
            {
                return NotFound();
            }

            return View(billingType);
        }

        // POST: BillingTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billingType = await _context.BillingType.FindAsync(id);
            _context.BillingType.Remove(billingType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillingTypeExists(int id)
        {
            return _context.BillingType.Any(e => e.BillingtypeId == id);
        }
    }
}
