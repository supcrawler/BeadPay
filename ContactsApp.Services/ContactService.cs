using ContactsApp.Models;
using ContactsApp.Models.Abstractions;
using MongoDB.Driver;

namespace ContactsApp.Services;
public class ContactService
{
    private readonly IMongoCollection<Contact> _contacts;

    public ContactService(IContactsDatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        _contacts = database.GetCollection<Contact>(settings.ContactsCollectionName);
    }

    public List<Contact> Get() => _contacts.Find(contact => true).ToList();

    public Contact Get(string id) => _contacts.Find<Contact>(contact => contact.Id == id).FirstOrDefault();

    public Contact Create(Contact contact)
    {
        _contacts.InsertOne(contact);
        return contact;
    }

    public void Update(string id, Contact contactIn) => _contacts.ReplaceOne(contact => contact.Id == id, contactIn);

    public void Remove(Contact contactIn) => _contacts.DeleteOne(contact => contact.Id == contactIn.Id);

    public void Remove(string id) => _contacts.DeleteOne(contact => contact.Id == id);

    public bool IsDuplicateContact(string firstName, string lastName, string contactId = null)
    {
        var filter = Builders<Contact>.Filter.And(
            Builders<Contact>.Filter.Eq(c => c.FirstName, firstName),
            Builders<Contact>.Filter.Eq(c => c.LastName, lastName)
        );

        if (!string.IsNullOrEmpty(contactId))
        {
            filter = Builders<Contact>.Filter.And(
                filter,
                Builders<Contact>.Filter.Ne(c => c.Id, contactId)
            );
        }

        return _contacts.Find(filter).Any();
    }
}
