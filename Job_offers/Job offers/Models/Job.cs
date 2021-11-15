using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace Job_offers.Models
{
    public class Job
    {
        public int ID { get; set; }
        
        [DisplayName("أسم الوظيفة")]
        [Required]
        public string JobName { get; set; }
        
        [Required]
        [DisplayName("وصف الوظيفة")]
        public string JobDescription { get; set; }

        [DisplayName("صورة الوظيفة")]
        public string JobImage { get; set; }

        [DisplayName("وصف الوظيفة")]
        public int CategoryID { get; set; }

        //[DisplayName("")]
        public string UserID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }
    }
}