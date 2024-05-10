using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GarageV32.Data;
using GarageV32.Models;
using static GarageV32.Models.ParkedVehicle;
using System.Linq;


namespace GarageV32.Controllers
{
    public class ParkedVehiclesController : Controller
    {
        private readonly GarageContext _context;
        private readonly ParkingService _parkingService;

        public ParkedVehiclesController(GarageContext context)
        {
            _context = context;
            _parkingService = new ParkingService(context);

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

            ViewBag.Colors = new SelectList(Enum.GetValues(typeof(VehicleColor)).Cast<VehicleColor>().Select(static v => new SelectListItem
            {
                Text = v.ToString(),
                Value = v.ToString()
            }), "Value", "Text");
            ViewData["GarageZoneId"] = new SelectList(_context.GarageZone, "Id", "ZoneName");
            ViewData["MemberId"] = new SelectList(_context.Member, "Id", "UserName");
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleType, "Id", "Name");

            return View();
        }

        // POST: ParkedVehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RegNr,Color,Year,Brand,Model,Wheels,Category,GarageZoneId,Spots,SpotNumber,SpotNumber2,SpotNumber3,MemberId,VehicleTypeId")] ParkedVehicle parkedVehicle)
        {
            if (_context.ParkedVehicle.Any(v => v.RegNr == parkedVehicle.RegNr))
            {
                ModelState.AddModelError("RegNr", "A Vehicle with Registration number already exists. Please check your registration number!.");
            }

            if (_context.ParkedVehicle.Any(v => v.SpotNumber == parkedVehicle.SpotNumber || v.SpotNumber == parkedVehicle.SpotNumber2 || v.SpotNumber == parkedVehicle.SpotNumber3 && v.GarageZoneId == parkedVehicle.GarageZoneId))
            {
                ModelState.AddModelError("SpotNumber", "This spot is occupied. Please choose another one or change the zone.");
                return View(parkedVehicle);
            }

            parkedVehicle.Category = _context.VehicleType.FirstOrDefault(v => v.Id == parkedVehicle.VehicleTypeId)?.Name ?? string.Empty;

            var garageZone = _context.GarageZone.Find(parkedVehicle.GarageZoneId);
            if (garageZone != null)
            {
                UpdateOccupiedSpots(garageZone, parkedVehicle);
                _context.Update(garageZone);
            }



            if (ModelState.IsValid)
            {
                _context.Add(parkedVehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Colors = new SelectList(Enum.GetValues(typeof(VehicleColor)).Cast<VehicleColor>().Select(static v => new SelectListItem
            {
                Text = v.ToString(),
                Value = v.ToString()
            }), "Value", "Text");
            ViewData["GarageZoneId"] = new SelectList(_context.GarageZone, "Id", "ZoneName", parkedVehicle.GarageZoneId);
                        ViewData["Spots"] = new SelectList(_context.GarageZone, "Id", "OccupiedSpotsList");

            ViewData["MemberId"] = new SelectList(_context.Member, "Id", "UserName", parkedVehicle.MemberId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleType, "Id", "Name", parkedVehicle.VehicleTypeId);

            return View(parkedVehicle);
        }

        static void UpdateOccupiedSpots(GarageZone zone, ParkedVehicle parkedVehicle)
        {
            switch (parkedVehicle.Category)
            {
                case "Truck":
                    AddOccupiedSpot(zone, parkedVehicle.SpotNumber);
                    AddOccupiedSpot(zone, (int)parkedVehicle.SpotNumber2);
                    AddOccupiedSpot(zone, (int)parkedVehicle.SpotNumber3);
                    break;
                case "Van":
                    AddOccupiedSpot(zone, parkedVehicle.SpotNumber);
                    AddOccupiedSpot(zone, (int)parkedVehicle.SpotNumber2);
                    break;
                default:
                    AddOccupiedSpot(zone, parkedVehicle.SpotNumber);
                    break;
            }
        }

        static void AddOccupiedSpot(GarageZone zone, int spotNumber)
        {
            if (!zone.OccupiedSpotsList.Contains(spotNumber))
            {
                zone.OccupiedSpotsList.Add(spotNumber);
            }
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

            ViewBag.Colors = new SelectList(Enum.GetValues(typeof(VehicleColor)).Cast<VehicleColor>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = v.ToString()
            }), "Value", "Text");
            ViewData["GarageZoneId"] = new SelectList(_context.GarageZone, "Id", "ZoneName", parkedVehicle.GarageZoneId);
            ViewData["MemberId"] = new SelectList(_context.Member, "Id", "UserName", parkedVehicle.MemberId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleType, "Id", "Name", parkedVehicle.VehicleTypeId);
            return View(parkedVehicle);
        }

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegNr,Color,Year,Brand,Model,Wheels,Category,GarageZoneId,SpotNumber, SpotNumber2,SpotNumber3,MemberId,VehicleTypeId")] ParkedVehicle parkedVehicle)
        {
            if (id != parkedVehicle.Id)
            {
                return NotFound();
            }
            parkedVehicle.Category = _context.VehicleType.FirstOrDefault(v => v.Id == parkedVehicle.VehicleTypeId)?.Name ?? string.Empty;

            parkedVehicle.Category = _context.VehicleType.FirstOrDefault(v => v.Id == parkedVehicle.VehicleTypeId)?.Name ?? string.Empty;


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
            ViewData["GarageZoneId"] = new SelectList(_context.GarageZone, "Id", "ZoneName", parkedVehicle.GarageZoneId);
            ViewData["MemberId"] = new SelectList(_context.Member, "Id", "Id", parkedVehicle.MemberId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleType, "Id", "Id", parkedVehicle.VehicleTypeId);
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
        public async Task<IActionResult> CheckOut(int? id)
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

            var invoice = new InvoiceViewModel
            {
                RegNr = parkedVehicle.RegNr,
                MemberId = parkedVehicle.MemberId ?? 0,
                EntryTime = parkedVehicle.EnteryTime,
                ExitTime = DateTime.Now,
                TotalMinutes = _parkingService.CalculateTotalMinutes(parkedVehicle),
                Fee = _parkingService.CalculateParkingFee(parkedVehicle)
            };

            parkedVehicle.ExitTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return View("Invoice", invoice);
        }
    }
}
