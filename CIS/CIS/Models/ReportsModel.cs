using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace CIS.Models
{
    public class ReportsModel
    {
        [Key]
        [Required]
        public int CrimeId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime time { get; set; }

        [Required]
        public decimal longitude { get; set; }

        [Required]
        public decimal latitude { get; set; }

        [Required]
        public bool is_verified { get; set; }


        public int votes { get; set; }

        [Display(Name = "Crime Details")]
        [DataType(DataType.MultilineText)]
        public string incident_details { get; set; }

        public int crime_rating { get; set; }

        [Required]
        public int user_id { get; set; }
    }
}