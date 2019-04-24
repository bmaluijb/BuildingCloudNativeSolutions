using Microsoft.AspNetCore.Mvc;
using NationalCookies.Data.Interfaces;
using System;

namespace NationalCookies.Controllers
{

    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;


        public OrderController(IOrderService orderService)
        {
            this._orderService = orderService;
        }

        public IActionResult Index()
        {
            System.Collections.Generic.List<Data.Order> orders 
                = this._orderService.GetAllOrders();

            return View(orders);
        }


        public IActionResult Detail(Guid id)
        {
            Data.Order order = this._orderService.GetOrderById(id);

            return View(order);
        }

        public IActionResult CancelOrder(Guid id)
        {
            this._orderService.CancelOrder(id);

            return RedirectToAction("Index");
        }

        public IActionResult PlaceOrder(Guid id)
        {
            this._orderService.PlaceOrder(id);

            return RedirectToAction("Index");
        }

        public IActionResult AddCookieToOrderLine(Guid cookieId)
        {
            this._orderService.AddCookieToOrder(cookieId);

            return RedirectToAction("Index", "Order");
        }
    }
}
