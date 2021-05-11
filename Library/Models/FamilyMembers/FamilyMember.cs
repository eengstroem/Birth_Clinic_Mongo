﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Models.FamilyMembers
{

    public abstract class FamilyMember
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}