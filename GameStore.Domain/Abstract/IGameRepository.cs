using GameStore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.Domain.Abstract
{
    public interface IGameRepository
    {
        List<Game> GetGames();
        Task<List<Game>> GetGamesAsync();
        Game Find(int gameId);
        Task<Game> FindAsync(int gameId);
        void SaveGame(Game game);
        Task SaveGameAsync(Game game);
        Game DeleteGame(int gameId);
        Task<Game> DeleteGameAsync(int gameId);
        IEnumerable<string> GetCategories();
        IEnumerable<Game> GetGamesByIds(IEnumerable<int> gameIds);
        Task<IEnumerable<Game>> GetGamesByIdsAsync(IEnumerable<int> gameIds);
        IEnumerable<Game> GetByCategory(string category, int page, int pageSize);
        int GetCount();
        int GetCountByCategory(string category);
        Task<int> GetCountAsync();
        Task<int> GetCountByCategoryAsync(string category);

    }
}
