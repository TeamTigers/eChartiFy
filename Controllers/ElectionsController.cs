using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using electionDbAnalytics.Data;
using electionDbAnalytics.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;

namespace electionDbAnalytics.Controllers
{
    public class ElectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ElectionsController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        public async Task<IActionResult> Index()
        {
            // Return a message "Index0", To load _LoadChartIndex0 in /Elections
            ViewBag.message = "Index0";
            return View(await _context.Elections.ToListAsync());
        }

        // GET: Elections/2008
        [HttpGet("Elections/{yearOrDistrict}")]
        public async Task<IActionResult> Index(string yearOrDistrict)
        {

            if (yearOrDistrict == null)
            {
                return NotFound();
            }

            // If yearOrDistrict is a int, assume this is a year; else a district
            if (Regex.IsMatch(yearOrDistrict, @"^\d+$"))
            {
                ViewBag.message = "Index1";
                ViewBag.electionYear = yearOrDistrict;
                return View(await _context.Elections.Where(m => m.Year == yearOrDistrict).ToListAsync());
            }
            else
            {
                ViewBag.message = "Index2";
                ViewBag.districtValue = yearOrDistrict;
                return View(await _context.Elections.Where(m => m.District == yearOrDistrict).ToListAsync());
            }
        }

        // GET: Elections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var election = await _context.Elections
                .FirstOrDefaultAsync(m => m.ID == id);
            if (election == null)
            {
                return NotFound();
            }

            return View(election);
        }

        // GET: Elections/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            return View(await _context.Elections.OrderBy(s => s.Constituency).ToListAsync());
        }

        // POST: Elections/Post
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Post(IFormFile files)
        {
            HashSet<Election> electionSet = new HashSet<Election>();

            using (var memoryStream = new MemoryStream())
            {
                await files.CopyToAsync(memoryStream).ConfigureAwait(false);
                using (var package = new ExcelPackage(memoryStream))
                {
                    var workSheet = package.Workbook.Worksheets.First();
                    var rowCount = workSheet.Dimension?.Rows;
                    var colCount = workSheet.Dimension?.Columns;


                    for (int row = 2; row <= rowCount.Value; row++)
                    {
                        electionSet.Add(new Election
                        {
                            Year = workSheet.Cells[row, 1].Text,
                            Constituency_Number = workSheet.Cells[row, 2].Text,
                            District = workSheet.Cells[row, 3].Text,
                            Constituency = workSheet.Cells[row, 4].Text,
                            Latitude = float.Parse(workSheet.Cells[row, 5].Text),
                            Longitude = float.Parse(workSheet.Cells[row, 6].Text),
                            Registered_Voters = int.Parse(workSheet.Cells[row, 7].Text),
                            ValidVotes = int.Parse(workSheet.Cells[row, 8].Text),
                            Voter_TurnOut_Percentage = float.Parse(workSheet.Cells[row, 9].Text),
                            Winner = workSheet.Cells[row, 10].Text,
                            RunnerUp = workSheet.Cells[row, 11].Text,
                            WinnerVotes = int.Parse(workSheet.Cells[row, 12].Text),
                            RunnerUpVotes = int.Parse(workSheet.Cells[row, 13].Text),
                            MarginVotes = int.Parse(workSheet.Cells[row, 14].Text),
                            MarginPercentage = float.Parse(workSheet.Cells[row, 15].Text),
                            Magnitude = int.Parse(workSheet.Cells[row, 16].Text)
                        });
                    }
                }
            }

            foreach (var item in electionSet)
            {
                if(_context.Elections.Where(m => m.Year == item.Year && m.Constituency == item.Constituency).Count() == 0){
                    _context.Add(item);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Elections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var election = await _context.Elections.FindAsync(id);
            if (election == null)
            {
                return NotFound();
            }
            return View(election);
        }

        // POST: Elections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Year,Constituency_Number,District,Constituency,Latitude,Longitude,Registered_Voters,ValidVotes,Voter_TurnOut_Percentage,Winner,RunnerUp,WinnerVotes,RunnerUpVotes,MarginVotes,MarginPercentage,Magnitude")] Election election)
        {
            if (id != election.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(election);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElectionExists(election.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Create));
            }
            return View(election);
        }

        // GET: Elections/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var election = await _context.Elections
                .FirstOrDefaultAsync(m => m.ID == id);
            if (election == null)
            {
                return NotFound();
            }

            return View(election);
        }

        // POST: Elections/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var election = await _context.Elections.FindAsync(id);
            _context.Elections.Remove(election);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Create));
        }
        private bool ElectionExists(int id)
        {
            return _context.Elections.Any(e => e.ID == id);
        }
    }
}
