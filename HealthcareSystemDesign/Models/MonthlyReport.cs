﻿using System;
using System.Collections.Generic;

namespace HealthcareSystemDesign.Models
{
    public partial class MonthlyReport
    {
        public int ReportId { get; set; }
        public DateTime ReportMonth { get; set; }
        public int NoPatients { get; set; }
        public string DoctorName { get; set; }
        public double MonthlyIncome { get; set; }
    }
}
