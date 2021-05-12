using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models.Births;
using Library.Models.Clinicians;
using Library.Repository;

namespace Library.Services
{
    class ClinicianService
    {
        private readonly IClinicianRepository _clinicianRepo;

        public ClinicianService(IClinicianRepository clinicianRepo)
        {
            _clinicianRepo = clinicianRepo;
        }
    }
}
