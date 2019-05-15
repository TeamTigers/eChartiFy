using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using electionDbAnalytics.Data;
using electionDbAnalytics.Models;

namespace electionDbAnalytics.Controllers
{
    public class AuditsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuditsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Audits
        public async Task<IActionResult> Index()
        {
            return View(await _context.Audit.ToListAsync());
        }

        // GET: Audits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditLogging = await _context.Audit
                .FirstOrDefaultAsync(m => m.ID == id);
            if (auditLogging == null)
            {
                return NotFound();
            }

            return View(auditLogging);
        }

        // POST: Audits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var auditLogging = await _context.Audit.FindAsync(id);
            _context.Audit.Remove(auditLogging);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuditLoggingExists(int id)
        {
            return _context.Audit.Any(e => e.ID == id);
        }
    }
}
