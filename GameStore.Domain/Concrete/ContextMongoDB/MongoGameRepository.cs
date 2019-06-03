using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Domain.Concrete.ContextMongoDB
{
    public class MongoGameRepository : IGameRepository
    {
        private readonly MongoContext _context;
        public MongoGameRepository(MongoContext context)
        {
            _context = context;
        }

        public async Task<List<Game>> GetGamesAsync()
        {
            return await _context.GamesMongo.Find(s => true).ToListAsync();
        }

        public List<Game> GetGames()
        {
            return _context.GamesMongo.Find(s => true).ToList();
        }

        public async Task<Game> FindAsync(int gameId)
        {
            return await _context.GamesMongo.AsQueryable().FirstOrDefaultAsync(s => s.GameId == gameId);
            //return await _context.GamesMongo.Find(new BsonDocument("_id", new ObjectId(gameId.ToString()))).FirstOrDefaultAsync();
        }
        public Game Find(int gameId)
        {
            return _context.GamesMongo.AsQueryable().FirstOrDefault(s => s.GameId == gameId);
        }

        public async Task<Game> DeleteGameAsync(int gameId)
        {
            var game = await FindAsync(gameId);
            await _context.GamesMongo.DeleteOneAsync(Builders<Game>.Filter.Eq("_id", game.GameId));
            return game;
        }

        public Game DeleteGame(int gameId)
        {
            var game = Find(gameId);
            _context.GamesMongo.DeleteOne(Builders<Game>.Filter.Eq("_id", game.GameId));
            return game;
        }

        public void SaveGame(Game game)
        {
            if (game.GameId == 0)
            {
                game.GameId = Guid.NewGuid().GetHashCode();
                _context.GamesMongo.InsertOne(game);
                return;
            }
            var gameFromDb = Find(game.GameId);
            gameFromDb.Name = game.Name;
            gameFromDb.Description = game.Description;
            gameFromDb.Price = game.Price;
            gameFromDb.Category = game.Category;
            gameFromDb.ImageData = game.ImageData;
            gameFromDb.ImageMimeType = game.ImageMimeType;
            _context.GamesMongo.ReplaceOne(Builders<Game>.Filter.Eq("_id", game.GameId), game);
        }

        public async Task SaveGameAsync(Game game)
        {
            if (game.GameId == 0)
            {
                game.GameId = Guid.NewGuid().GetHashCode();
                await _context.GamesMongo.InsertOneAsync(game);
                return;
            }
            var gameFromDb = await FindAsync(game.GameId);
            gameFromDb.Name = game.Name;
            gameFromDb.Description = game.Description;
            gameFromDb.Price = game.Price;
            gameFromDb.Category = game.Category;
            gameFromDb.ImageData = game.ImageData;
            gameFromDb.ImageMimeType = game.ImageMimeType;
            await _context.GamesMongo.ReplaceOneAsync(Builders<Game>.Filter.Eq("_id", game.GameId), game);
        }

        public IEnumerable<string> GetCategories()
        {
            return _context.GamesMongo.AsQueryable().Select(game => game.Category)
                  .Distinct()
                  .OrderBy(x => x);

            //_context.GamesMongo.Distinct(nameof(Game.Category))
        }

        public IEnumerable<Game> GetGamesByIds(IEnumerable<int> gameIds)
        {
            var filter = new BsonDocument("_id", new BsonDocument("$in", new BsonArray(gameIds)));
            return _context.GamesMongo.Find(filter).ToList();
            //return _context.GamesMongo.AsQueryable().Where(g => gameIds.Contains(g.GameId)).ToList();
        }

        public async Task<IEnumerable<Game>> GetGamesByIdsAsync(IEnumerable<int> gameIds)
        {
            var filter = new BsonDocument("_id", new BsonDocument("$in", new BsonArray(gameIds)));
            return await _context.GamesMongo.Find(filter).ToListAsync();
            //return await _context.GamesMongo.AsQueryable().Where(g => gameIds.Contains(g.GameId)).ToListAsync();
        }

        public IEnumerable<Game> GetByCategory(string category, int page, int pageSize)
        {
            return _context.GamesMongo.AsQueryable().Where(p => category == null || p.Category == category)
                    .OrderBy(game => game.GameId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);
        }

        public int GetCount()
        {
            return _context.GamesMongo.AsQueryable().Count();
        }

        public int GetCountByCategory(string category)
        {
            return _context.GamesMongo.AsQueryable().Where(game => game.Category == category).Count();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.GamesMongo.AsQueryable().CountAsync();
        }

        public async Task<int> GetCountByCategoryAsync(string category)
        {
            return await _context.GamesMongo.AsQueryable().Where(game => game.Category == category).CountAsync();
        }
    }
}
