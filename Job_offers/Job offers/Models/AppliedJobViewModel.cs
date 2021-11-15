using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Job_offers.Models
{
    public class AppliedJobViewModel
    {
        public string JobName { get; set; }
        public IEnumerable<ApplyForJob> Items { get; set; }
    }
}