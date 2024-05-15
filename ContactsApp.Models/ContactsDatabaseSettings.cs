using ContactsApp.Models.Abstractions;

namespace ContactsApp.Models;

public class ContactsDatabaseSettings : IContactsDatabaseSettings
{
    public string ContactsCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
