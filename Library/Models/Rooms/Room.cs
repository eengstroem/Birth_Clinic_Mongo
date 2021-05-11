using Library.Models.Reservations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Models.Rooms
{
    public enum RoomType
    {
        MATERNITY,
        BIRTH,
        REST
    }

    public class Room
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }

        [Required]
        public RoomType RoomType { get; set; }

        public List<int> ReservationIds { get; set; }

    }
}
