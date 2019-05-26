using GameStore.Domain.Entities;

namespace GameStore.Web.Models
{
    public class CartLine
    {
        public Game Game { get; set; }
        public int Quantity { get; set; }
    }
}
