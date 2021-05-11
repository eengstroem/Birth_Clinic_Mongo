using Library.Models.Births;
using Library.Models.Rooms;
using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Library.Models.Reservations
{
    public class Reservation
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime StartTime { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime EndTime { get;  set; }

        public int ReservedRoomId { get;  set; }

        public bool IsEndedEarly { get; set; } = false;

    }
}
