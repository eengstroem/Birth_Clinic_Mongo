using Bogus;
using Library.Models.Births;
using Library.Models.Clinicians;
using Library.Models.Reservations;
using System;
using System.Collections.Generic;

namespace Library.Factory.Births
{
    public class BirthFactory
    {
        public static Birth CreateFakeBirth()
        {

            var faker = new Faker("en");
            var o = new Birth()
            {
                BirthDate = faker.Date.Between(DateTime.Now, DateTime.Now.AddDays(10)),
                Reservations = new List<Reservation>(),
                AssociatedClinicians = new List<Clinician>(),
            };
            return o;
        }
    }
}
