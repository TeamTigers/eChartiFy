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
using NPOI.XSSF.UserModel;
using System.Net.Http.Headers;
using System.IO;
using NPOI.SS.UserModel;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

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
            if(Regex.IsMatch(yearOrDistrict, @"^\d+$")){
                ViewBag.message = "Index1";
                ViewBag.electionYear = yearOrDistrict;
                return View(await _context.Elections.Where(m => m.Year == yearOrDistrict).ToListAsync());
            }
            else{
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
            XSSFWorkbook workbook;
            IRow rowData;
            HashSet<Election> electionSet = new HashSet<Election>();
            string streamFilePath = null;

            streamFilePath = Path.GetFullPath(ContentDispositionHeaderValue.Parse(files.ContentDisposition).FileName.Trim('"'));
            using (var stream = new FileStream(streamFilePath, FileMode.OpenOrCreate))
            {
                await files.CopyToAsync(stream);
                workbook = new XSSFWorkbook(streamFilePath);
                ISheet sheet = workbook.GetSheet("Sheet1");

                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    rowData = sheet.GetRow(row);
                    if((_context.Elections.Count(m => m.Year == rowData.GetCell(0).ToString()) >0))
                    { return RedirectToAction(nameof(Index)); }
                    
                    electionSet.Add(new Election
                    {
                        Year = rowData.GetCell(0).ToString(),
                        Constituency_Number = rowData.GetCell(1).ToString(),
                        District = rowData.GetCell(2).ToString(),
                        Constituency = rowData.GetCell(3).ToString(),
                        Latitude = float.Parse(rowData.GetCell(4).ToString()),
                        Longitude = float.Parse(rowData.GetCell(5).ToString()),
                        Registered_Voters = int.Parse(rowData.GetCell(6).ToString()),
                        ValidVotes = int.Parse(rowData.GetCell(7).ToString()),
                        Voter_TurnOut_Percentage = float.Parse(rowData.GetCell(8).ToString()),
                        Winner = rowData.GetCell(9).ToString(),
                        RunnerUp = rowData.GetCell(10).ToString(),
                        WinnerVotes = int.Parse(rowData.GetCell(11).ToString()),
                        RunnerUpVotes = int.Parse(rowData.GetCell(12).ToString()),
                        MarginVotes = int.Parse(rowData.GetCell(13).ToString()),
                        MarginPercentage = float.Parse(rowData.GetCell(14).ToString()),
                        Magnitude = int.Parse(rowData.GetCell(15).ToString())
                    });
                }
                stream.Dispose();
            }

            foreach (var item in electionSet)
            {
                _context.Add(item);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        /* In Windows Server 
         *
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            XSSFWorkbook workbook;
            var tempFilePath = Path.GetTempFileName();
            IRow rowData;
            HashSet<Election> electionSet = new HashSet<Election>();
            string streamFilePath = null;

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    streamFilePath = Path.GetFullPath(ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"'));
                    using (var stream = new FileStream(tempFilePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                        
                        workbook = new XSSFWorkbook(streamFilePath);

                        ISheet sheet = workbook.GetSheet("Sheet1");
                        
                        for (int row = 1; row <= sheet.LastRowNum; row++)
                        {
                            rowData = sheet.GetRow(row);
                            if ((_context.Elections.Count(m => m.Year == rowData.GetCell(0).ToString()) > 0))
                            { return RedirectToAction(nameof(Index)); }

                            electionSet.Add(new Election
                            {
                                Year = rowData.GetCell(0).ToString(),
                                Constituency_Number = rowData.GetCell(1).ToString(),
                                District = rowData.GetCell(2).ToString(),
                                Constituency = rowData.GetCell(3).ToString(),
                                Latitude = float.Parse(rowData.GetCell(4).ToString()),
                                Longitude = float.Parse(rowData.GetCell(5).ToString()),
                                Registered_Voters = int.Parse(rowData.GetCell(6).ToString()),
                                ValidVotes = int.Parse(rowData.GetCell(7).ToString()),
                                Voter_TurnOut_Percentage = float.Parse(rowData.GetCell(8).ToString()),
                                Winner = rowData.GetCell(9).ToString(),
                                RunnerUp = rowData.GetCell(10).ToString(),
                                WinnerVotes = int.Parse(rowData.GetCell(11).ToString()),
                                RunnerUpVotes = int.Parse(rowData.GetCell(12).ToString()),
                                MarginVotes = int.Parse(rowData.GetCell(13).ToString()),
                                MarginPercentage = float.Parse(rowData.GetCell(14).ToString()),
                                Magnitude = int.Parse(rowData.GetCell(15).ToString())
                            });
                        }
                    }
                }
            }

            foreach (var item in electionSet)
            {
                _context.Add(item);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Create));
        }

        */


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
