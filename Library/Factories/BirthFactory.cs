﻿using Bogus;
using Library.Models.Births;
using System;

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
            };
            return o;
        }
    }
}
