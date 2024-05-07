using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace forum.Models.Modelviwe
{
    public class userpasseditview
    {
        
        
          
            [Required(ErrorMessage = "*")]
            [DataType(dataType: DataType.Password)]
            [StringLength(maximumLength: 50, MinimumLength = 8, ErrorMessage = "max=50 and min=8")]
            public string password { get; set; }
            [Required(ErrorMessage = "*")]
            [DataType(dataType: DataType.Password)]
            [StringLength(maximumLength: 50, MinimumLength = 8, ErrorMessage = "max=50 and min=8")]
            public string newpassword { get; set; }
            [Required(ErrorMessage = "*")]
            [DataType(dataType: DataType.Password)]
            [StringLength(maximumLength: 50, MinimumLength = 8, ErrorMessage = "max=50 and min=8")]
            [Compare(otherProperty: "newpassword")]
            public string renewpassword { get; set; }
        
    }
}