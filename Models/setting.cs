using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace forum.Models
{
    public static class setting
    {
        [Required (ErrorMessage="*")]
        public static int perpage = 10;
        [Required(ErrorMessage = "*")]
        public static string sitename = "mysite";
        [Required(ErrorMessage = "*")]
        public static string keywords = "mykeywords";
        [Required(ErrorMessage = "*")]
        public static string descriptions = "mydescription";

    }
}