using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
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

        private readonly IOrderService _order;
        private readonly ILogger<OrdersController> _logger;
        public OrdersController(ILogger<OrdersController> logger, IOrderService orderService)
        {
            _order = orderService;
            _logger = logger;
        }

        //[HttpGet("GetOrdById/{userId}")]
        //public List<Order> GetOrdById(int userId)


        //[HttpGet("GetOrdersWithProductsByUserId/{userId}")]
        //public List<Order> GetOrdersWithProductsByUserId(int userId)
        //{
        //    return _order.GetOrdById(userId);
            
        //    return _order.GetOrdersWithProductsByUserId(userId);
        //}

        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<List<Order>>> GetAllOrders()
        {
            return _order.GetAllOrders();
        }

        [HttpPut("PutOrder/{Id}")]
        public async Task<ActionResult<bool>> PutOrder([FromBody] PutOrderObject orderObject)
        {
            return _order.PutOrder(orderObject);
        }
    }
}
