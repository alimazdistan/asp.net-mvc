//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
namespace forum.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Cats = new HashSet<Cat>();
            this.Comments = new HashSet<Comment>();
            this.Topics = new HashSet<Topic>();
        }
    
        public int ID { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "*")]
        public string Email { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> RegistDate { get; set; }
        public Nullable<System.DateTime> LastActivity { get; set; }
        public string Image { get; set; }
        public Nullable<short> RoleID { get; set; }
    
        public virtual ICollection<Cat> Cats { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
    }
}
