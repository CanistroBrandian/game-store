using System.Collections.Generic;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Abstract
{
    public interface IGameRepository
    {
        IEnumerable<Game> Games { get; }
        void SaveGame(Game game);
        Game DeleteGame(int gameId);
<<<<<<< Migrating_to_core
=======
        Task<Game> DeleteGameAsync(int gameId);
        IEnumerable<string> GetCategories();
        Task <IEnumerable<string>> GetCategoriesAsync();
        IEnumerable<Game> GetGamesByIds(IEnumerable<int> gameIds);
        Task<IEnumerable<Game>> GetGamesByIdsAsync(IEnumerable<int> gameIds);
        IEnumerable<Game> GetByCategory(string category, int page, int pageSize);
        int GetCount();
        Task<int> GetCountAsync();
        int GetCountByCategory(string category);
        Task<int> GetCountByCategoryAsync(string category);


>>>>>>> local
    }
}
