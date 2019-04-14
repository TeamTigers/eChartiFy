using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace electionDbAnalytics.Models
{
    public class AuditLogging
    {
        public int ID { get; set; }
        
        [Required]
        public string IPAddress { get; set; }
        
        [Required]
        public string Location { get; set; }
        
        [Required, DataType(DataType.DateTime)]
        public DateTime TimeOfAction { get; set; }

        
    }
}
