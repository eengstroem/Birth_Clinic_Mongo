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
        private readonly IBirthRepository _birthRepo;

        public ClinicianService(IClinicianRepository clinicianRepo, IBirthRepository birthRepo)
        {
            _clinicianRepo = clinicianRepo;
            _birthRepo = birthRepo;
        }
        public async Task<List<Clinician>> FindAvailableClinicians(ClinicianType role, Birth birth, int RequiredDelta, int AllowedOccurences)
        {
            var _births = _birthRepo.GetAll();
            var clinicians = _clinicianRepo.GetAll().Where(c => c.Role == role);

            List<Clinician> availableClinicians = new();

            foreach (var b in _births)
            {
                availableClinicians.AddRange(
                    clinicians.TakeWhile(c => c.AssignedBirthsIds.Contains(b.Id))
                        .Where(c => ((birth.BirthDate - b.BirthDate).TotalDays - (birth.BirthDate - b.BirthDate).Days) * 60 >= RequiredDelta * 60));
            }

            return availableClinicians;
        }
    }
}
