using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models.Births;
using Library.Models.Clinicians;

namespace Library.Services
{
    interface IClinicianService
    {

        Task<List<Clinician>> FindAvailableClinicians(ClinicianType role, Birth birth, int RequiredDelta, int AllowedOccurences);
    }
}
