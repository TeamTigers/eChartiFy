using System;
using System.Collections.Generic;
using System.Text;
using electionDbAnalytics.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace electionDbAnalytics.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Election> Elections { get; set; }
        public DbSet<AuditLogging> Audit { get; set; }
    }
}
