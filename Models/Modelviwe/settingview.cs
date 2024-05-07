using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace forum.Models.Modelviwe
{
    
    public class settingview
    {
        [Required(ErrorMessage = "*")]
        public  int perpage {
            get
            {
                return setting.perpage;
            }
            set
            {
                setting.perpage = value;
            }
        }
        [Required(ErrorMessage = "*")]
        public string sitename
        {
            get
            {
                return setting.sitename;
            }
            set
            {
                setting.sitename = value;
            }
        }
        [Required(ErrorMessage = "*")]
        public string keywords
        {
            get
            {
                return setting.keywords;
            }
            set
            {
                setting.keywords = value;
            }
        }
        [Required(ErrorMessage = "*")]
        public string descriptions
        {
            get
            {
                return setting.descriptions;
            }
            set
            {
                setting.descriptions = value;
            }
        }
        
    }
}