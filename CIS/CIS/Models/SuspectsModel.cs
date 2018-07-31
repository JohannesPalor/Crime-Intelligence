using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CIS.Models
{
    public class SuspectsModel
    {

        [Key]        
        public int suspect_id { get; set; }

        public int crime_id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

       
        public int Face_Shape { get; set; }


        [Display(Name = "Hair Style")]
        [Required(ErrorMessage = "Required field")]
        public int Hair_Style { get; set; }

        public List<HairTypeViewModel> HairTypes { get; set; }

        [MaxLength(50)]
        [Display(Name = "Prominent Facial Feature")]
        public string Prominent_Facial_Feature{ get; set; }

        [MaxLength(50)]
        [Display(Name = "Body Built")]
        public string Body_Built { get; set; }

        [MaxLength(50)]
        [Display(Name = "Shirt Color")]
        public string Shirt_Color { get; set; }

        [MaxLength(50)]
        [Display(Name = "Tattoo Location")]
        public string Tattoo_Location { get; set; }

        [MaxLength(50)]
        public string is_Armed { get; set; }

        [MaxLength(50)]
        [Display(Name = "Type of Weapon")]
        public string Type_of_Weapon { get; set; }

        [MaxLength(50)]
        [Display(Name = "Other Description")]
        [DataType(DataType.MultilineText)]
        public string Other_Description{ get; set; }
    }
}