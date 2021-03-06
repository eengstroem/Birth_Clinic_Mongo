using Library.Models.Clinicians;
using Library.Models.FamilyMembers;
using Library.Models.Reservations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Library.Models.Births
{
    public class Birth
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime BirthDate { get; set; }

        public List<Clinician> AssociatedClinicians { get; set; }

        public List<Child> ChildrenToBeBorn { get; set; }

        public Mother Mother { get; set; }

        public bool IsEnded { get; set; }

        public Father Father { get; set; }

        public List<Relative> Relatives { get; set; }

        public List<Reservation> Reservations { get; set; }


    }
}
