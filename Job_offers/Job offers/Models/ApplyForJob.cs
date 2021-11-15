using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace Job_offers.Models
{
    public class ApplyForJob
    {
        public int  ID { get; set; }

        [DisplayName("نص الرسالة")]
        [AllowHtml]
        public string Message { get; set; }
        
        [DisplayName("تاريخ التقدم")]
        public DateTime ApplyDate { get; set; }
        public int JobId { get; set; }
        public string UserId { get; set; }

        [ForeignKey("JobId")]
        public virtual Job job { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser user { get; set; }
    }
}