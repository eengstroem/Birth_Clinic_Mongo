using Bogus;
using Library.Models.Births;
using Library.Models.Clinicians;
using System.Collections.Generic;

namespace Library.Factory.Clinicians
{
    class ClinicianFactory
    {
        public static Clinician CreateFakeClinician(ClinicianType type)
        {

            var faker = new Faker("en");
            var o = new Clinician()
            {
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName(),
                Role = type
            };
            return o;
        }
    }
}
