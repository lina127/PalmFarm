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
    public class CowsController : Controller
    {
        private readonly TrackfarmContext _context;

        public CowsController(TrackfarmContext context)
        {
            _context = context;
        }

        // GET: Cows
        public async Task<IActionResult> Index()
        {
            List<Cow> cows = _context.Cow.Include(o => o.Sold).OrderByDescending(o=> o.BirthDate).ToList();
            return View(cows);
        }

        // GET: Cows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cow = await _context.Cow
                .Include(c => c.Farm).Include(o => o.Pregnancy)
                .FirstOrDefaultAsync(m => m.CowId == id);
            if (cow == null)
            {
                return NotFound();
            }

            var cowPregnancies = await _context.Pregnancy.Where(o => o.CowId == id ).ToListAsync();
            CowDTO cowDTO = new CowDTO(cow, cowPregnancies);

            return View(cowDTO);
        }

        // GET: Cows/Create
        public IActionResult Create()
        {
            ViewData["FarmId"] = new SelectList(_context.Farm, "FarmId", "Name");
            return View();
        }

        // POST: Cows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CowId,UId,Type,Sex,BirthDate,Status,MomUid,Comment,Kpn,NickName")] Cow cow)
        {
            if (ModelState.IsValid)
            {
                cow.FarmId = HttpContext.Session.GetInt32("farmId").Value;
                _context.Add(cow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FarmId"] = new SelectList(_context.Farm, "FarmId", "Name", cow.FarmId);
            return View(cow); 
        }

        // GET: Cows/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cow = await _context.Cow.FindAsync(id);
            if (cow == null)
            {
                return NotFound();
            }
            ViewData["FarmId"] = new SelectList(_context.Farm, "FarmId", "Name", cow.FarmId);
            return View(cow);
        }

        // POST: Cows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CowId,UId,Type,Sex,BirthDate,Status,MomUid,Comment,Kpn,NickName,FarmId")] Cow cow)
        {
            if (id != cow.CowId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CowExists(cow.CowId))
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
            ViewData["FarmId"] = new SelectList(_context.Farm, "FarmId", "Name", cow.FarmId);
            return View(cow);
        }

        // GET: Cows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pregnancy = await _context.Pregnancy.Where(o => o.CowId == id).ToListAsync();
            var sold = await _context.Sold.Where(o => o.CowId == id).ToListAsync();
            _context.Sold.RemoveRange(sold);
            _context.Pregnancy.RemoveRange(pregnancy);
            await _context.SaveChangesAsync();

            var cow = await _context.Cow
                .FirstOrDefaultAsync(m => m.CowId == id);
            if (cow == null)
            {
                return NotFound();
            }

            return View(cow);
        }

        // POST: Cows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var cow = await _context.Cow.FindAsync(id);
            _context.Cow.Remove(cow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CowExists(int id)
        {
            return _context.Cow.Any(e => e.CowId == id);
        }

        public void DeadCow(int Id)
        {
            Cow cow = _context.Cow.Where(o => o.CowId == Id).FirstOrDefault();
            cow.Status = "dead";
            _context.Cow.Update(cow);
            _context.SaveChanges();
        }

        public void SoldCow(int Id, int? price, string grade, string comment)
        {
            Cow cow = _context.Cow.Where(o => o.CowId == Id).FirstOrDefault();
            cow.Status = "sold";
            _context.Cow.Update(cow);
            _context.SaveChanges();

            Sold soldCow = new Sold();
            soldCow.CowId = Id;
            soldCow.Grade = grade;
            soldCow.Price = price;
            soldCow.Comment = comment;
            _context.Sold.Add(soldCow);
            _context.SaveChanges();
        }
    }
}
