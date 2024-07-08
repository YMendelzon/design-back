using DesigneryCommon.Models;

namespace DesigneryCore.Interfaces
{
    public interface IOrderItemService
    {
        List<OrderItem> GetAllOrderItems();
        List<OrderItem> GetOrderItemByOrdId(int orderId);
        bool PostOrderItem(OrderItem o);
    }
}