﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthcareSystemDesign.Models;
using Stripe;
using Rotativa.AspNetCore;

namespace HealthcareSystemDesign.Controllers
{
    public class BillingsController : Controller
    {
        private readonly healthcareContext _context;

        public BillingsController(healthcareContext context)
        {
            _context = context;
        }

        // GET: Billings
        public async Task<IActionResult> Index()
        {
            var healthcareContext = _context.Billing.Include(b => b.Patient);
            return View(await healthcareContext.ToListAsync());
        }

        //Search invoice without logging in 
        public IActionResult SearchInvoice( string billingid, double billingamount)
        {

            return View();
        }

        // GET: Billings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billing = await _context.Billing
                .Include(b => b.Patient)
                .FirstOrDefaultAsync(m => m.BillingId == id);
            if (billing == null)
            {
                return NotFound();
            }

            return new ViewAsPdf(billing);
        }

        //Card payment using stripe
        public async Task<IActionResult> Charge(int? id, string stripeEmail, string stripeToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billing = await _context.Billing
                .Include(b => b.Patient)
                .FirstOrDefaultAsync(m => m.BillingId == id);
            if (billing == null)
            {
                return NotFound();
            }

            //Create stripe charge
            var customers = new CustomerService();
            var charges = new ChargeService();
           
            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken,
               
                
            });
            

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = (long)billing.BillingAmount * 100,
                Description = "Invoice",
                Currency = "usd",
                Customer = customer.Id,
                ReceiptEmail = stripeEmail
                
                                
            });
            //


            if (charge.Status == "succeeded")
            {
                //Add payment record to card payment
                if (ModelState.IsValid) {

                 /// Use charge id as reference no  
                var newpayment = new CardPayment { PaymentDate = DateTime.Now, PaymentAmount = billing.BillingAmount, BillingId = (int)id, ReferenceNo = charge.Id };
                _context.Add(newpayment);
                await _context.SaveChangesAsync();

                 //Update the paid starus in the invoice
                 billing.Paid = true;
                 await _context.SaveChangesAsync();
                }

               

                return RedirectToAction("Index");
            }
            else { }
            return View("Index", "Billings");
        }
        // GET: Billings/Create
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "Address");
            return View();
        }

        // POST: Billings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BillingDate,BillingAmount,BillingId,Paid,PatientId")] Billing billing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(billing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "Address", billing.PatientId);
            return View(billing);
        }

        // GET: Billings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billing = await _context.Billing.FindAsync(id);
            if (billing == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "Address", billing.PatientId);
            return View(billing);
        }

        // POST: Billings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BillingDate,BillingAmount,BillingId,Paid,PatientId")] Billing billing)
        {
            if (id != billing.BillingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillingExists(billing.BillingId))
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
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "Address", billing.PatientId);
            return View(billing);
        }

        // GET: Billings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billing = await _context.Billing
                .Include(b => b.Patient)
                .FirstOrDefaultAsync(m => m.BillingId == id);
            if (billing == null)
            {
                return NotFound();
            }

            return View(billing);
        }

        // POST: Billings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billing = await _context.Billing.FindAsync(id);
            _context.Billing.Remove(billing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillingExists(int id)
        {
            return _context.Billing.Any(e => e.BillingId == id);
        }
    }
}
