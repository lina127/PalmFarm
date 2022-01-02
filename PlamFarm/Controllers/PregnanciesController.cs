#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlamFarm.Data;
using PlamFarm.Models;

namespace PlamFarm.Controllers
{
    public class PregnanciesController : Controller
    {
        private readonly TrackfarmContext _context;

        public PregnanciesController(TrackfarmContext context)
        {
            _context = context;
        }

        // GET: Pregnancies
        public IActionResult Index()
        {
            int farmId = HttpContext.Session.GetInt32("farmId").Value;
            List<Pregnancy> pregnancies = _context.Pregnancy.Where(o => o.Cow.FarmId == farmId && o.PregnancyStatus == "T" && o.Cow.Status != "dead" && o.Cow.Status !="sold").OrderBy(o => o.PregnantDate).Include(o => o.Cow).ToList();
            return View(pregnancies);
        }

        // GET: Pregnancies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pregnancy = await _context.Pregnancy
                .Include(p => p.Cow)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pregnancy == null)
            {
                return NotFound();
            }

            return View(pregnancy);
        }

        // GET: Pregnancies/Create
        public IActionResult Create(int id)
        {
            ViewData["cowId"] = id;
            return View();
        }

        // POST: Pregnancies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CowId, PregnantDate,Kpn")] Pregnancy pregnancy)
        {
            _context.Add(pregnancy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Pregnancies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pregnancy = await _context.Pregnancy.FindAsync(id);
            if (pregnancy == null)
            {
                return NotFound();
            }
            ViewData["CowId"] = new SelectList(_context.Cow, "CowId", "Sex", pregnancy.CowId, "PregnancyStatus");
            return View(pregnancy);
        }

        // POST: Pregnancies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CowId,PregnantDate,Kpn, PregnancyStatus")] Pregnancy pregnancy)
        {
            if (id != pregnancy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pregnancy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PregnancyExists(pregnancy.Id))
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
            ViewData["CowId"] = new SelectList(_context.Cow, "CowId", "Sex", pregnancy.CowId);
            return View(pregnancy);
        }

        // GET: Pregnancies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pregnancy = await _context.Pregnancy
                .Include(p => p.Cow)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pregnancy == null)
            {
                return NotFound();
            }

            return View(pregnancy);
        }

        // POST: Pregnancies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pregnancy = await _context.Pregnancy.FindAsync(id);
            _context.Pregnancy.Remove(pregnancy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PregnancyExists(int id)
        {
            return _context.Pregnancy.Any(e => e.Id == id);
        }

        public void GiveBirth(int Id)
        {
            Pregnancy pregnancy = _context.Pregnancy.Where(o => o.Id == Id).FirstOrDefault();
            pregnancy.BirthDate = DateTime.Now;
            pregnancy.PregnancyStatus = "F";
            _context.Pregnancy.Update(pregnancy);
            _context.SaveChanges();
        }

        public void AddBirthDate(int Id, string BirthDate)
        {
            DateTime dateTime = DateTime.Parse(BirthDate);
            Pregnancy pregnancy = _context.Pregnancy.Where(o => o.Id == Id).FirstOrDefault();
            pregnancy.BirthDate = dateTime;
            pregnancy.PregnancyStatus = "F";
            _context.Pregnancy.Update(pregnancy);
            _context.SaveChanges();
        }
    }
}
