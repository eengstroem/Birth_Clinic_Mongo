using Library.Models.Rooms;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Library.Models.Reservations
{
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime StartTime { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime EndTime { get; set; }

        public Room Room { get; set; }

        public bool IsEndedEarly { get; set; } = false;

    }
}
