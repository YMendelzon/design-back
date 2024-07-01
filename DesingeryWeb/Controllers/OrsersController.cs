using DesigneryCommon.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
namespace DesingeryWeb.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly List<Order> orders = new List<Order>
    {
        new Order { Status = "", CreatedAt =new DateTime(2024, 6, 30, 14, 30, 0) , UserID = 1, TotalAmount =300, OrderID=1 },
        new Order { Status = "", CreatedAt = new DateTime(2024, 6, 30, 14, 30, 0), UserID = 1, TotalAmount = 100, OrderID=2 }
    };

        //return for user his orders history
        [HttpGet("{userId}")]
        public ActionResult<IEnumerable<Order>> GetHistoryOrdersByUserId(int userId)
        {
            var userOrders = orders.Where(o => o.UserID == userId).ToList();
            return Ok(userOrders);
        }
    }
}
