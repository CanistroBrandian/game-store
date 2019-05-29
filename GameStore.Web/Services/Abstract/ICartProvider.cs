using GameStore.Domain.Entities;
using GameStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Web.Services.Abstract
{
    public interface ICartProvider
    {
        void AddItem(Game game, int quantity);
       
        void RemoveLine(Game game);

        decimal ComputeTotalValue();

        void Clear();

        IEnumerable<CartLine> Lines { get; }

    }   
}
