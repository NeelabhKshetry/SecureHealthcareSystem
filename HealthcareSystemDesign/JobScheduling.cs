using HealthcareSystemDesign.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthcareSystemDesign
{
    public class JobScheduling
    {
        private readonly healthcareContext _context;

        public JobScheduling(healthcareContext context)
        {
            _context = context;
        }
        public static DateTime Now { get; }

        public void ClearAppointment()
        {

            /*DateTime localdate = DateTime.Now;

            var appts = _context.Appointment
                 .FromSqlRaw($"DELETE from Appointment where appt_details < \'2020-04-25\' ");*/

        }
    }
}
