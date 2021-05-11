﻿using Library.Models.Births;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Models.Clinicians
{
    public enum ClinicianType
    {
        DOCTOR,
        MIDWIFE,
        NURSE,
        HEALTH_ASSISTANT,
        SECRETARY
    }
    public class Clinician
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }


        public string FirstName { get; set; }


        public string LastName { get; set; }

        public List<int> AssignedBirthsIds { get; set; }

        public ClinicianType Role { get; set; }
    }
}
