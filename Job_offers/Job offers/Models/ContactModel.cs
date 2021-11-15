using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Job_offers.Models
{
    public class ContactModel
    {
        [Required]
        [DisplayName("الأسم")]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        [DisplayName("البريد الإلكترونى")]
        public string Email { get; set; }

        [Required]
        [DisplayName("عنوان الرسالة")]
        public string Subject { get; set; }
        
        [Required]
        [DisplayName("نص الرسالة")]
        [AllowHtml]
        public string Body { get; set; }

    }
}