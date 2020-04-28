using System;
using System.Collections.Generic;

namespace HealthcareSystemDesign.Models
{
    public partial class BillingType
    {
        public int BillingtypeId { get; set; }
        public string BillingName { get; set; }
        public int BillingId { get; set; }

        public virtual Billing Billing { get; set; }
    }
}
