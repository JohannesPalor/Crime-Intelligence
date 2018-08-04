using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CIS.Models
{
    public class UserModel
    {

        [Key]
        public int UserID { get; set; }

        public string UserType { get; set; }


        public int TypeId { get; set; }

        public List<UserTypesModel> UserTypes { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Phone { get; set; }
    }
}