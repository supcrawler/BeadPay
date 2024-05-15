using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContactsApp.Models;
public class Contact
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("FirstName")]
    public string FirstName { get; set; }

    [BsonElement("LastName")]
    public string LastName { get; set; }

    [BsonElement("Email")]
    public string Email { get; set; }

    [BsonElement("PhoneNumbers")]
    public List<string> PhoneNumbers { get; set; }
}
