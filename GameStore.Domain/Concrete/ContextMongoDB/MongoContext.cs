using GameStore.Domain.Entities;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace GameStore.Domain.Concrete.ContextMongoDB
{
    public  class MongoContext 
    {
        IMongoDatabase database; // база данных
        IGridFSBucket gridFS;

        public MongoContext(string connectionString)
        {
            //string connectionString = "mongodb://localhost:27017/";
            var connection = new MongoUrlBuilder(connectionString);
            // получаем клиента для взаимодействия с базой данных
            MongoClient client = new MongoClient(connectionString);
            // получаем доступ к самой базе данных
            database = client.GetDatabase(connection.DatabaseName);
            // получаем доступ к файловому хранилищу
            gridFS = new GridFSBucket(database);
          
        }

        public IMongoCollection<Game> GamesMongo
        {
            get { return database.GetCollection<Game>("GamesMongo"); }
        }
    }
}
