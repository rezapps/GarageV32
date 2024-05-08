using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GarageV32.Data;
using GarageV32.Models;
using static GarageV32.Models.VTypeViewModel;
using static GarageV32.Models.ParkedVehicle;

namespace GarageV32.Controllers
{
    public class ParkedVehiclesController : Controller
    {
        private readonly GarageContext _context;

        public ParkedVehiclesController(GarageContext context)
        {
            _context = context;
        }

        // GET: ParkedVehicles
        public async Task<IActionResult> Index(string VehicleCategory, string searchString)
        {
            var garageContext = _context.ParkedVehicle
                .Include(p => p.GarageZone)
                .Include(p => p.Member)
                .Include(p => p.VehicleType);
            if (_context.ParkedVehicle == null)
            {
                return Problem("Entity set 'GarageContext.ParkedVehicle'  is null.");
            }

            IQueryable<string> categoryQuery = from p in _context.ParkedVehicle
                                               orderby p.Category
                                               select p.Category;

            var parkedVehicles = from p in _context.ParkedVehicle
                                 select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                parkedVehicles = parkedVehicles.Where(s => s.RegNr.ToUpper().Contains(searchString.ToUpper()));
            }
            if (!String.IsNullOrEmpty(VehicleCategory))
            {
                parkedVehicles = parkedVehicles.Where(x => x.Category == VehicleCategory);
            }

            var parkedVehiclesVM = new VTypeViewModel
            {
                Categories = new SelectList(await categoryQuery.Distinct().ToListAsync()),
                ParkedVehicles = await parkedVehicles.ToListAsync(),
            };

            return View(parkedVehiclesVM);
        }

        // GET: ParkedVehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle
                .Include(p => p.GarageZone)
                .Include(p => p.Member)
                .Include(p => p.VehicleType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }

            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Create
        public IActionResult Create()
        {
            ViewBag.Colors = new SelectList(Enum.GetValues(typeof(VehicleColor)).Cast<VehicleColor>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = v.ToString()
            }), "Value", "Text");

            ViewData["GarageZoneId"] = new SelectList(_context.Set<GarageZone>(), "Id", "Id");
            ViewData["MemberId"] = new SelectList(_context.Set<Member>(), "UserName", "UserName");
            ViewData["VehicleTypeId"] = new SelectList(_context.Set<VehicleType>(), "Name", "Name");
            return View();
        }

        // POST: ParkedVehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RegNr,Color,Year,Brand,Model,Wheels,Category,EnteryTime,ExitTime,GarageZoneId,SpotNumber,MemberId")] ParkedVehicle parkedVehicle)
        {

            if (_context.ParkedVehicle.Any(v => v.RegNr == parkedVehicle.RegNr))
            {
                ModelState.AddModelError("RegNr", "Registration number must be unique");
            }

            if (_context.ParkedVehicle.Any(v => v.SpotNumber == parkedVehicle.SpotNumber))
            {
                ModelState.AddModelError("ParkingSpotNumber", "Spot number is occupied. Please choose another one.");
                return View(parkedVehicle);
            }

            if (ModelState.IsValid)
            {
                _context.Add(parkedVehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            ViewBag.Colors = new SelectList(Enum.GetValues(typeof(VehicleColor)).Cast<VehicleColor>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = v.ToString()
            }), "Value", "Text");


            ViewData["GarageZoneId"] = new SelectList(_context.Set<GarageZone>(), "Id", "Id", parkedVehicle.GarageZoneId);
            ViewData["MemberId"] = new SelectList(_context.Set<Member>(), "UserName", "UserName", parkedVehicle.MemberId);
            ViewData["VehicleTypeId"] = new SelectList(_context.Set<VehicleType>(), "Name", "Name", parkedVehicle.VehicleTypeId);
            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle.FindAsync(id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }
            ViewData["GarageZoneId"] = new SelectList(_context.Set<GarageZone>(), "Id", "Id", parkedVehicle.GarageZoneId);
            ViewData["MemberId"] = new SelectList(_context.Set<Member>(), "UserName", "UserName", parkedVehicle.MemberId);
            ViewData["VehicleTypeId"] = new SelectList(_context.Set<VehicleType>(), "Name", "Name", parkedVehicle.VehicleTypeId);
            return View(parkedVehicle);
        }

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegNr,Color,Year,Brand,Model,Wheels,Category,EnteryTime,ExitTime,GarageZoneId,SpotNumber,MemberId,VehicleTypeId")] ParkedVehicle parkedVehicle)
        {
            if (id != parkedVehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkedVehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkedVehicleExists(parkedVehicle.Id))
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
            ViewBag.Colors = new SelectList(Enum.GetValues(typeof(VehicleColor)).Cast<VehicleColor>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = v.ToString()
            }), "Value", "Text");

            ViewData["GarageZoneId"] = new SelectList(_context.Set<GarageZone>(), "Id", "Id", parkedVehicle.GarageZoneId);
            ViewData["MemberId"] = new SelectList(_context.Set<Member>(), "UserName", "UserName", parkedVehicle.MemberId);
            ViewData["VehicleTypeId"] = new SelectList(_context.Set<VehicleType>(), "Name", "Name", parkedVehicle.VehicleTypeId);
            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle
                .Include(p => p.GarageZone)
                .Include(p => p.Member)
                .Include(p => p.VehicleType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }

            return View(parkedVehicle);
        }

        // POST: ParkedVehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkedVehicle = await _context.ParkedVehicle.FindAsync(id);
            if (parkedVehicle != null)
            {
                _context.ParkedVehicle.Remove(parkedVehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkedVehicleExists(int id)
        {
            return _context.ParkedVehicle.Any(e => e.Id == id);
        }
    }
}
