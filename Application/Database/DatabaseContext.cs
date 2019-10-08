using Application.Models;
using Application.Models.Entities;
using Application.Utility.Database;
using MongoDB.Driver;

namespace Application.Database
{
    public interface IDatabaseContext
    {
        IMongoCollection<Collaboration> Collaborations { get; }

        bool IsConnectionOpen();
    }

    public class DatabaseContext : IDatabaseContext
    {
        private readonly IMongoDatabase _database;

        public DatabaseContext(IDatabaseSettings settings)
        {
            var mongoSettings = settings.GetSettings();
            var client = new MongoClient(mongoSettings);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<Collaboration> Collaborations => _database.GetCollection<Collaboration>("Collaborations");

        public bool IsConnectionOpen()
        {
            return _database != null;
        }
    }
}