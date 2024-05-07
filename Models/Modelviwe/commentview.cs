using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace forum.Models.Modelviwe
{
    public class commentview
    {
        [Key]
       public int id{get;set;}
       public string title {get;set;}
       public string Content {get;set;}
       public string username {get;set;}
       public string image { get; set; }
       public short? position { get; set; }
       
    }
}