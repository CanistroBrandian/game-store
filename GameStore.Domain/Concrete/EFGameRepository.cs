using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using System.Collections.Generic;

namespace GameStore.Domain.Concrete
{
    public class EFGameRepository : IGameRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Game> Games
        {
            get { return context.Games; }
        }
        public void SaveGame(Game game)
        {
            if (game.GameId == 0)
                context.Games.Add(game);
            else
            {
                Game dbEntry = context.Games.Find(game.GameId);
                if (dbEntry != null)
                {
                    dbEntry.Name = game.Name;
                    dbEntry.Description = game.Description;
                    dbEntry.Price = game.Price;
                    dbEntry.Category = game.Category;
                    dbEntry.ImageData = game.ImageData;
                    dbEntry.ImageMimeType = game.ImageMimeType;
                }
            }
            context.SaveChanges();
        }
        public Game DeleteGame(int gameId)
        {
            Game dbEntry = context.Games.Find(gameId);
            if (dbEntry != null)
            {
                context.Games.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
<<<<<<< Migrating_to_core
=======

        public async Task<Game> DeleteGameAsync(int gameId)
        {
            Game dbEntry = _context.Games.Find(gameId);
            if (dbEntry != null)
            {
                _context.Games.Remove(dbEntry);
                await _context.SaveChangesAsync();
            }
            return dbEntry;
        }

        public List<Game> GetGames()
        {
            return _context.Games.AsNoTracking().ToList();
        }

        public async Task<List<Game>> GetGamesAsync()
        {
            return await _context.Games.AsNoTracking().ToListAsync();
        }

        public Game Find(int gameId)
        {
            return _context.Games.AsNoTracking().FirstOrDefault(s => s.GameId == gameId);
        }

        public Task<Game> FindAsync(int gameId)
        {
            return _context.Games.AsNoTracking().FirstOrDefaultAsync(s => s.GameId == gameId);
        }

        public IEnumerable<string> GetCategories()
        {
            return _context.Games.Select(game => game.Category)
                .Distinct()
                .OrderBy(x => x);
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            return await _context.Games.Select(game => game.Category)
                .Distinct()
                .OrderBy(x => x).AsNoTracking().ToListAsync();
        }
        public IEnumerable<Game> GetGamesByIds(IEnumerable<int> gameIds)
        {
            return _context.Games.AsNoTracking().Where(g => gameIds.Contains(g.GameId)).ToList();
        }

        public async Task<IEnumerable<Game>> GetGamesByIdsAsync(IEnumerable<int> gameIds)
        {
            return await _context.Games.AsNoTracking().Where(g => gameIds.Contains(g.GameId)).ToListAsync();
        }

        public IEnumerable<Game> GetByCategory(string category, int page, int pageSize)
        {
            return _context.Games.Where(p => category == null || p.Category == category)
                    .OrderBy(game => game.GameId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);
        }

        public int GetCount()
        {
            return _context.Games.Count();
        }
        public async Task<int> GetCountAsync()
        {
            return await _context.Games.AsNoTracking().CountAsync();
        }

        public int GetCountByCategory(string category)
        {
            return _context.Games.Where(game => game.Category == category).Count();
        }

        public async Task<int> GetCountByCategoryAsync(string category)
        {
            return  await _context.Games.Where(game => game.Category == category).AsNoTracking().CountAsync();
        }
>>>>>>> local
    }
}
