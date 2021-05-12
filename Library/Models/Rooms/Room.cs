using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

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
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        public RoomType RoomType { get; set; }

    }
}
