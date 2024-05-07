using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace forum.Models.Modelviwe
{
    public class userview
    {
        [Key]
        public int ID { get; set; }
        [Required (ErrorMessage="*")]
        [StringLength (maximumLength:50,MinimumLength=3,ErrorMessage="max=50 and min=3")] 
        public string Name { get; set; }
        [Required (ErrorMessage="*")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "max=50 and min=3")] 
        public string LastName { get; set; }
       [Required(ErrorMessage = "*")]
        [DataType (dataType:DataType.Password)]
        [StringLength(maximumLength: 50, MinimumLength = 8, ErrorMessage = "max=50 and min=8")] 
        public string Password { get; set; }
       [Required(ErrorMessage = "*")]
       [DataType(dataType: DataType.Password)]
       [StringLength(maximumLength: 50, MinimumLength = 8, ErrorMessage = "max=50 and min=8")]
        [Compare (otherProperty:"Password")]
       public string RePassword { get; set; }
         [Required(ErrorMessage = "*")]
        
        [StringLength(maximumLength: 50, MinimumLength = 6, ErrorMessage = "max=50 and min=6")] 
        public string UserName { get; set; }
         [Required(ErrorMessage = "*")]
        [EmailAddress (ErrorMessage="enter email format")]
        public string Email { get; set; }
        public HttpPostedFileBase Image{get;set;}
    }
   
}