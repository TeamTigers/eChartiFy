using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace electionDbAnalytics.Models
{
    public class Election
    {
        public int ID { get; set; }
        [Required]
        public string Year { get; set; }
        [Required]
        public string Constituency_Number { get; set; }
        [Required]
        public string District { get; set; }
        [Required]
        public string Constituency { get; set; }
        [Required]
        public float Latitude { get; set; }
        [Required]
        public float Longitude { get; set; }
        [Required]
        public int Registered_Voters { get; set; }
        [Required]
        public int ValidVotes { get; set; }
        [Required]
        public float Voter_TurnOut_Percentage { get; set; }
        [Required]
        public string Winner { get; set; }
        [Required]
        public string RunnerUp { get; set; }
        [Required]
        public int WinnerVotes { get; set; }
        [Required]
        public int RunnerUpVotes { get; set; }
        [Required]
        public int MarginVotes { get; set; }
        [Required]
        public float MarginPercentage { get; set; }
        [Required]
        public int Magnitude { get; set; }
    }
}
