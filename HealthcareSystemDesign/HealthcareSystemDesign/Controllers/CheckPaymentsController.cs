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
    public class CheckPaymentsController : Controller
    {
        private readonly healthcareContext _context;

        public CheckPaymentsController(healthcareContext context)
        {
            _context = context;
        }

        // GET: CheckPayments
        public async Task<IActionResult> Index()
        {
            var healthcareContext = _context.CheckPayment.Include(c => c.Billing);
            return View(await healthcareContext.ToListAsync());
        }

        // GET: CheckPayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkPayment = await _context.CheckPayment
                .Include(c => c.Billing)
                .FirstOrDefaultAsync(m => m.CheckNo == id);
            if (checkPayment == null)
            {
                return NotFound();
            }

            return View(checkPayment);
        }

        // GET: CheckPayments/Create
        public IActionResult Create(int? id)
        {

            ViewData["PaymentAmount"] = new SelectList(_context.Billing, "BillingId", "BillingAmount", id);
            ViewData["BillingId"] = new SelectList(_context.Billing, "BillingId", "BillingId", id);
            return View();
        }

        // POST: CheckPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CheckNo,PaymentAmount,PaymentDate,BillingId")] CheckPayment checkPayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(checkPayment);
                await _context.SaveChangesAsync();

                //Update the paid status in the billing 
                var billing = await _context.Billing.FindAsync(checkPayment.BillingId);
                billing.Paid = true;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BillingId"] = new SelectList(_context.Billing, "BillingId", "BillingId", checkPayment.BillingId);
            return View(checkPayment);
        }

        // GET: CheckPayments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkPayment = await _context.CheckPayment.FindAsync(id);
            if (checkPayment == null)
            {
                return NotFound();
            }
            ViewData["BillingId"] = new SelectList(_context.Billing, "BillingId", "BillingId", checkPayment.BillingId);
            return View(checkPayment);
        }

        // POST: CheckPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CheckNo,PaymentAmount,PaymentDate,BillingId")] CheckPayment checkPayment)
        {
            if (id != checkPayment.CheckNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(checkPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckPaymentExists(checkPayment.CheckNo))
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
            ViewData["BillingId"] = new SelectList(_context.Billing, "BillingId", "BillingId", checkPayment.BillingId);
            return View(checkPayment);
        }

        // GET: CheckPayments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkPayment = await _context.CheckPayment
                .Include(c => c.Billing)
                .FirstOrDefaultAsync(m => m.CheckNo == id);
            if (checkPayment == null)
            {
                return NotFound();
            }

            return View(checkPayment);
        }

        // POST: CheckPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var checkPayment = await _context.CheckPayment.FindAsync(id);
            _context.CheckPayment.Remove(checkPayment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckPaymentExists(int id)
        {
            return _context.CheckPayment.Any(e => e.CheckNo == id);
        }
    }
}
