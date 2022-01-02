using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlamFarm.Data;
using PlamFarm.Models;
using System.Diagnostics;

namespace PlamFarm.Controllers
{
    public class HomeController : BaseController
    {
        private readonly TrackfarmContext _context;

        public HomeController(TrackfarmContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if(!IsUserSessionValid())
            {
                return RedirectToAction("Login", "Farms");
            }
            int farmId = HttpContext.Session.GetInt32("farmId").Value;
            List<Cow> cows = _context.Cow.Where(o => o.FarmId == farmId && o.Status.ToLower() != "dead" && o.Status.ToLower() != "sold").ToList();
            List<Pregnancy> pregnancies = _context.Pregnancy.Where(o => o.PregnancyStatus == "T" && o.Cow.FarmId == farmId && o.Cow.Status.ToLower() != "dead" && o.Cow.Status.ToLower() != "sold").OrderBy(o => o.PregnantDate).Include(o => o.Cow).ToList();
            CowDTO cowDTO = new CowDTO(null, cows, pregnancies);
            return View(cowDTO);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult FarmList()
        {
            List<Farm> farms = _context.Farm.ToList();
            return View(farms);
        }

        public IActionResult AddFarm()
        {
            return View();
        }

        public void AddNewFarm(Farm farm) { 
            _context.Farm.Add(farm);
            _context.SaveChanges();
        }
    }
}