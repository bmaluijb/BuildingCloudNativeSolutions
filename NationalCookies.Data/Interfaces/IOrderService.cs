using System;
using System.Collections.Generic;

namespace NationalCookies.Data.Interfaces
{
    public interface IOrderService
    {
        void AddCookieToOrder(Guid cookieId);

        List<Order> GetAllOrders();

        Order GetOrderById(Guid orderId);

        void CancelOrder(Guid orderId);

        void PlaceOrder(Guid orderId);

    }
}
