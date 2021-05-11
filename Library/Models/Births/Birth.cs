using Library.Models.Clinicians;
using Library.Models.FamilyMembers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Models.Reservations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Models.Births
{
    public class Birth
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime BirthDate { get; set; }

        public List<int> AssociatedClinicians { get; set; }

        public List<Child> ChildrenToBeBorn { get; set; }

        public Mother Mother { get; set; }

        public bool IsEnded { get; set; }

        public Father Father { get; set; }

        public List<Relative> Relatives { get; set; }

        public List<Reservation> Reservations { get; set; }


    }
}
