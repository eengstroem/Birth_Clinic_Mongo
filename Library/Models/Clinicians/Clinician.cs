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
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ClinicianType Role { get; set; }
    }
}
