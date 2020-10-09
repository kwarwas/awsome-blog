using MongoDB.Driver;

namespace AwesomeBlog.Infrastructure
{
    public class DatabaseContext
    {
        private readonly IMongoDatabase _database;

        public DatabaseContext()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("AwesomeBlog");
        }

        public IMongoCollection<TModel> GetCollection<TModel>() => _database.GetCollection<TModel>(typeof(TModel).Name);
    }
}