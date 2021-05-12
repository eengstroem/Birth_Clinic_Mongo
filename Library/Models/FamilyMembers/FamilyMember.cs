using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Models.FamilyMembers
{

    public abstract class FamilyMember
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}
