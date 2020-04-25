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
    public class CashPaymentsController : Controller
    {
        private readonly healthcareContext _context;

        public CashPaymentsController(healthcareContext context)
        {
            _context = context;
        }

        // GET: CashPayments
        public async Task<IActionResult> Index()
        {
            var healthcareContext = _context.CashPayment.Include(c => c.Billing);
            return View(await healthcareContext.ToListAsync());
        }

        // GET: CashPayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashPayment = await _context.CashPayment
                .Include(c => c.Billing)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (cashPayment == null)
            {
                return NotFound();
            }

            return View(cashPayment);
        }

        // GET: CashPayments/Create
        public IActionResult Create(int ? id)
        {
            
            ViewData["PaymentAmount"] = new SelectList(_context.Billing, "BillingId", "BillingAmount", id);
            ViewData["BillingId"] = new SelectList(_context.Billing, "BillingId", "BillingId", id);
            return View();
        }

        // POST: CashPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentDate,PaymentId,PaymentAmount,BillingId")] CashPayment cashPayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cashPayment);
                await _context.SaveChangesAsync();

                //Update the paid status in the billing 
                var billing = await _context.Billing.FindAsync(cashPayment.BillingId);
                billing.Paid = true;
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Billings");
            }
            ViewData["BillingId"] = new SelectList(_context.Billing, "BillingId", "BillingId", cashPayment.BillingId);
            return View(cashPayment);
        }

        // GET: CashPayments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashPayment = await _context.CashPayment.FindAsync(id);
            if (cashPayment == null)
            {
                return NotFound();
            }
            ViewData["BillingId"] = new SelectList(_context.Billing, "BillingId", "BillingId", cashPayment.BillingId);
            return View(cashPayment);
        }

        // POST: CashPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentDate,PaymentId,PaymentAmount,BillingId")] CashPayment cashPayment)
        {
            if (id != cashPayment.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cashPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CashPaymentExists(cashPayment.PaymentId))
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
            ViewData["BillingId"] = new SelectList(_context.Billing, "BillingId", "BillingId", cashPayment.BillingId);
            return View(cashPayment);
        }

        // GET: CashPayments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashPayment = await _context.CashPayment
                .Include(c => c.Billing)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (cashPayment == null)
            {
                return NotFound();
            }

            return View(cashPayment);
        }

        // POST: CashPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cashPayment = await _context.CashPayment.FindAsync(id);
            _context.CashPayment.Remove(cashPayment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CashPaymentExists(int id)
        {
            return _context.CashPayment.Any(e => e.PaymentId == id);
        }
    }
}
