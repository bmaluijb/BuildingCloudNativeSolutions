using NationalCookies.Data.Interfaces;
using System;
using System.Collections.Generic;


namespace NationalCookies.Data.Services
{
    public class OrderService : IOrderService
    {

        private readonly CosmosDBConnector _cosmos;

        public OrderService(CosmosDBConnector cosmos)
        {
            this._cosmos = cosmos;
        }

        /// <summary>
        /// Adds cookies to orders
        /// </summary>
        /// <param name="cookieId"></param>
        public void AddCookieToOrder(Guid cookieId)
        {
            this._cosmos.AddCookieToOrder(cookieId);
        }

        /// <summary>
        /// gets all orders
        /// </summary>
        /// <returns></returns>
        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();

            orders = this._cosmos.RetrieveAllOrders();

            return orders;
        }

        /// <summary>
        /// gets a specific order by id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Order GetOrderById(Guid orderId)
        {
            //get the order by id
            Order order = new Order();

            order = this._cosmos.RetrieveOrderById(orderId);

            return order;
        }



        /// <summary>
        /// deletes an order
        /// </summary>
        /// <param name="orderId"></param>
        public void CancelOrder(Guid orderId)
        {
            this._cosmos.CancelOrder(orderId);
        }

        /// <summary>
        /// places an order by chaning it status to "Placed"
        /// </summary>
        /// <param name="orderId"></param>
        public void PlaceOrder(Guid orderId)
        {
            this._cosmos.PlaceOrder(orderId);
        }
    }

}
