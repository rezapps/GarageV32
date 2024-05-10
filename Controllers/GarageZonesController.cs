using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GarageV32.Data;
using GarageV32.Models;

namespace GarageV32.Controllers
{
    public class GarageZonesController : Controller
    {
        private readonly GarageContext _context;

        public GarageZonesController(GarageContext context)
        {
            _context = context;
        }

        // GET: GarageZones
        public async Task<IActionResult> Index()
        {
            return View(await _context.GarageZone.ToListAsync());
        }

        // GET: GarageZones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var garageZone = await _context.GarageZone
                .FirstOrDefaultAsync(m => m.Id == id);
            if (garageZone == null)
            {
                return NotFound();
            }

            return View(garageZone);
        }

        // GET: GarageZones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GarageZones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Capacity,ZoneName,OccupiedSpotsList")] GarageZone garageZone)
        {
            if (ModelState.IsValid)
            {
                _context.Add(garageZone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(garageZone);
        }

        // GET: GarageZones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var garageZone = await _context.GarageZone.FindAsync(id);
            if (garageZone == null)
            {
                return NotFound();
            }
            return View(garageZone);
        }

        // POST: GarageZones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Capacity,ZoneName,OccupiedSpotsList")] GarageZone garageZone)
        {
            if (id != garageZone.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(garageZone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GarageZoneExists(garageZone.Id))
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
            return View(garageZone);
        }

        // GET: GarageZones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var garageZone = await _context.GarageZone
                .FirstOrDefaultAsync(m => m.Id == id);
            if (garageZone == null)
            {
                return NotFound();
            }

            return View(garageZone);
        }

        // POST: GarageZones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var garageZone = await _context.GarageZone.FindAsync(id);
            if (garageZone != null)
            {
                _context.GarageZone.Remove(garageZone);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GarageZoneExists(int id)
        {
            return _context.GarageZone.Any(e => e.Id == id);
        }
    }
}
