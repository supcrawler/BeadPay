namespace ContactsApp.Models.Abstractions;
public interface IContactsDatabaseSettings
{
    string ContactsCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}
