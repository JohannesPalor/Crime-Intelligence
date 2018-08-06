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
        public string Image { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name ="Date of Incident")]
        public DateTime time { get; set; }

        public List<CrimeTypesModel> CrimeTypes{ get; set; }

        [Display(Name = "Crime Type")]
        public int CrimeTypeId { get; set; }

        public string CrimeName { get; set; }
        public int SuspectCount { get; set; }
        [Required]
        public string longitude { get; set; }

        [Required]
        public string latitude { get; set; }

        [Required]
        public bool is_verified { get; set; }


        public int votes { get; set; }

        [Display(Name = "Crime Details")]
        [DataType(DataType.MultilineText)]
        public string incident_details { get; set; }

        public int crime_rating { get; set; }

        [Required]
        public int user_id { get; set; }

        public List<ReportsModel> ReportsList { get; set; }
        public List<SuspectsModel> SuspectsList { get; set; }

        public UserModel Login { get; set; }
    }
}