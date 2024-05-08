using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GarageV32.Data;
using GarageV32.Models;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GarageV32.Controllers
{
    public class MembersController : Controller
    {
        private readonly GarageContext _context;

        public MembersController(GarageContext context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            return View(await _context.Member.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .Include(m => m.ParkedVehicles)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,PersonNumber,UserName,MemberSince")] Member member)
        {

            bool nameIsValid = CheckName(member.FirstName, member.LastName);
            int age = CheckAge(member.PersonNumber);
            if (age >= 18 && nameIsValid) {
                member.UserName = MakeMemberId(member.FirstName, member.LastName, member.PersonNumber);
            }

            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }


        public string MakeMemberId(string fName, string lName, string pNumber)
        {
            if (string.IsNullOrEmpty(fName) || string.IsNullOrEmpty(lName) || string.IsNullOrEmpty(pNumber))
            {
                ModelState.AddModelError("LastName", "First Name, Last Name and Person Number is empty");
            }

            string MemberId = fName[..1] + lName[..1] + pNumber.Substring(9, 4);

            return MemberId;
        }

        public int CheckAge(string pNumber)
        {
            string y = pNumber.Substring(0, 4);
            string m = pNumber.Substring(4, 2);
            string d = pNumber.Substring(6, 2);
            string x = y + "-" + m + "-" + d;
            DateTime birthDate = DateTime.ParseExact(x, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            TimeSpan age = DateTime.Now - birthDate;
            int memberAge = age.Days / 365;
            if (memberAge < 18) {
                ModelState.AddModelError("PersonNumber", "You should be at least 18 years old to register!");
            }
            return memberAge;
        }
        public bool CheckName(string fName, string lName)
        {
            if (fName.ToLower() == lName.ToLower())
            {
                ModelState.AddModelError("LastName", "First Name and Last Name can not be same!");
            }
            return true;
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,PersonNumber,UserName,MemberSince")] Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
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
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Member.FindAsync(id);
            if (member != null)
            {
                _context.Member.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Member.Any(e => e.Id == id);
        }
    }
}
