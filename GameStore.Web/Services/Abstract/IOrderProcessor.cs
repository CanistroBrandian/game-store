using GameStore.Domain.Entities;

namespace GameStore.Web.Services.Abstract
{
    public interface IOrderProcessor
    {
        void ProcessOrder(ShippingDetails shippingDetails);
    }
}
